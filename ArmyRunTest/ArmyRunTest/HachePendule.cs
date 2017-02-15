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
        float Temps�coul�DepuisMaj { get; set; } 
        BoundingSphere HitBoxG�n�rale { get; set; }
        public HachePendule(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel)
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
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
            Cr�erHitBoxG�n�rale();
            MondeInitial = Monde;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            Temps�coul�DepuisMaj += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
             if(Temps�coul�DepuisMaj >= INTERVALLE_MAJ)
            {
                CalculerNouvellePositionHache();
                Angle = MathHelper.PiOver2 * (float)Math.Sin((float)gameTime.TotalGameTime.TotalSeconds);

                Temps�coul�DepuisMaj = 0;
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

        void Cr�erHitBoxG�n�rale()
        {
            HitBoxG�n�rale = new BoundingSphere(new Vector3(0, 4, 0), 4);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {

            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
            {
                v = HitBoxG�n�rale.Center - a.Position;
            }

            return v;

        }
    }
}
