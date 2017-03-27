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
        Vector3[] TableauPositionInitiales { get; set; }
        Vector3 PositionInitialeHbGénérale { get; set; }
        float cpt { get; set; }
        float Angle { get; set; }


        float INTERVALLE_MAJ { get; set; }
        float TempsÉcouléDepuisMaj { get; set; } 
        BoundingSphere HitBoxGénérale { get; set; }
        int sens { get; set; }
        float AnglePrécédent { get; set; }
        float AngleDépart { get; set; }
        public HachePendule(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel,float angleDépart)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            INTERVALLE_MAJ = 1 / 60f;
            AngleDépart = angleDépart;
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
            sens = 1;
            AnglePrécédent = 0;
            Angle = 0;
            MondeInitial = Monde;
            PlacerHachce();
        }

        private void PlacerHachce()
        {
            //MondeInitial = Monde* Matrix.CreateTranslation(-Position.X, -GRANDEUR_HACHE_STANDARD, -Position.Z) *
            //          Matrix.CreateRotationZ(0) *
            //          Matrix.CreateTranslation(Position.X, GRANDEUR_HACHE_STANDARD, Position.Z);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            AnglePrécédent = Angle;
            TempsÉcouléDepuisMaj += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
             if(TempsÉcouléDepuisMaj >= INTERVALLE_MAJ)
            {

                CalculerNouvellePositionHache();
                BougerHitBox();
                Angle = MathHelper.PiOver2 * (float)Math.Sin(AngleDépart+(float)gameTime.TotalGameTime.TotalSeconds) ;

                if( Angle>AnglePrécédent )
                {
                    sens = 1;
                }
                else
                {
                    sens = -1;
                }
                

                TempsÉcouléDepuisMaj = 0;
            }

            base.Update(gameTime);
        }

        private void BougerHitBox()
        {
            Vector3 NouveauCentre = Vector3.Transform(PositionInitialeHbGénérale, Matrix.CreateTranslation(-PositionInitialeHbGénérale.X, -GRANDEUR_HACHE_STANDARD, -PositionInitialeHbGénérale.Z) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(PositionInitialeHbGénérale.X, GRANDEUR_HACHE_STANDARD, PositionInitialeHbGénérale.Z));
            HitBoxGénérale = new BoundingSphere(NouveauCentre, HitBoxGénérale.Radius);

            for (int i=0;i<TableauxHitBoxPrécises.Count();++i)
            {
                
                Vector3 centre = Vector3.Transform(TableauPositionInitiales[i], Matrix.CreateTranslation(-TableauPositionInitiales[i].X, -GRANDEUR_HACHE_STANDARD, -TableauPositionInitiales[i].Z) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(TableauPositionInitiales[i].X, GRANDEUR_HACHE_STANDARD, TableauPositionInitiales[i].Z));
                TableauxHitBoxPrécises[i] = new BoundingSphere(centre, TableauxHitBoxPrécises[i].Radius);
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
            PositionInitialeHbGénérale = new Vector3(HitBoxGénérale.Center.X, HitBoxGénérale.Center.Y, HitBoxGénérale.Center.Z);
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
                       Vector3 temp = sens * 2800 * new Vector3(1,0.5f,0); //(Angle / Math.Abs(Angle))
                        v = new Vector3(temp.X,Math.Abs(temp.Y),0);
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

            TableauPositionInitiales = new Vector3[TableauxHitBoxPrécises.Length];

            for(int i = 0;i<TableauxHitBoxPrécises.Length;++i)
            {
                TableauPositionInitiales[i] = new Vector3(TableauxHitBoxPrécises[i].Center.X, TableauxHitBoxPrécises[i].Center.Y, TableauxHitBoxPrécises[i].Center.Z);
            }


        }
    }
}
