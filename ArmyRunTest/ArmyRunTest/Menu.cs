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
        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton Start{ get; set; }


        public Menu(Game jeu)
            : base(jeu)
        {  }

        public override void Initialize()
        {
            Start = new Boutton(Game, "Start", new Vector2(Game.Window.ClientBounds.X/2,Game.Window.ClientBounds.Y/2), Color.Blue);
            Game.Components.Add(Start);
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

            Game.Window.Title = PosSouris.X + " ; " + PosSouris.Y;

            if (GestionnaireManager.EstSourisActive)
            {
                if (EstDansBoutton())
                {
                    if (GestionnaireManager.EstNouveauClicGauche())
                    {
                        Jeu partie = new AtelierXNA.Jeu(Game, 5, new Vector3(0, 0, 0), 20, INTERVALLE_MOYEN);
                        Game.Components.Add(partie);
                        Game.Components.Remove(Start);
                        this.Enabled = false;
                    }
                }
            }
            base.Update(gameTime);
        }

        bool EstDansBoutton()
        {
            bool estDansBoutton = false;
            Rectangle temp = Start.GetDimensionBoutton();

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
