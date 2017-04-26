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
        const int MARGE_DROITE = 10;
        const int MARGE_BAS = 90;
        const int MARGE_GAUCHE = 150;
        const int MAXIMUM_NOMBRE_SOLDAT = 30;
        const int MINIMUM_NOMBRE_SECTION = 15;

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
        AfficheurNbVivant afficheur { get; set; }

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
            RectangleAffichageMute = new Rectangle(0, temp.Y - MARGE_BAS, 60, 60);

            Bouttons = new List<Boutton>();
            DifficultéFacile = new Boutton(Game, "Facile", new Rectangle(temp.X-MARGE_DROITE, temp.Y - MARGE_BAS * 2 , 120, 70), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT,2, INTERVALLE_MOYEN);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Rectangle(temp.X - MARGE_DROITE, temp.Y - MARGE_BAS, 120, 70), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT-5,MINIMUM_NOMBRE_SECTION+10, INTERVALLE_MOYEN);
            DifficultéDifficile = new Boutton(Game, "Difficile", new Rectangle(temp.X - MARGE_DROITE, temp.Y, 120, 70), Color.Blue, nomImageAvant, nomImageAprès, 
                                            MAXIMUM_NOMBRE_SOLDAT-10,MINIMUM_NOMBRE_SECTION+20, INTERVALLE_MOYEN);
            Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute, 0,0, INTERVALLE_MOYEN);

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
                Game.Components.Remove(Game.Components.Where(x => x is AfficheurNbVivant).ToList().Last());
            }
            afficheur = new AfficheurNbVivant(Game, "185281", Color.Red, CompteurNiveau, new Vector2(0, 0), INTERVALLE_MOYEN);
            Game.Components.Add(afficheur);

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

            afficheur.ChangerNombreVivant(CompteurNiveau);

            if (PartieEnCours.EstRéussi)
            {
                Boutton Continuer = new Boutton(Game, "Continuer", new Rectangle(Game.Window.ClientBounds.X / 2 - 40, Game.Window.ClientBounds.Y / 2 - 10, 120, 70), Color.Blue, "fond écran blanc", "FondEcranGris", PartieEnCours.GetNbSoldat(), CalculerNbSection(), INTERVALLE_MOYEN);
                Bouttons.Add(Continuer);
                Game.Components.Add(Continuer);
                ++CompteurNiveau;
                PartieEnCours.EstRéussi = false;
            }

            if (PartieEnCours.EstÉchec)
            {

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
                            b.CréerJeu();
                            PartieEnCours = Game.Services.GetService(typeof(Jeu)) as Jeu;

                            foreach (Boutton boutton in Bouttons)
                            {
                                Game.Components.Remove(boutton);
                                boutton.Enabled = false;
                            }
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

        int CalculerNbSection()
        {
            return 3;
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
