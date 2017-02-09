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




        public TerrainDeBase(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel) 
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            
        }

        public bool EstCollisionable
        {
            get
            {
                return true;
            }
        }

        public BoundingBox HitBoxGénérale { get; protected set; }
        
     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {
         

            Vector3 v = Vector3.Zero;
            if(a is ICollisionable)
                if((a as  Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
                {
                    v = new Vector3(0, -(a as Soldat).VecteurResultant.Y, 0);
                }

            return v;
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


            Vector3 minHB = new Vector3(-0.5f * TAILLE_HITBOX_STANDARD.X, -0.5f * TAILLE_HITBOX_STANDARD.Y, -0.5F*TAILLE_HITBOX_STANDARD.Z);
            Vector3 maxHB = new Vector3(0.5f * TAILLE_HITBOX_STANDARD.X, 0.5f * TAILLE_HITBOX_STANDARD.Y, 0.5F*TAILLE_HITBOX_STANDARD.Z);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homothétie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homothétie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(PositionInitiale));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(PositionInitiale));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));


            HitBoxGénérale = new BoundingBox(minHB,maxHB);
            int A = 1;
        }

    }


 }
