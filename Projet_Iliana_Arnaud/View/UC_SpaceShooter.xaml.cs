using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Configuration;



namespace Projet_Iliana_Arnaud.View
{
  
    public partial class Mars : UserControl
    {
        Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer lifeTimer = new DispatcherTimer();
        bool moveLeft;
        bool moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random random = new Random();
        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        int enemySpeed = 5;
        int score = 0; 
        int vie = 100;
        int limit = 50;
        Rect playerHitBox;

        public Mars()
        {

            InitializeComponent();

            SpielCanvas.Focus();
            // Définir l'intervalle de gameTimer à 20 ms et lier l'événement GameLoop à celui-ci
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;

            // Mettre le focus sur SpielCanvas
            SpielCanvas.Focus();

            gameTimer.Start();

            // Définir l'intervalle de lifeTimer à 30 secondes et lier l'événement Makelife à celui-ci
            lifeTimer.Interval = TimeSpan.FromSeconds(30);
            lifeTimer.Tick += Makelife;

            lifeTimer.Start();

            // Créer une nouvelle ImageBrush et définir la source en playerImage
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\SpaceSheep.png", UriKind.Relative));

            // Définir le remplissage de l'objet Player avec l'ImageBrush playerImage
            Player.Fill = playerImage;
        }

        private void GameLoop(object sender, EventArgs e)
        {

            // Initialise la hitbox du personnage joueur
            playerHitBox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            enemyCounter -= 1;
            LB_Score.Content = "Score: " + score;
            LB_Life.Content = "Vie: " + vie;

            // Si le compteur d'ennemi est inférieur à 0, en crée de nouveaux et remet le compteur à sa limite
            if (enemyCounter < 0)
            {
                MakeEnemies();
                enemyCounter = limit;
            }
            // Si le joueur veut aller à gauche et qu'il n'est pas au bord gauche de l'écran, déplace le joueur à gauche
            if (moveLeft == true && Canvas.GetLeft(Player) > 0)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - playerSpeed);
            }
            // Si le joueur veut aller à droite et qu'il n'est pas au bord droit de l'écran, déplace le joueur à droite
            if (moveRight == true && Canvas.GetLeft(Player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + playerSpeed);
            }
            // Pour chaque objet de type rectangle sur le canvas
            foreach (var x in SpielCanvas.Children.OfType<Rectangle>())
            {
                // Si c'est une balle
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    // Déplace la balle vers le haut
                    Canvas.SetTop(x, Canvas.GetTop(x) - 15);
                    // Initialise la hitbox de la balle
                    Rect bulletHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    // Si la balle sort de l'écran, la supprime
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }
                    // Pour chaque objet de type rectangle sur le canvas
                    foreach (var y in SpielCanvas.Children.OfType<Rectangle>())
                    {
                        // Si c'est un ennemi
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            // Initialise la hitbox de l'ennemi
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            // Si la balle touche l'ennemi, les supprime et augmente le score
                            if (bulletHitbox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score = score + 10;
                            }
                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    // Déplace l'ennemi vers le bas
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);
                    // Si l'ennemi sort de l'écran en bas, le supprime et enlève 10 points de vie au joueur
                    if (Canvas.GetTop(x) > 620)
                    {
                        itemRemover.Add(x);
                        vie -= 10;
                    }
                    // Initialise la hitbox de l'ennemi
                    Rect enemyHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    // Si l'ennemi touche le joueur, le supprime et enlève 5 points de vie au joueur
                    if (playerHitBox.IntersectsWith(enemyHitbox))
                    {
                        itemRemover.Add(x);
                        vie -= 5;
                    }
                }
                if (x is Rectangle && (string)x.Tag == "life")
                {
                    // Déplace l'objet de vie vers le bas
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);
                    // Si l'objet de vie sort de l'écran en bas, le supprime
                    if (Canvas.GetTop(x) > 700)
                    {
                        itemRemover.Add(x);
                    }
                    // Initialise la hitbox de l'objet de vie
                    Rect lifeHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    // Si le joueur touche l'objet de vie, le supprime et augmente les points de vie du joueur
                    if (playerHitBox.IntersectsWith(lifeHitbox))
                    {
                        itemRemover.Add(x);
                        vie += 10;
                    }
                }

            }
            foreach (Rectangle i in itemRemover)
            {

                // Supprime les objets de la liste itemRemover du canvas
                SpielCanvas.Children.Remove(i);
            }

            // Selon le score actuel, met à jour les paramètres de jeu (vitesse des ennemis, vitesse du joueur, limite d'ennemis)
            switch (score)
            {
                case 100:
                    LB_Lvl.Content = "Level 2";
                    enemySpeed = 7;
                    playerSpeed = 15;
                    limit = 30;
                    break;

                case 300:
                    LB_Lvl.Content = "Level 3";
                    limit = 20;
                    playerSpeed = 15;
                    break;

                case 600:
                    LB_Lvl.Content = "Level 4";
                    enemySpeed = 10;
                    limit = 15;
                    playerSpeed = 20;
                    break;

                case 800:
                    LB_Lvl.Content = "Level 5";
                    limit = 20;
                    enemySpeed = 12;
                    playerSpeed = 20;
                    break;

                case 1000:
                    LB_Lvl.Content = "Level 6";
                    limit = 18;
                    enemySpeed = 13;
                    playerSpeed = 25;
                    break;

                case 1300:
                    LB_Lvl.Content = "Level 7";
                    limit = 15;
                    enemySpeed = 14;
                    playerSpeed = 25;
                    break;

                case 1600:
                    LB_Lvl.Content = "Level 8";
                    limit = 12;
                    enemySpeed = 15;
                    playerSpeed = 30;
                    break;

                case 1800:
                    LB_Lvl.Content = "Level 8";
                    limit = 10;
                    enemySpeed = 16;
                    playerSpeed = 30;
                    break;
            }
            if (vie <= 0)
            {
                gameTimer.Stop();
                lifeTimer.Stop();

                // Change la couleur du texte indiquant les points de vie
                LB_Life.Foreground = Brushes.Red;

                // MessageBox pour demander au joueur s'il souhaite recommencer la partie
                string messageBoxText = "Recommencer?";
                string caption = "GAME OVER";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.None;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                // Selon la réponse du joueur, soit recommence la partie, soit quitte l'application
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        GameReset();
                        break;
                    case MessageBoxResult.No:
                        var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
                        Process.Start(currentExecutablePath);
                        System.Windows.Application.Current.Shutdown(); break;
                        break;
                }

            }

        }
        private void GameReset()
        {
            // Remet la couleur du texte indiquant les points de vie à blanc et remet le Level à 1
            LB_Life.Foreground = Brushes.White;
            LB_Lvl.Content = "Level 1";

            // Démarre les timers de jeu
            gameTimer.Start();
            lifeTimer.Start();

            // Réinitialise les compteurs et paramètres de jeu
            enemySpriteCounter = 0;
            enemyCounter = 100;
            playerSpeed = 10;
            enemySpeed = 5;
            score = 0;
            vie = 100;
            limit = 50;

            // Supprime tous les ennemis et objets de vie du canvas
            foreach (var y in SpielCanvas.Children.OfType<Rectangle>())
            {
                if (y is Rectangle && (string)y.Tag == "enemy" || (string)y.Tag == "life")
                {
                    itemRemover.Add(y);
                }
            }

        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Efface le texte des règles
            TB_Rule.Text = "";

            // Si la touche flèche gauche est enfoncée, active le mouvement vers la gauche
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            else if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            else if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            // Si la touche espace est relâchée, crée un nouveau projectile (bullet)
            if (e.Key == Key.Space)
            {
                // Création d'un nouveau rectangle pour le projectile
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 7, 
                    Fill = Brushes.White,
                    Stroke = Brushes.Red

                };
                    // Positionne le projectile au milieu de l'emplacement de joueur en haut de l'écran
                    Canvas.SetLeft(newBullet, Canvas.GetLeft(Player) + Player.ActualWidth / 2);
                    Canvas.SetTop(newBullet, Canvas.GetTop(Player));
                    SpielCanvas.Children.Add(newBullet);
              
            }
        }
        private void MakeEnemies()
        {
            // Crée un nouveau brush pour les sprites des ennemis
            ImageBrush enemySprite = new ImageBrush();

            // Choisit un sprite aléatoire pour l'ennemi
            enemySpriteCounter = random.Next(1, 5);
            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\Obstacle.png", UriKind.Relative));
                    break;

                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\Enemy1.png", UriKind.Relative));
                    break;

                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\Enemy2.png", UriKind.Relative));
                    break;

                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\Enemy3.png", UriKind.Relative));
                    break;
            }

            // Crée un nouveau rectangle pour l'ennemi
            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 100,
                Width = 100,
                Fill = enemySprite,
            };
            Canvas.SetTop(newEnemy, 0);
            Canvas.SetLeft(newEnemy, random.Next(30, 1000));
            SpielCanvas.Children.Add(newEnemy);
        }
        private void Makelife(object sender, EventArgs e)
        {
            // Création d'un nouveau pinceau d'image
            ImageBrush LifeSprite = new ImageBrush();

            // Définition de la source de l'image pour le pinceau
            LifeSprite.ImageSource = new BitmapImage(new Uri("..\\..\\..\\Ressources\\Image\\Game\\Life.png", UriKind.Relative));

            // Création d'un nouveau rectangle pour afficher l'image de vie
            Rectangle newLife = new Rectangle
            {
                // Ajout d'un tag pour identifier l'élément
                Tag = "life",
                // Taille de l'image de vie
                Height = 100,
                Width = 100,
                // Affectation du pinceau à la propriété de remplissage du rectangle
                Fill = LifeSprite,
            };

            // Positionnement aléatoire de la vie sur le canvas en haut et à gauche
            Canvas.SetTop(newLife, 0);
            Canvas.SetLeft(newLife, random.Next(30, 1000));

            // Ajout de l'image de vie au canvas
            SpielCanvas.Children.Add(newLife);
        }

    }
}

