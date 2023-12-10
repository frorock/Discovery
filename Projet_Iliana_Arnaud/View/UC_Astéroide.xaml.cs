using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Threading;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using Projet_Iliana_Arnaud.Services;

namespace Projet_Iliana_Arnaud.View
{
    // Définition de la classe Earth qui représente la Terre
    public class Earth
    {
        public double Radius { get; set; }
        public Point3D Center { get; set; }
        public DiffuseMaterial Material { get; set; }
    }

    public partial class Astéroide : System.Windows.Controls.UserControl
    {
        // Instanciation de l'objet api_asteroides pour accéder à l'API des astéroïdes
        private readonly API_Asteroides api_asteroides = new API_Asteroides();

        private DispatcherTimer timer; // Déclaration du timer

        private double angle = 0;   // Initialisation de la variable asteroide
        private double angleterre = 0; // Initialisation de la variable terre


        private RotateTransform3D rotationTransform; // Transformation de rotation de la Terre

        // Propriétés des astéroïdes
        public string Name;
        public string Description;
        public string Dernière_Date;
        public string Première_Date;
        public string Distance;
        public int  Diameter;
        public int Vitesse_KMH;


        // Création du modèle 3D de la Terre
        private SphereVisual3D earth = new SphereVisual3D
        {
            Radius = 1.2,
            Center = new Point3D(0, 0, 0),
            Material = new DiffuseMaterial
            {
                Brush = new ImageBrush(new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Texture_Terre.jpg", UriKind.Relative)))
            }
        };

        public Astéroide()
        {
            InitializeComponent();

            // Ajout de la Terre à la scène
            Viewport3D.Children.Add(earth);

            // Initialisation de la transformation de rotation de la Terre
            rotationTransform = new RotateTransform3D();
            rotationTransform.Rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleterre);
            earth.Transform = rotationTransform;


            // Initialisation du timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10); // Intervalle de temps de 10 millisecondes
            timer.Tick += Timer_Tick; // Abonnement à l'événement Tick
            timer.Start(); // Démarrage du timer
        }

        // Listes qui stockeront les modèles 3D des astéroïdes
        private List<SphereVisual3D> asteroidModels = new List<SphereVisual3D>();
        private AsteroidData selectedAsteroid;


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Modification de la valeur pour ajuster la vitesse de rotation de la Terre
            angleterre += 0.01;
            // Application de la rotation à la Terre
            rotationTransform.Rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleterre);
            earth.Transform = rotationTransform;

            // Pour chaque astéroïde dans les données récupérées de l'API
            foreach (AsteroidData asteroid in api_asteroides.asteroidData)
            {
                // Récupération des données de l'astéroïde
                Name = asteroid.Name;
                Dernière_Date = asteroid.Last_Date;
                Première_Date = asteroid.First_Date;
                Description = asteroid.Description;
                Distance = asteroid.Distance;
                string Longitude_ecliptique = asteroid.Longitude_ecliptique;
                string Latitude_ecliptique = asteroid.Latitude_ecliptique;
                Diameter = asteroid.Diameter;
                Vitesse_KMH = asteroid.Velocity;
                int Inclinaison = asteroid.Inclinaison;

                // Ajout du nom de l'astéroïde à la listbox
                ListName.Items.Add(Name);

                // Mise à jour de l'angle de rotation de l'astéroïde en fonction de sa vitesse et de son inclinaison
                angle += (Vitesse_KMH / 3600) * (Math.PI / 180) / Inclinaison;

                angleterre += 0.01;  // Modification de la valeur pour ajuster la vitesse de rotation de la Terre


                // Récupération des coordonnées cartésiennes de l'astéroïde à partir de l'angle de rotation
                double x = asteroid.x * 2;
                double y = asteroid.y * 2;
                double z = asteroid.z * 2;

                // Création d'un modèle 3D de l'astéroïde en utilisant un visualiseur de sphère
                var asteroidModel = new SphereVisual3D();
                asteroidModel.Radius = (Diameter * 0.004);
                asteroidModel.Center = new Point3D(x, y, z);

                // Création de la transformation de rotation de l'astéroïde
                var asteroidRotationTransform = new RotateTransform3D();
                asteroidRotationTransform.Rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle);



                // Appliquer la transformation de rotation à l'astéroïde
                asteroidModel.Transform = asteroidRotationTransform;

                // Ajout de la texture aux astéroides
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Texture_Asteroide.jpg", UriKind.Relative));

                // Utilisez l'objet ImageBrush pour remplir la sphère
                asteroidModel.Fill = imageBrush;


                // Suppression de l'ancien astéroïde s'il existe dans la vue 3D
                SphereVisual3D oldAsteroidModel = asteroidModels.FirstOrDefault(a => a.Center == asteroidModel.Center);
                if (oldAsteroidModel != null)
                {
                    Viewport3D.Children.Remove(oldAsteroidModel);
                    asteroidModels.Remove(oldAsteroidModel);
                }

                // Ajout du nouvel astéroïde à la liste
                asteroidModels.Add(asteroidModel);

                // Ajout du nouvel astéroïde à la vue 3D
                Viewport3D.Children.Add(asteroidModel);

            }
            // Si un astéroïde a été sélectionné dans la listbox
            if (selectedAsteroid != null)
            {
                // Récupération de l'index de l'astéroïde sélectionné dans la liste des astéroïdes
                int asteroidIndex = api_asteroides.asteroidData.IndexOf(selectedAsteroid);

                // Récupération du modèle 3D de l'astéroïde sélectionné
                var selectedAsteroidModel = asteroidModels[asteroidIndex];

                // Modification de la couleur de l'astéroïdeen rouge
                selectedAsteroidModel.Material = new DiffuseMaterial(Brushes.Red);
            }
        }        


        private void ListName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Récupération de l'astéroïde sélectionné à partir de la liste api_asteroides.asteroidData
            selectedAsteroid = api_asteroides.asteroidData.FirstOrDefault(a => a.Name == ListName.SelectedItem.ToString());

            // Mise à jour des informations .Text avec les données de l'astéroïde sélectionné
            Date1.Text = "First Observation Date : " + selectedAsteroid.First_Date;
            Date2.Text = "Last Observation Date : " + selectedAsteroid.Last_Date;
            Desc.Text = "Description : " + selectedAsteroid.Description;
            Dist.Text = "Earth Distance : " + selectedAsteroid.Distance + " KM";
            Diam.Text = "Diameter : " + selectedAsteroid.Diameter + " KM";
            Vit.Text = "Speed : " + selectedAsteroid.Velocity + " KM/H" ;

        }

    }
}

