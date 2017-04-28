using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace AtelierXNA
{
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_DROITE = 500;
        const int MARGE_BAS =140;
        const int MARGE_GAUCHE = 150;
        const int MAXIMUM_NOMBRE_SOLDAT = 30;
        const int MINIMUM_NOMBRE_SECTION = 10;
        const int LARGEUR_BOUTTON = 120;
        const int HAUTEUR_BOUTTON = 70;
        const int AUGMENTATION_SECTION_NIVEAU = 2;
        const int AUGMENTATION_SOLDATS_NIVEAU = 5;

        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        Vector2 AnciennePosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton DifficultéFacile { get; set; }
        Boutton DifficultéMoyenne { get; set; }
        Boutton DifficultéDifficile { get; set; }
        Boutton Tutoriel { get; set; }
        Boutton Mute { get; set; }
        Boutton Exit { get; set; }
        Vector3 PosInitialeNiveau { get; set; }
        List<Boutton> Bouttons { get; set; }
        Jeu PartieEnCours { get; set; }
        Rectangle RectangleAffichageMute { get; set; }
        Rectangle RectangleAffichageBoutonsDifficulté { get; set; }
        bool SourrisEstDansBoutton { get;set;}
        int CompteurNiveau { get; set; }
        SpriteBatch GestionSprites { get; set; }
        SpriteFont Font { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        AfficheurNb Afficheur { get; set; }

        public Menu(Game jeu)
            : base(jeu)
        {  }

        public override void Initialize()
        {
            string nomImageAvant = "fond écran blanc";
            string nomImageAprès = "FondEcranGris";
            string son = "icone son";
            string mute = "mute button";

            Rectangle temp = Game.Window.ClientBounds;
            RectangleAffichageMute = new Rectangle(0, temp.Height - MARGE_BAS, 60, 60);

            Bouttons = new List<Boutton>();
            DifficultéFacile = new Boutton(Game, "Facile", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS , LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT, 3, INTERVALLE_MOYEN);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS*3, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT-5,MINIMUM_NOMBRE_SECTION+10, INTERVALLE_MOYEN);
            DifficultéDifficile = new Boutton(Game, "Difficile", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS*5, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT-10,MINIMUM_NOMBRE_SECTION+20, INTERVALLE_MOYEN);
            Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute, 0,0, INTERVALLE_MOYEN);

            Exit = new Boutton(Game, "Quitter", new Rectangle(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 + 50, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, "fond écran blanc", "FondEcranGris", 0, 0, INTERVALLE_MOYEN);

            Bouttons.Add(DifficultéFacile);
            Bouttons.Add(DifficultéMoyenne);
            Bouttons.Add(DifficultéDifficile);

            foreach (Boutton b in Bouttons)
            {
                Game.Components.Add(b);
            }
            Game.Components.Add(Mute);

            PosInitialeNiveau = new Vector3(0, 0, 0);

            AnciennePosSouris = PosSouris;
            PartieEnCours = new Jeu(Game, 0, new Vector3(0, 0,0), 0, INTERVALLE_MOYEN);

            CompteurNiveau = 1;

            if (CompteurNiveau != 1)
            {
                Game.Components.Remove(Game.Components.Where(x => x is AfficheurNb).ToList().Last());
            }
            Afficheur = new AfficheurNb(Game, "185281", Color.Red, CompteurNiveau, new Vector2(0, 30), "Niveau :", INTERVALLE_MOYEN);
            Game.Components.Add(Afficheur);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            GestionSprites = new SpriteBatch(Game.GraphicsDevice);
            CaméraJeuAutomate = Game.Services.GetService(typeof(CaméraAutomate)) as CaméraAutomate;
            GestionnaireManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = GestionnaireDeFonts.Find("Arial");
        }

        public override void Update(GameTime gameTime)
        {
            Game.IsMouseVisible = true;
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Point point = GestionnaireManager.GetPositionSouris();
            PosSouris = new Vector2(point.X, point.Y);
            Afficheur.ChangerNombreAfficheur(CompteurNiveau);

            if (PartieEnCours.EstRéussi)
            {
                Boutton Continuer = new Boutton(Game, "Continuer", new Rectangle(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 50, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, "fond écran blanc", "FondEcranGris", CalculerNbSoldats(), CalculerNbSection(), INTERVALLE_MOYEN);
                
                Bouttons.Add(Continuer);
                Bouttons.Add(Exit);
                Game.Components.Add(Exit);
                Game.Components.Add(Continuer);
                ++CompteurNiveau;
                PartieEnCours.EstRéussi = false;
            }

            if (PartieEnCours.EstÉchec)
            {
                Boutton Recommencer = new Boutton(Game, "Recommencer", new Rectangle(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 50, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, "fond écran blanc", "FondEcranGris", PartieEnCours.GetNbSoldat(),PartieEnCours.GetNbSections(), INTERVALLE_MOYEN);
                Bouttons.Add(Recommencer);
                Bouttons.Add(Exit);

                Game.Components.Add(Recommencer);
                Game.Components.Add(Exit);
                PartieEnCours.EstÉchec = false;
            }
            //if (PartieEnCours.EstEnPause)
            //{
            //    foreach (GameComponent p in Game.Components)
            //    {
            //        p.Enabled = true;
            //    }

            //}

            if (GestionnaireManager.EstSourisActive)
            {
                for (int i = 0; i < Bouttons.Count; i++)
                {
                    Boutton b = Bouttons[i];
                    if (EstDansBoutton(b))
                    {
                        b.ChangerDeCouleur(true);
                        if (GestionnaireManager.EstNouveauClicGauche())
                        {
                            if (Game.Components.Where(x => x is Jeu).ToList().Count != 0)
                            {
                                Game.Components.Remove(Game.Components.Where(x => x is Jeu).ToList().First());
                                Game.Services.RemoveService(typeof(Jeu));
                            }
                            if (b != Exit)
                            {
                                b.CréerJeu();
                            }
                            else
                            {
                                Game.Exit();
                            }
                            PartieEnCours = Game.Services.GetService(typeof(Jeu)) as Jeu;

                            foreach (Boutton boutton in Bouttons)
                            {
                                Game.Components.Remove(boutton);
                                boutton.Enabled = false;
                            }
                            Bouttons.Clear();
                        }
                    }
                    else
                    {
                        b.ChangerDeCouleur(false);
                    }
                }

                if (EstDansBoutton(Mute))
                {
                    if (GestionnaireManager.EstNouveauClicGauche())
                    {
                        Mute.ChangerDeCouleur();
                        
                        PartieEnCours.FaireJouerMusique();
                    }
                }
            }
            base.Update(gameTime);
        }

        int CalculerNbSoldats()
        {
            return PartieEnCours.GetNbSoldat() + AUGMENTATION_SOLDATS_NIVEAU;
        }

        int CalculerNbSection()
        {
            return PartieEnCours.GetNbSections() + CompteurNiveau * AUGMENTATION_SECTION_NIVEAU;
        }

        bool EstDansBoutton(Boutton b)
        {
            bool estDansBoutton = false;
            Rectangle temp = b.GetDimensionBoutton();

            Point point = GestionnaireManager.GetPositionSouris();
            if (PosSouris.X >= temp.Left && PosSouris.X <= temp.Right)
            {
                if (PosSouris.Y >= temp.Top && PosSouris.Y <= temp.Bottom)
                {
                    estDansBoutton = true;
                }
            }
            SourrisEstDansBoutton = estDansBoutton;
            return estDansBoutton;
        }
    }
}
