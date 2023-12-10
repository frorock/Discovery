using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Projet_Iliana_Arnaud.Model;

namespace Projet_Iliana_Arnaud.Services
{
    class API_ImageDuJour
    {
        // Clé API privée pour accéder à l'API
        const string Api_Key = "voG8rW7vT0I5JKJQ3SeGyvFfBsyETyJgegSqIwQe";

        // Propriétés publiques pour stocker les données de l'API
        public string Date { get; set; }
        public string Explanation { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        // Constructeur qui appelle la méthode Getapidetails()
        public API_ImageDuJour()
        {
            var asyncTask = Task.Run(async () => await Getapidetails());
            asyncTask.Wait();
            var asyncResult = asyncTask.Result;
        }

        // Méthode async pour appeler l'API et stocker les données dans les propriétés publiques
        public async Task<string> Getapidetails()
        {
            // Instanciation d'un client HTTP
            HttpClient client = new HttpClient();

            // Récupération de la réponse de l'API en utilisant la clé API
            HttpResponseMessage response = await client.GetAsync("https://api.nasa.gov/planetary/apod?api_key=" + Api_Key);

            // Si la réponse est réussie
            if (response.IsSuccessStatusCode)
            {
                // Lire le contenu de la réponse en tant que chaîne de caractères
                var content = await response.Content.ReadAsStringAsync();

                // Désérialiser le contenu JSON en utilisant la classe racine
                Entity root = JsonConvert.DeserializeObject<Entity>(content);

                // Stocker les données de l'API dans les propriétés publiques
                Date = root.date.ToString();
                Explanation = root.explanation.ToString();
                Title = root.title.ToString();
                Url = root.url.ToString();

                // Retourner une chaîne vide
                return "";
            }
            else
            {
                // Si la réponse n'est pas réussie, retourner null
                return null;
            }
        }
    }
}

