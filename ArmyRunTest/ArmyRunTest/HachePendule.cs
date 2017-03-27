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
        const int NB_HITBOX_PR�CISES = 8;
        Matrix MondeInitial { get; set; }
        BoundingSphere[] TableauxHitBoxPr�cises { get; set; }
        Vector3[] TableauPositionInitiales { get; set; }
        Vector3 PositionInitialeHbG�n�rale { get; set; }
        float cpt { get; set; }
        float Angle { get; set; }


        float INTERVALLE_MAJ { get; set; }
        float Temps�coul�DepuisMaj { get; set; } 
        BoundingSphere HitBoxG�n�rale { get; set; }
        int sens { get; set; }
        float AnglePr�c�dent { get; set; }
        float AngleD�part { get; set; }
        public HachePendule(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel,float angleD�part)
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            INTERVALLE_MAJ = 1 / 60f;
            AngleD�part = angleD�part;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
            TableauxHitBoxPr�cises = new BoundingSphere[NB_HITBOX_PR�CISES];
            Cr�erHitBoxG�n�rale();
            Cr�erHitBoxesPr�cises();
            sens = 1;
            AnglePr�c�dent = 0;
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
            AnglePr�c�dent = Angle;
            Temps�coul�DepuisMaj += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
             if(Temps�coul�DepuisMaj >= INTERVALLE_MAJ)
            {

                CalculerNouvellePositionHache();
                BougerHitBox();
                Angle = MathHelper.PiOver2 * (float)Math.Sin(AngleD�part+(float)gameTime.TotalGameTime.TotalSeconds) ;

                if( Angle>AnglePr�c�dent )
                {
                    sens = 1;
                }
                else
                {
                    sens = -1;
                }
                

                Temps�coul�DepuisMaj = 0;
            }

            base.Update(gameTime);
        }

        private void BougerHitBox()
        {
            Vector3 NouveauCentre = Vector3.Transform(PositionInitialeHbG�n�rale, Matrix.CreateTranslation(-PositionInitialeHbG�n�rale.X, -GRANDEUR_HACHE_STANDARD, -PositionInitialeHbG�n�rale.Z) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(PositionInitialeHbG�n�rale.X, GRANDEUR_HACHE_STANDARD, PositionInitialeHbG�n�rale.Z));
            HitBoxG�n�rale = new BoundingSphere(NouveauCentre, HitBoxG�n�rale.Radius);

            for (int i=0;i<TableauxHitBoxPr�cises.Count();++i)
            {
                
                Vector3 centre = Vector3.Transform(TableauPositionInitiales[i], Matrix.CreateTranslation(-TableauPositionInitiales[i].X, -GRANDEUR_HACHE_STANDARD, -TableauPositionInitiales[i].Z) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(TableauPositionInitiales[i].X, GRANDEUR_HACHE_STANDARD, TableauPositionInitiales[i].Z));
                TableauxHitBoxPr�cises[i] = new BoundingSphere(centre, TableauxHitBoxPr�cises[i].Radius);
            }


        }

        private void CalculerNouvellePositionHache()
        {

            
            Matrix transform  = Matrix.CreateTranslation(-Position.X, -GRANDEUR_HACHE_STANDARD, -Position.Z) *
                      Matrix.CreateRotationZ(Angle) *
                      Matrix.CreateTranslation(Position.X, GRANDEUR_HACHE_STANDARD, Position.Z);

            Monde = MondeInitial * transform;
        }

        void Cr�erHitBoxG�n�rale()
        {
            HitBoxG�n�rale = new BoundingSphere(new Vector3(Position.X, Position.Y+3, Position.Z), 5);
            PositionInitialeHbG�n�rale = new Vector3(HitBoxG�n�rale.Center.X, HitBoxG�n�rale.Center.Y, HitBoxG�n�rale.Center.Z);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {

            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
            {
                for (int i = 0; i < NB_HITBOX_PR�CISES; i++)
                {
                    if((a as Soldat).HitBoxG�n�rale.Intersects(TableauxHitBoxPr�cises[i]))
                    {
                        Vector3 EntreSph�reetPers = (TableauxHitBoxPr�cises[i].Center - a.Position);
                       // (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, (a as Soldat).Vitesse.Y, 0);
                       Vector3 temp = sens * 2800 * new Vector3(1,0.5f,0); //(Angle / Math.Abs(Angle))
                        v = new Vector3(temp.X,Math.Abs(temp.Y),0);
                    }
                }      
            }
            return v;
        }

        void Cr�erHitBoxesPr�cises()
        {
            TableauxHitBoxPr�cises[0] = new BoundingSphere(new Vector3(Position.X, Position.Y, Position.Z),1f);
            TableauxHitBoxPr�cises[1] = new BoundingSphere(new Vector3(Position.X+0.5F, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[2] = new BoundingSphere(new Vector3(Position.X-0.5F, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[3] = new BoundingSphere(new Vector3(Position.X + 1, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[4] = new BoundingSphere(new Vector3(Position.X - 1, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[5] = new BoundingSphere(new Vector3(Position.X + 1.5f, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[6] = new BoundingSphere(new Vector3(Position.X - 1.5f, Position.Y, Position.Z), 1f);
            TableauxHitBoxPr�cises[7] = new BoundingSphere(new Vector3(Position.X , Position.Y +1.5f, Position.Z), 1f);

            TableauPositionInitiales = new Vector3[TableauxHitBoxPr�cises.Length];

            for(int i = 0;i<TableauxHitBoxPr�cises.Length;++i)
            {
                TableauPositionInitiales[i] = new Vector3(TableauxHitBoxPr�cises[i].Center.X, TableauxHitBoxPr�cises[i].Center.Y, TableauxHitBoxPr�cises[i].Center.Z);
            }


        }
    }
}
