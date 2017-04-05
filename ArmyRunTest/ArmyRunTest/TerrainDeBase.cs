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
    public class TerrainDeBase : ObjetBase, ICollisionable
    {
       const int HOMOTHETHIE_STANDARD = 10;
       static public Vector3 TAILLE_HITBOX_STANDARD = new Vector3(1,0.555f,0.724f);
       protected Vector3 VECTOR_HAUT = new Vector3(0, 1, 0);
       protected Vector3 VECTOR_BAS = new Vector3(0, -1, 0);
       protected Vector3 VECTOR_GAUCHE = new Vector3(-1, 0, 0);
       protected Vector3 VECTOR_DROITE = new Vector3(1, 0, 0);
       protected Vector3 VECTOR_DEVANT = new Vector3(0, 0, 1);
       protected Vector3 VECTOR_DERRIERE = new Vector3(0, 0, -1);

        //a faire friction pour terrain

        public TerrainDeBase(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel) 
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            
        }

        public BoundingBox HitBoxGénérale { get; protected set; }
        
     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {
            Vector3 vitesseTemp = (a as Soldat).Vitesse;

            Vector3 forceTemp = (a as Soldat).VecteurResultantForce;
            Vector3 v = Vector3.Zero;
            if (HitBoxGénérale.Intersects((a as Soldat).HitBoxGénérale))
            {

                // c mon

                v = a.Position - Position;
                v = new Vector3(v.X / (HOMOTHETHIE_STANDARD * TAILLE_HITBOX_STANDARD.X), v.Y / (HOMOTHETHIE_STANDARD * TAILLE_HITBOX_STANDARD.Y ), (v.Z / HOMOTHETHIE_STANDARD * TAILLE_HITBOX_STANDARD.Z));

                v = Convert.ToInt32((v.Y >= v.X) && (v.Y >= v.Z) && (vitesseTemp.Y < -0.0001f) && v.Y >= 0.0001f) * VECTOR_HAUT

                + Convert.ToInt32((v.Y <= v.X) && (v.Y <= v.Z) && (vitesseTemp.Y > 0.0001f) && (v.Y <= -0.0001f)) * VECTOR_BAS

                   + Convert.ToInt32((v.Z >= v.X) && (v.Z >= v.Y) && vitesseTemp.Z < -0.0001f && v.Z > 0.0001f) * VECTOR_DEVANT

                   + Convert.ToInt32((vitesseTemp.X >= 0.001f) && (v.X <= -0.0001f) && (Math.Abs(v.X) >= Math.Abs(v.Y)) && (Math.Abs(v.X) >= Math.Abs(v.Z))) * VECTOR_GAUCHE

                   + Convert.ToInt32(vitesseTemp.X <= -0.0001f && v.X >= 0.0001f && (Math.Abs(v.X) >= Math.Abs(v.Y)) && (Math.Abs(v.X) >= Math.Abs(v.Z))) * VECTOR_DROITE

                   + Convert.ToInt32((Math.Abs(v.Z) >= Math.Abs(v.X)) && (Math.Abs(v.Z) >= Math.Abs(v.Y)) && vitesseTemp.Z > 0.0001f && v.Z < -0.0001f) * VECTOR_DERRIERE
                ;

                if (v.Y > 0)
                    (a as Soldat).EstSurTerrain = true;

                (a as Soldat).EstEnCollision = true;

                (a as Soldat).Vitesse += new Vector3(v.X * Math.Abs(vitesseTemp.X), v.Y * Math.Abs(vitesseTemp.Y), v.Z * Math.Abs(vitesseTemp.Z));
                //  v = new Vector3(v.X * forceTemp.X, v.Y * forceTemp.Y, v.Z * forceTemp.Z);
                GarderHorsBornes((a as Soldat),v);

                SuivreTerrain(a as Soldat);

            }
            return -v;
        }
        private void GarderHorsBornes(Soldat a, Vector3 v)
        {
            //faire le reste
            if (v.X > 0 && a.HitBoxGénérale.Min.X < HitBoxGénérale.Max.X)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X + HitBoxGénérale.Max.X - a.HitBoxGénérale.Min.X,a.VarPosition.Y, a.VarPosition.Z));
            }
            if ( v.X < 0 && a.HitBoxGénérale.Max.X > HitBoxGénérale.Min.X)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X + HitBoxGénérale.Min.X - a.HitBoxGénérale.Max.X, a.VarPosition.Y, a.VarPosition.Z));
            }
            if ( v.Z > 0 && a.HitBoxGénérale.Max.Z < HitBoxGénérale.Min.Z)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X, a.VarPosition.Y, a.VarPosition.Z + HitBoxGénérale.Min.Z - a.HitBoxGénérale.Min.Z));
            }
            if ( v.Z < 0 && a.HitBoxGénérale.Min.Y > HitBoxGénérale.Max.Y)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X, a.VarPosition.Y, a.VarPosition.Z + HitBoxGénérale.Max.Z - a.HitBoxGénérale.Min.Z));
            }
            if ( v.Y < 0 && a.HitBoxGénérale.Max.Y > HitBoxGénérale.Min.Y)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X, a.VarPosition.Y + HitBoxGénérale.Min.Y - a.HitBoxGénérale.Max.Y, a.VarPosition.Z));
            }
            if ( v.Y>0 && a.HitBoxGénérale.Min.Y<HitBoxGénérale.Max.Y)
            {
                a.ModifierPosition(new Vector3(a.VarPosition.X, a.VarPosition.Y + HitBoxGénérale.Max.Y - a.HitBoxGénérale.Min.Y, a.VarPosition.Z));
            }




       


        }

  
        public override void Initialize()
        {
           

            
            base.Initialize();
            CréerHitboxGénérale();
        }

  
        public override void Update(GameTime gameTime)
        {
           Vector3 posPre = Position;

            base.Update(gameTime);


            if(posPre!=Position)
                  CréerHitboxGénérale();

        }

        protected void CréerHitboxGénérale()
        {

            Vector3 minHB = new Vector3(-0.5f * TAILLE_HITBOX_STANDARD.X, -0.1f * TAILLE_HITBOX_STANDARD.Y, -0.5F*TAILLE_HITBOX_STANDARD.Z);
            Vector3 maxHB = new Vector3(0.5f * TAILLE_HITBOX_STANDARD.X, 0.5f * TAILLE_HITBOX_STANDARD.Y, 0.5F*TAILLE_HITBOX_STANDARD.Z);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homothétie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homothétie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(Position));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(Position));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));


            HitBoxGénérale = new BoundingBox(minHB,maxHB);
            
        }
      protected  virtual void SuivreTerrain(Soldat a)
        {

        }

    }


 }
