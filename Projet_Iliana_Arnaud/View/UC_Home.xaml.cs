using Projet_Iliana_Arnaud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projet_Iliana_Arnaud.View
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        API_ImageDuJour api_imageDuJour;
        public Home()
        {
            InitializeComponent();

            api_imageDuJour = new API_ImageDuJour();
            // Récupération de l'URL de l'image du jour depuis l'API ImageDuJour
            Uri urlImage = new Uri(api_imageDuJour.Url);
            Image_Jour.Source = new BitmapImage(urlImage);           

            // Récupération de la date, de l'explication et du titre de l'image du jour depuis l'API ImageDuJour
            string Date = api_imageDuJour.Date;
            TB_Date.Text = Date;
            string Explanation = api_imageDuJour.Explanation;
            TB_description.Text = Explanation;
            string Title = api_imageDuJour.Title;
            TB_Titre_Jour.Text = Title;
        }
    }
}
