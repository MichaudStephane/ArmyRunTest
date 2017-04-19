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
        
        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        Vector2 AnciennePosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton Start { get; set; }
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
            DifficultéFacile = new Boutton(Game, "Facile", new Rectangle(temp.X-MARGE_DROITE, temp.Y - MARGE_BAS * 2 , 120, 70), Color.Blue, nomImageAvant, nomImageAprès);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Rectangle(temp.X - MARGE_DROITE, temp.Y - MARGE_BAS, 120, 70), Color.Blue, nomImageAvant, nomImageAprès);
            DifficultéDifficile = new Boutton(Game, "Difficile", new Rectangle(temp.X - MARGE_DROITE, temp.Y, 120, 70), Color.Blue, nomImageAvant, nomImageAprès);
            Tutoriel = new Boutton(Game, "Tutorial", new Rectangle(MARGE_GAUCHE, temp.Y - MARGE_BAS * 2, 120,70), Color.Blue, nomImageAvant, nomImageAprès);
            Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute);

            Bouttons.Add(DifficultéFacile);
            Bouttons.Add(DifficultéMoyenne);
            Bouttons.Add(DifficultéDifficile);
            Bouttons.Add(Tutoriel);

            foreach (Boutton b in Bouttons)
            {
                Game.Components.Add(b);
            }
            Game.Components.Add(Mute);

            PosInitialeNiveau = new Vector3(0, 0, 0);

            AnciennePosSouris = PosSouris;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CaméraJeuAutomate = Game.Services.GetService(typeof(CaméraAutomate)) as CaméraAutomate;
            GestionnaireManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            Game.IsMouseVisible = true;
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Point point = GestionnaireManager.GetPositionSouris();
            PosSouris = new Vector2(point.X, point.Y);
            
            if (GestionnaireManager.EstSourisActive)
            {
                for (int i = 0; i < Bouttons.Count; i++)
                {
                    Boutton b = Bouttons[i];
                    if (EstDansBoutton(b))
                    {
                        b.ChangerDeCouleurTexture();
                        b.ChangerCouleurTexte = Color.Red;
                        if (GestionnaireManager.EstNouveauClicGauche())
                        {
                            CréerJeu(i);
                        }
                    }
                    else
                    {
                        b.ChangerCouleurTexte = Color.Blue;
                    }
                }

                if (EstDansBoutton(Mute))
                {
                    if(GestionnaireManager.EstNouveauClicGauche())
                    {
                        Mute.ChangerDeCouleurTexture();

                        if (PartieEnCours != null)
                        {
                            PartieEnCours.FaireJouerMusique();
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        void CréerJeu(int index)
        {
            if(Bouttons[index] == DifficultéFacile)
            {
                PartieEnCours = new Jeu(Game, 15, PosInitialeNiveau, 30, INTERVALLE_MOYEN);
                Game.Components.Add(PartieEnCours);
            }
            
            if(Bouttons[index] == DifficultéMoyenne)
            {
                PartieEnCours = new Jeu(Game, 25, PosInitialeNiveau, 25, INTERVALLE_MOYEN);
                Game.Components.Add(PartieEnCours);
            }

            if (Bouttons[index] == DifficultéDifficile)
            {
                PartieEnCours = new Jeu(Game, 35, PosInitialeNiveau, 20, INTERVALLE_MOYEN);
                Game.Components.Add(PartieEnCours);
            }

            if (Bouttons[index] == Tutoriel)
            {
                //Jeu partie = new Jeu(Game); // créer tutoriel
            }
            
            foreach (Boutton b in Bouttons)
            {
                Game.Components.Remove(b);
                b.Enabled = false;
            }

            for(int i = Bouttons.Count-1; i>=0;i--)
            {
                Bouttons.RemoveAt(i);
            }
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
            return estDansBoutton;
        }
    }
}
