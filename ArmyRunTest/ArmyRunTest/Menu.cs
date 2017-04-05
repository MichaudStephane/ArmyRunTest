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

namespace AtelierXNA
{
   public class Menu :  Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        public Menu(Game jeu)
            : base(jeu)
        {  }

        public override void Initialize()
        {
            
            Game.Components.Add(new Boutton(Game, "Hello world", new Vector2(0, 0), Color.Blue));
            base.Initialize();
        }
        protected override void LoadContent()
        {
            GestionnaireManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
           
        }
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (tempsÉcoulé >= INTERVALLE_MOYEN)
            {
                if (GestionnaireManager.EstSourisActive)
                {
                    Point point = GestionnaireManager.GetPositionSouris();
                    PosSouris = new Vector2(point.X, point.Y);
                    if(EstDansBoutton())
                    { }
                }
            }
            base.Update(gameTime);
        }

        bool EstDansBoutton()
        {
            bool estDansBoutton = false;
            //if(PosSouris.X>)
            {

            }
            return estDansBoutton;
        }
    }
}
