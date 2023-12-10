
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Projet_Iliana_Arnaud.Model;
using Projet_Iliana_Arnaud.View;
using static System.Resources.ResXFileRef;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using System.IO;
using System.Configuration;


namespace Projet_Iliana_Arnaud
{


    public partial class MainWindow : Window
    {
   
        public MainWindow()
        {
            InitializeComponent();

            // Ajoutez un gestionnaire d'événements pour l'événement MediaEnded
            StarVideo.MediaEnded += MediaElement_MediaEnded;

            // Chargez et lisez la vidéo d'étoiles
            StarVideo.Source = new Uri("..\\..\\..\\Ressources\\Video\\Discovery_Long.mp4", UriKind.Relative);
            StarVideo.Play();

            Border_Gauche.Visibility = Visibility.Hidden;

        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (StarVideo.Source ==  new Uri("..\\..\\..\\Ressources\\Video\\Discovery_Long.mp4", UriKind.Relative))
            {
                Border_Gauche.Visibility = Visibility.Visible;
                StarVideo.Source = new Uri("..\\..\\..\\Ressources\\Video\\Backgroun_Space.mp4", UriKind.Relative);
                Home HM = new Home();
                Conteneur.Children.Add(HM);
            }
            // Redémarrez la vidéo lorsque l'événement MediaEnded est déclenché
            StarVideo.Position = TimeSpan.FromSeconds(0);
            StarVideo.Play();
        }

        private void BTN_Home_Click(object sender, RoutedEventArgs e)
        {
            Conteneur.Children.Clear();
            Home HM = new Home();
            Conteneur.Children.Add(HM);
        }
        private void BTN_Asteroid_Click(object sender, RoutedEventArgs e)
        {
            Conteneur.Children.Clear();
            Astéroide AS = new Astéroide();
            Conteneur.Children.Add(AS);

        }
  
        private void BTN_SpaceS_Click(object sender, RoutedEventArgs e)

        {
            Conteneur.Children.Clear();
            Mars SPS = new Mars();
            Conteneur.Children.Add(SPS);
        }

      
    }


}

