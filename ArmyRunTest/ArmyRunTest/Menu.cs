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
            Cursor.Show();
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Point point = GestionnaireManager.GetPositionSouris();
            PosSouris = new Vector2(point.X, point.Y);
            if (tempsÉcoulé >= INTERVALLE_MOYEN)
            {
                if (EstDansBoutton())
                {
                    if (GestionnaireManager.EstNouveauClicGauche())
                    {
                        List<PrimitiveDeBase>[] temp = new List<PrimitiveDeBase>[1];

                        List<SectionDeNiveau> a = null;
                        Game.Components.Add(new Armée(Game, 50, new Vector3(0, 2, -60), Atelier.INTERVALLE_STANDARD, temp,a));
                        Game.Components.Add(CaméraJeuAutomate);
                        Game.Components.Add(new Afficheur3D(Game));
                        Game.Components.Add(new TuileTexturée(Game, 100F, Vector3.Zero, Vector3.Zero, new Vector2(1, 1), "FeuFollet", 1f / 60));
                    }
                }

            }
            base.Update(gameTime);
        }

        bool EstDansBoutton()
        {
            bool estDansBoutton = false;
            if (PosSouris.X >= Start.GetDimensionBoutton().Left && PosSouris.X <= Start.GetDimensionBoutton().Right && PosSouris.Y >= Start.GetDimensionBoutton().Top && PosSouris.Y <= Start.GetDimensionBoutton().Bottom) 
            {
                estDansBoutton = true;
            }
            return estDansBoutton;
        }
    }
}
