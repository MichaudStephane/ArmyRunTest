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




        public TerrainDeBase(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel) 
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            
        }

        public BoundingBox HitBoxG�n�rale { get; protected set; }
        
     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {


            

                Vector3 v = Vector3.Zero;
            (a as Soldat).EstSurTerrain = true;
                if((a as  Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
                {
                // A CHANGER POUR INCLURE TOUTE LES DIRECTIONS. CHECKER POUR UNE FACON PLUS ELEGANTE DE LECRIRE
                    if((a as Soldat).VecteurResultantForce.Y<0)
                    v = new Vector3(0, -(a as Soldat).VecteurResultantForce.Y, 0);

                    if((a as Soldat).Vitesse.Y<0)
                    (a as Soldat).Vitesse = new Vector3((a as Soldat).Vitesse.X, 0, (a as Soldat).Vitesse.Z);



                }

            return v + GarderHorsBornes((a as Soldat));


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
                return v;

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
