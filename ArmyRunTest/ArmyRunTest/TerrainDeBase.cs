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



        public TerrainDeBase(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel) 
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            
        }

        public BoundingBox HitBoxG�n�rale { get; protected set; }
        
     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {
            Vector3 vitesseTemp = (a as Soldat).Vitesse;
            Vector3 forceTemp = (a as Soldat).VecteurResultantForce;
            Vector3 v = Vector3.Zero;
            if (HitBoxG�n�rale.Intersects((a as Soldat).HitBoxG�n�rale))
            {

                v = a.Position - Position;
                v = new Vector3(v.X / TAILLE_HITBOX_STANDARD.X, v.Y / TAILLE_HITBOX_STANDARD.Y, v.Z / TAILLE_HITBOX_STANDARD.Z);
                v = Convert.ToInt32((v.Y >= v.X) && (v.Y >= v.Z) &&forceTemp.Y<0&&vitesseTemp.Y<=0) * VECTOR_HAUT;//+ Convert.ToInt32((v.Y <= v.X) && (v.Y <= v.Z) && vitesseTemp.Y>0) * VECTOR_BAS;
                (a as Soldat).EstSurTerrain = true;
                (a as Soldat).EstEnCollision = true;

                if ((v.Y >= v.X) && (v.Y >= v.Z) && vitesseTemp.Y < 0)
                {
                    int a6gh = 3;
                }

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
            //if ((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
            //{
            //    // A CHANGER POUR INCLURE TOUTE LES DIRECTIONS. CHECKER POUR UNE FACON PLUS ELEGANTE DE LECRIRE
            //    if ((a as Soldat).VecteurResultantForce.Y < 0)
            //        v = new Vector3(0, -(a as Soldat).VecteurResultantForce.Y, 0);

            //    if ((a as Soldat).Vitesse.Y < 0)
            //        (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, 0, (a as Soldat).Vitesse.Z);



            //     }
            return -v;
            //return -v + GarderHorsBornes((a as Soldat));


        }
        private Vector3 GarderHorsBornes(Soldat a)
        {
            Vector3 v = Vector3.Zero;


            if ((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
            {
                if (HitBoxG�n�rale.Max.Y > a.HitBoxG�n�rale.Min.Y && HitBoxG�n�rale.Min.Y < a.HitBoxG�n�rale.Max.Y)
                {
                    //   float NouvellePositionY = a.VarPosition.Y + Math.Abs((HitBoxG�n�rale.Max.Y - a.VarPosition.Y));
                    // a.ModifierVarPosition(new Vector3(a.VarPosition.X, NouvellePositionY, a.VarPosition.Z));
                    v = new Vector3 (0,HitBoxG�n�rale.Max.Y - a.HitBoxG�n�rale.Min.Y,0);
                   
                }
                
            }
                return 10*v;

        }

  
        public override void Initialize()
        {
           

            
            base.Initialize();
            Cr�erHitboxG�n�rale();
        }

  
        public override void Update(GameTime gameTime)
        {
           Vector3 posPre = Position;

            base.Update(gameTime);


            if(posPre!=Position)
                  Cr�erHitboxG�n�rale();

        }

        void Cr�erHitboxG�n�rale()
        {

            Vector3 minHB = new Vector3(-0.5f * TAILLE_HITBOX_STANDARD.X, -0.1f * TAILLE_HITBOX_STANDARD.Y, -0.5F*TAILLE_HITBOX_STANDARD.Z);
            Vector3 maxHB = new Vector3(0.5f * TAILLE_HITBOX_STANDARD.X, 0.5f * TAILLE_HITBOX_STANDARD.Y, 0.5F*TAILLE_HITBOX_STANDARD.Z);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homoth�tie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homoth�tie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(PositionInitiale));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(PositionInitiale));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));


            HitBoxG�n�rale = new BoundingBox(minHB,maxHB);
            
        }

    }


 }
