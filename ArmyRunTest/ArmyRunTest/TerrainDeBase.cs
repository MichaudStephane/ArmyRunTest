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
        Vector3 TAILLE_HITBOX_STANDARD = new Vector3(1,0.775555f,0.785F);
        Vector3 VECTOR_HAUT = new Vector3(0, 1, 0);
        Vector3 VECTOR_BAS = new Vector3(0, -1, 0);
        Vector3 VECTOR_GAUCHE = new Vector3(-1, 0, 0);
        Vector3 VECTOR_DROITE = new Vector3(1, 0, 0);
        Vector3 VECTOR_DEVANT = new Vector3(0, 0, 1);
        Vector3 VECTOR_DERRIERE = new Vector3(0, 0, -1);



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
              

                v = a.Position - Position;
                v = new Vector3(v.X / TAILLE_HITBOX_STANDARD.X*0.9995f, v.Y / TAILLE_HITBOX_STANDARD.Y, v.Z / TAILLE_HITBOX_STANDARD.Z*0.9995f);
                v = Convert.ToInt32((v.Y >= v.X) && (v.Y >= v.Z) && (vitesseTemp.Y < -0.0001f)) * VECTOR_HAUT
               // +Convert.ToInt32((v.Y <= v.X) && (v.Y <= v.Z) && vitesseTemp.Y > 0) * VECTOR_BAS
                + Convert.ToInt32((v.Z >= v.X) && (v.Z >= v.Y) && vitesseTemp.Z < -0.0001f) * VECTOR_DEVANT
                ;



                (a as Soldat).EstSurTerrain = true;
                (a as Soldat).EstEnCollision = true;
                //if ((a as Soldat).Vitesse.Y < 0)
                //    (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, 0, (a as Soldat).Vitesse.Z);


            }
            //if (v.Y != 0)
            //{
            //    (a as Soldat).Vitesse = new Vector3(vitesseTemp.X, 0, vitesseTemp.Z);
            //}

              (a as Soldat).Vitesse -= new Vector3(v.X * vitesseTemp.X, v.Y * vitesseTemp.Y, v.Z * vitesseTemp.Z);
            v = new Vector3(v.X * forceTemp.X, v.Y * forceTemp.Y, v.Z * forceTemp.Z);


            //Vector3 v = Vector3.Zero;
            //(a as Soldat).EstSurTerrain = true;
            //if ((a as Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
            //{
            //    // A CHANGER POUR INCLURE TOUTE LES DIRECTIONS. CHECKER POUR UNE FACON PLUS ELEGANTE DE LECRIRE
            //    if ((a as Soldat).VecteurResultantForce.Y < 0)
            //        v = new Vector3(0, -(a as Soldat).VecteurResultantForce.Y, 0);

            //    if ((a as Soldat).Vitesse.Y < 0)
            //        (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, 0, (a as Soldat).Vitesse.Z);



            //     }

            return -v+ GarderHorsBornes((a as Soldat));


        }
        private Vector3 GarderHorsBornes(Soldat a)
        {
            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
            {
                if (HitBoxGénérale.Max.Y > a.HitBoxGénérale.Min.Y && HitBoxGénérale.Min.Y < a.HitBoxGénérale.Max.Y)
                {
                    //   float NouvellePositionY = a.VarPosition.Y + Math.Abs((HitBoxGénérale.Max.Y - a.VarPosition.Y));
                    // a.ModifierVarPosition(new Vector3(a.VarPosition.X, NouvellePositionY, a.VarPosition.Z));
                    v = new Vector3 (0,1,0);
                   
                }
                
            }
                return (HitBoxGénérale.Max.Y-(a as Soldat).HitBoxGénérale.Min.Y)*v;

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

        void CréerHitboxGénérale()
        {

            Vector3 minHB = new Vector3(-0.5f * TAILLE_HITBOX_STANDARD.X, -0.1f * TAILLE_HITBOX_STANDARD.Y, -0.5F*TAILLE_HITBOX_STANDARD.Z);
            Vector3 maxHB = new Vector3(0.5f * TAILLE_HITBOX_STANDARD.X, 0.5f * TAILLE_HITBOX_STANDARD.Y, 0.5F*TAILLE_HITBOX_STANDARD.Z);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homothétie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homothétie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(PositionInitiale));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(PositionInitiale));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));


            HitBoxGénérale = new BoundingBox(minHB,maxHB);
            
        }

    }


 }
