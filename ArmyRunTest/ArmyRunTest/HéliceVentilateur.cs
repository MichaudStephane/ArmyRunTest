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

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HéliceVentilateur : ObjetBase,IDeletable
    {
        float Angle { get; set; }
        Matrix MondeInitial { get; set; }
        const float INTERVALLE_MAJ = 1 / 60f;
        float TempsÉcouléDepuisMaj { get; set; }
        public HéliceVentilateur(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,intervalleMAJ,nomModel)
        {
            PlacerHélice();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Tangage = true;
            MondeInitial = GetMonde();
            
        }
        private void PlacerHélice()
        {
            Monde *= Matrix.CreateRotationY(MathHelper.PiOver2);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //TempsÉcouléDepuisMaj += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (TempsÉcouléDepuisMaj >= INTERVALLE_MAJ)
            //{ 
            //    BougerHélice();
            //    Angle += MathHelper.TwoPi/60f;
            //    TempsÉcouléDepuisMaj = 0;
            //}


            base.Update(gameTime);
        }

        private void BougerHélice()
        {
            //Monde = MondeInitial * Matrix.CreateRotationX(Angle);
        }
    }
}
