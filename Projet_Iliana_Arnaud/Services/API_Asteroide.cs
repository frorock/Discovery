using Newtonsoft.Json;
using Projet_Iliana_Arnaud.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class AsteroidData
{
    // Propriétés publiques de la classe AsteroidData
    public bool Hazardous { get; set; }
    public string Name { get; set; }
    public string Last_Date { get; set; }
    public string First_Date { get; set; }
    public string Longitude_ecliptique { get; set; }
    public string Latitude_ecliptique { get; set; }
    public string Distance { get; set; }
    public string Description { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public int Inclinaison { get; set; }
    public int Velocity { get; set; }
    public int Diameter { get; set; }


}

namespace Projet_Iliana_Arnaud.Services
{
    class API_Asteroides
    {
        // Liste publique d'objets AsteroidData
        public List<AsteroidData> asteroidData = new List<AsteroidData>();

        public API_Asteroides()
        {
            // Exécuter la méthode GetAsteroideDetails de manière asynchrone
            var asyncTask = Task.Run(async () => await GetAsteroideDetails());
            // Attendre la fin de l'exécution de la tâche asynchrone
            asyncTask.Wait();
            // Récupérer le résultat de la tâche asynchrone
            var asyncResult = asyncTask.Result;
        }

        public async Task<Root_Asteroide> GetAsteroideDetails()
        {
            // Créer un nouvel objet HttpClient pour envoyer une demande à l'API
            HttpClient client = new HttpClient();

            // Construire l'URL complet en utilisant les propriétés _url, _date et _apiKey
            string url = "https://api.nasa.gov/neo/rest/v1/neo/browse?api_key=voG8rW7vT0I5JKJQ3SeGyvFfBsyETyJgegSqIwQe";

            // Envoyer une demande à l'API
            HttpResponseMessage reponse = await client.GetAsync(url);

            // Vérifier que la réponse est correcte
            if (reponse.IsSuccessStatusCode)
            {
                // Lire le contenu de la réponse en tant que chaîne de caractères
                var contenu = await reponse.Content.ReadAsStringAsync();

                // Désérialiser le contenu JSON en utilisant la classe racine
                Root_Asteroide root = JsonConvert.DeserializeObject<Root_Asteroide>(contenu);
                MissDistance missDistance = JsonConvert.DeserializeObject<MissDistance>(contenu);

                // Parcourir chaque objet proche de la Terre dans la réponse
                foreach (NearEarthObject asteroid in root.near_earth_objects)
                {
                    // Créer un nouvel objet AsteroidData
                    AsteroidData data = new AsteroidData();

                    // Récupérer le nom de l'astéroïde
                    data.Name = asteroid.name;

                    data.Description = asteroid.orbital_data.orbit_class.orbit_class_description.ToString();

                    string Incli = asteroid.orbital_data.inclination.ToString();
                    int indexI = Incli.IndexOf('.');
                    data.Inclinaison = int.Parse(Incli.Substring(0, indexI));

                    // Récupérer le diamètre maximal de l'astéroïde en kilomètres
                    string Diam = asteroid.estimated_diameter.kilometers.estimated_diameter_max.ToString();
                    int index = Diam.IndexOf(',');
                    data.Diameter = int.Parse(Diam.Substring(0, index));


                    // Récupérer la vitesse relative de l'astéroïde en kilomètres par heure
                    string Vel = asteroid.close_approach_data[0].relative_velocity.kilometers_per_hour;
                    int indexV = Vel.IndexOf('.');
                    data.Velocity = int.Parse(Vel.Substring(0, indexV));

                    // Récupérer la date de la dernière observation de l'astéroïde
                    data.Last_Date = asteroid.orbital_data.last_observation_date.ToString();

                    data.First_Date = asteroid.orbital_data.first_observation_date.ToString();


                    // Déterminer si l'astéroïde est potentiellement dangereux
                    data.Hazardous = asteroid.is_potentially_hazardous_asteroid;

                    // Récupérer la longitude du noeud ascendant de l'orbite de l'astéroïde
                    data.Longitude_ecliptique = asteroid.orbital_data.ascending_node_longitude;
                    int indexD = data.Longitude_ecliptique.IndexOf('.');
                    double Long = int.Parse(data.Longitude_ecliptique.Substring(0, indexD));

                    // Récupérer l'inclinaison de l'orbite de l'astéroïde
                    data.Latitude_ecliptique = asteroid.orbital_data.inclination;
                    int index2 = data.Latitude_ecliptique.IndexOf('.');
                    double Lat = int.Parse(data.Latitude_ecliptique.Substring(0, index2)) * System.Math.PI / 180;


                    // Distance moyenne de l'astéroïde à la Terre 
                    data.Distance = asteroid.close_approach_data[0].miss_distance.kilometers.ToString();
                    int index3 = data.Distance.IndexOf('.');
                    double r = int.Parse(data.Distance.Substring(0, index3));

                    data.x = r / 40000000 * System.Math.Cos(Long) * System.Math.Cos(Lat);
                    data.y = r / 40000000 * System.Math.Sin(Long) * System.Math.Cos(Lat);
                    data.z = r / 40000000 * System.Math.Sin(Lat);



                    // Ajouter l'objet AsteroidData à la liste asteroidData
                    asteroidData.Add(data);
                }


                // Renvoyer l'objet root
                return root;

            }
            else
            {
                return null;
            }
        }


    }
}


