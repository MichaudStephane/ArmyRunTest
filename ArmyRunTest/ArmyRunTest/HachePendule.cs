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
    public class HachePendule : ObjetBase,ICollisionable
    {
        const int GRANDEUR_HACHE_STANDARD = 12;
        const int NB_HITBOX_PRÉCISES = 8;
        Matrix MondeInitial { get; set; }
        BoundingSphere[] TableauxHitBoxPrécises { get; set; }
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
            TableauxHitBoxPrécises = new BoundingSphere[NB_HITBOX_PRÉCISES];
            CréerHitBoxGénérale();
            CréerHitBoxesPrécises();
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
                BougerHitBox();
                Angle = MathHelper.PiOver2 * (float)Math.Sin((float)gameTime.TotalGameTime.TotalSeconds);

                TempsÉcouléDepuisMaj = 0;
            }

            base.Update(gameTime);
        }

        private void BougerHitBox()
        {
            foreach(BoundingSphere a in TableauxHitBoxPrécises)
            {
             //   Vector3 centre = Vector3.Transform(a.Center, Matrix.CreateTranslation(0, -GRANDEUR_HACHE_STANDARD, 0f) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(0, GRANDEUR_HACHE_STANDARD, 0f));
             //   BoundingSphere b = new BoundingSphere(centre, a.Radius);
                 a.Transform(Matrix.CreateTranslation(-a.Center.X, -GRANDEUR_HACHE_STANDARD, -a.Center.Z) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(a.Center.X, GRANDEUR_HACHE_STANDARD, a.Center.Z));
            }
            
            
        }

        private void CalculerNouvellePositionHache()
        {

            
            Matrix transform  = Matrix.CreateTranslation(-Position.X, -GRANDEUR_HACHE_STANDARD, -Position.Z) *
                      Matrix.CreateRotationZ(Angle) *
                      Matrix.CreateTranslation(Position.X, GRANDEUR_HACHE_STANDARD, Position.Z);

            Monde = MondeInitial * transform;
        }

        void CréerHitBoxGénérale()
        {
            HitBoxGénérale = new BoundingSphere(new Vector3(Position.X, Position.Y+3, Position.Z), 5);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {

            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
            {
                for (int i = 0; i < NB_HITBOX_PRÉCISES; i++)
                {
                    if((a as Soldat).HitBoxGénérale.Intersects(TableauxHitBoxPrécises[i]))
                    {
                        Vector3 EntreSphèreetPers = (TableauxHitBoxPrécises[i].Center - a.Position);
                       // (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, (a as Soldat).Vitesse.Y, 0);
                        v = Angle*1000 * EntreSphèreetPers;
                        

                    }
                }

               
            }

            return v;

        }

        void CréerHitBoxesPrécises()
        {
            TableauxHitBoxPrécises[0] = new BoundingSphere(new Vector3(Position.X, Position.Y, Position.Z),1f);
            TableauxHitBoxPrécises[1] = new BoundingSphere(new Vector3(Position.X+0.5F, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[2] = new BoundingSphere(new Vector3(Position.X-0.5F, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[3] = new BoundingSphere(new Vector3(Position.X + 1, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[4] = new BoundingSphere(new Vector3(Position.X - 1, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[5] = new BoundingSphere(new Vector3(Position.X + 1.5f, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[6] = new BoundingSphere(new Vector3(Position.X - 1.5f, Position.Y, Position.Z), 1f);
            TableauxHitBoxPrécises[7] = new BoundingSphere(new Vector3(Position.X , Position.Y +1.5f, Position.Z), 1f);


        }
    }
}
