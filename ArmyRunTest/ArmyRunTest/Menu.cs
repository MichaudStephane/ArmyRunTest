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
   public class Menu :  Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_DROITE = 160;
        const int MARGE_BAS = 90;
        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton Start{ get; set; }
        Boutton DifficultéFacile { get; set; }
        Boutton DifficultéMoyenne { get; set; }
        Boutton DifficultéDifficile { get; set; }

        Vector3 PosInitialeNiveau { get; set; }
        List<Boutton> Bouttons { get; set; }


        public Menu(Game jeu)
            : base(jeu)
        {  }

        public override void Initialize()
        {
            string nomImageAvant = "fond écran blanc";
            string nomImageAprès = "FondEcranGris";
            Rectangle temp = Game.Window.ClientBounds;
            Bouttons = new List<Boutton>();
            Start = new Boutton(Game, "Start", new Vector2(temp.X/2, temp.Y/2), Color.Blue, nomImageAvant, nomImageAprès);
            DifficultéFacile = new Boutton(Game, "Facile", new Vector2(temp.X + MARGE_DROITE, temp.Y -MARGE_BAS*2), Color.Blue, nomImageAvant, nomImageAprès);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Vector2(temp.X + MARGE_DROITE, temp.Y - MARGE_BAS), Color.Blue, nomImageAvant, nomImageAprès);
            DifficultéDifficile = new Boutton(Game, "difficile", new Vector2(temp.X + MARGE_DROITE, temp.Y), Color.Blue, nomImageAvant, nomImageAprès);
            Bouttons.Add(Start);
            Bouttons.Add(DifficultéFacile);
            Bouttons.Add(DifficultéMoyenne);
            Bouttons.Add(DifficultéDifficile);
            Game.Components.Add(Start);

            PosInitialeNiveau = new Vector3(0, 0, 0);
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

            //Game.Window.Title = PosSouris.X + " ; " + PosSouris.Y;

            if (GestionnaireManager.EstSourisActive)
            {
                for (int i = 0; i < Bouttons.Count; i++)
                {
                    Boutton b = Bouttons[i];
                    if (EstDansBoutton(b))
                    {
                        b.ChangerDeCouleurTexture = true;
                        b.ChangerCouleurTexte = Color.Red;
                        if (GestionnaireManager.EstNouveauClicGauche())
                        {
                            CréerJeu(i);
                        }
                    }
                    else
                    {
                        b.ChangerDeCouleurTexture = false;
                        b.ChangerCouleurTexte = Color.Blue;
                    }
                }
            }
            base.Update(gameTime);
        }

        private void CréerJeu(int index)
        {
            Jeu partie = new AtelierXNA.Jeu(Game, 10, PosInitialeNiveau, 20, INTERVALLE_MOYEN);
            Game.Components.Add(partie);
            Game.Components.Remove(Bouttons[index]);
            Bouttons[index].Enabled = false;
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
