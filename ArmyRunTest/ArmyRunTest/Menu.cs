﻿using System;
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
            DifficultéFacile = new Boutton(Game, "Facile", new Rectangle(temp.X-MARGE_DROITE, temp.Y - MARGE_BAS * 2 , 120, 70), Color.Blue, nomImageAvant, nomImageAprès, MAXIMUM_NOMBRE_SOLDAT,2, INTERVALLE_MOYEN);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Rectangle(temp.X - MARGE_DROITE, temp.Y - MARGE_BAS, 120, 70), Color.Blue, nomImageAvant, nomImageAprès, MAXIMUM_NOMBRE_SOLDAT-5,MINIMUM_NOMBRE_SECTION+10, INTERVALLE_MOYEN);
            DifficultéDifficile = new Boutton(Game, "Difficile", new Rectangle(temp.X - MARGE_DROITE, temp.Y, 120, 70), Color.Blue, nomImageAvant, nomImageAprès, MAXIMUM_NOMBRE_SOLDAT-10,MINIMUM_NOMBRE_SECTION+20, INTERVALLE_MOYEN);
            Tutoriel = new Boutton(Game, "Tutorial", new Rectangle(MARGE_GAUCHE, temp.Y - MARGE_BAS * 2, 120,70), Color.Blue, nomImageAvant, nomImageAprès, MAXIMUM_NOMBRE_SOLDAT, MINIMUM_NOMBRE_SECTION, INTERVALLE_MOYEN);
            Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute, 0,0, INTERVALLE_MOYEN);

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
            PartieEnCours = new Jeu(Game, 0, new Vector3(0, 0,0), 0, INTERVALLE_MOYEN);
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
                        b.ChangerDeCouleur(true);
                        if (GestionnaireManager.EstNouveauClicGauche())
                        {
                            b.CréerJeu();

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
