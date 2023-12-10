
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Projet_Iliana_Arnaud.View
{

    public partial class InfoPositions : UserControl
    {
        public InfoPositions()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitlePropety); }
            set { SetValue(TitlePropety, value);}
        }

        public static readonly DependencyProperty TitlePropety = DependencyProperty.Register("Title", typeof(string), typeof(InfoPositions));


        public string Number
        {
            get { return (string)GetValue(NumberPropety); }
            set { SetValue(NumberPropety, value); }
        }

        public static readonly DependencyProperty NumberPropety = DependencyProperty.Register("Number", typeof(string), typeof(InfoPositions));


        public  FontAwesome.Sharp.IconChar Icon
        {
            get { return (FontAwesome.Sharp.IconChar)GetValue(IconPropety); }
            set { SetValue(IconPropety, value); }
        }

        public static readonly DependencyProperty IconPropety = DependencyProperty.Register("Icon", typeof(FontAwesome.Sharp.IconChar), typeof(InfoPositions));


        public Color Background1
        {
            get { return (Color)GetValue(Background1Propety); }
            set { SetValue(Background1Propety, value); }
        }

        public static readonly DependencyProperty Background1Propety = DependencyProperty.Register("Background1", typeof(Color), typeof(InfoPositions));


        public Color Background2
        {
            get { return (Color)GetValue(Background2Propety); }
            set { SetValue(Background2Propety, value); }
        }


        public static readonly DependencyProperty Background2Propety = DependencyProperty.Register("Background2", typeof(Color), typeof(InfoPositions));
        public Color EllipseBackground1
        {
            get { return (Color)GetValue(EllipseBackground1Propety); }
            set { SetValue(EllipseBackground1Propety, value); }
        }


        public static readonly DependencyProperty EllipseBackground1Propety = DependencyProperty.Register("EllipseBackground1", typeof(Color), typeof(InfoPositions));
        public Color EllipseBackground2
        {
            get { return (Color)GetValue(EllipseBackground2Propety); }
            set { SetValue(EllipseBackground2Propety, value); }
        }

        public static readonly DependencyProperty EllipseBackground2Propety = DependencyProperty.Register("EllipseBackground2", typeof(Color), typeof(InfoPositions));

    }
}
