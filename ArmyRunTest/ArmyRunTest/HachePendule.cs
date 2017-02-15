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
    public class HachePendule : ObjetBase
    {
        float angle;
        Matrix MondeInitial { get; set; }
        float cpt { get; set; }
        float Angle { get; set; }

        float INTERVALLE_MAJ { get; set; }
        float TempsÉcouléDepuisMaj { get; set; } 
        BoundingSphere HitBoxGénérale { get; set; }
        public HachePendule(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            INTERVALLE_MAJ = 1 / 60f;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
            CréerHitBoxGénérale();
            MondeInitial = Monde;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            TempsÉcouléDepuisMaj += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
             if(TempsÉcouléDepuisMaj >= INTERVALLE_MAJ)
            {
                CalculerNouvellePositionHache();
                Angle = MathHelper.PiOver2 * (float)Math.Sin((float)gameTime.TotalGameTime.TotalSeconds);

                TempsÉcouléDepuisMaj = 0;
            }

            base.Update(gameTime);
        }

        private void CalculerNouvellePositionHache()
        {

            
            Matrix transform  = Matrix.CreateTranslation(0, -12, 0f) *
                      Matrix.CreateRotationZ(Angle) *
                      Matrix.CreateTranslation(0, 12, 0f);

            Monde = MondeInitial * transform;
        }

        void CréerHitBoxGénérale()
        {
            HitBoxGénérale = new BoundingSphere(new Vector3(0, 4, 0), 4);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {

            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
            {
                v = HitBoxGénérale.Center - a.Position;
            }

            return v;

        }
    }
}
