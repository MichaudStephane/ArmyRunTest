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
   public class Soldat : Humanoide, ICollisionable
    {
        const float CONSTANTE_SAUT = 20f;
        const float CONSTANTE_GRAVITE = 9.81F;
        const float NB_PIXEL_D�PLACEMENT = 10f;
        const float INTERVALLE_DE_DEPART_STANDARD = 1f/60;
        const float MASSE_SOLDAT_KG = 10;
        const float DENSITER_AIR =1.225F;  //KG/M CUBE
        const float DRAG_COEFFICIENT = 1.05F;


     

        const float INTERVALLE_CALCUL_PHYSIQUE = 1f / 60;

        public BoundingBox HitBoxG�n�rale { get; protected set; }
        
        Vector3 VecteurGravit� { get; set; }
       
        
       
        Vector3 AnciennePosition { get; set; }
        
        public float Intervalle_MAJ_Mouvement { get;  set; }
        public float TempsEcouleDepuisMajMouvement { get; set; }



        public bool EstEnCollision { get; set; }
     
       public Vector3 VecteurResultantForce { get; protected set; }
       Vector3 Acceleration { get; set; }
        public Vector3 Vitesse { get; set; }
       Vector3 Commande { get; set; }





        public Soldat(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 �tendue, string nomImageDos,string nomImageFace ,Vector2 descriptionImageDos,Vector2 DescriptionImageFace, float intervalleMAJ)
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, �tendue, nomImageDos,nomImageFace, descriptionImageDos,DescriptionImageFace, intervalleMAJ)
        {
           
        }
        public override void Initialize()
        {
            base.Initialize();
            Commande = Vector3.Zero;
            VecteurGravit� = new Vector3(0, -CONSTANTE_GRAVITE, 0);
            VecteurResultantForce = Vector3.Zero;
            Vitesse = Vector3.Zero;
            Acceleration = Vector3.Zero;
          
            Intervalle_MAJ_Mouvement = INTERVALLE_DE_DEPART_STANDARD;
            TempsEcouleDepuisMajMouvement = 0;

            CreerHitbox();
           
          
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            VecteurResultantForce = Vector3.Zero;

            TempsEcouleDepuisMajMouvement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsEcouleDepuisMajMouvement >= Intervalle_MAJ_Mouvement)
            {
                CalculerForcesExercees();
                CalculerAcceleration();
                AnciennePosition = Position;
                BougerHitbox();
                CalculerMatriceMonde();
                TempsEcouleDepuisMajMouvement = 0;            
            }


            GameWindow a =Game.Window;
            a.Title = "Vitesse:[ " + Math.Round(Vitesse.X, 2) + "   " + Math.Round(Vitesse.Y, 2) + "   " + Math.Round(Vitesse.Z, 2) + 
                "] Position: [" + Math.Round(Position.X, 2) + "   " + Math.Round(Position.Y, 2) + "   " + Math.Round(Position.Z, 2) + "]"
                + "  Acceleration:[ " + Math.Round(Acceleration.X, 2) + "   " + Math.Round(Acceleration.Y, 2) + "   " + Math.Round(Acceleration.Z, 2) + "]"
                + "  VecteurForce:[ " + Math.Round(VecteurResultantForce.X, 2) + "   " + Math.Round(VecteurResultantForce.Y, 2) + "   " + Math.Round(VecteurResultantForce.Z, 2) + "]";
           
        }

       //TEMPORAIRE
        protected override void G�rerClavier()
        {

            /// POUR TESTER HITBOX SEULEUMENT SOLDAT DE LARMER NE SE CONTROLE PAS DIRECTEMENT



            float d�placementGaucheDroite = G�rerTouche(Keys.D) - G�rerTouche(Keys.A); //� inverser au besoin
            float d�placementAvantArri�re = G�rerTouche(Keys.S) - G�rerTouche(Keys.W);


            if(GestionInput.EstNouvelleTouche(Keys.R))
            {
                Position = PositionInitiale;
                VecteurResultantForce = Vector3.Zero;
                Vitesse = Vector3.Zero;
                Acceleration = Vector3.Zero;
                CreerHitbox();
            }

            if (!GestionInput.EstEnfonc�e(Keys.LeftShift))
            {
                if (GestionInput.EstNouvelleTouche(Keys.Space))
                {
                    AjouterVecteur(CONSTANTE_SAUT);
                }
                if (d�placementGaucheDroite != 0 || d�placementAvantArri�re != 0)
                {
                    AjouterVecteur(d�placementAvantArri�re, d�placementGaucheDroite);
                }
            }


            //--------------------------------------------------------------------
            
        }

        void AjouterVecteur(float d�placementAvantArri�re, float d�placementGaucheDroite)
        {
            Commande = new Vector3(d�placementGaucheDroite, Commande.Y, d�placementAvantArri�re);

           
        }
        void AjouterVecteur(float d�placementSaut)
        {
            Commande = new Vector3(Commande.X, d�placementSaut, Commande.Y);
        }

        float G�rerTouche(Keys k)
         {
            return GestionInput.EstEnfonc�e(k) ? NB_PIXEL_D�PLACEMENT : 0;
        }

     /*  protected override void AnimerImage()
        {
            CompteurY++;
            if (CompteurY <= 4)
            {
                CompteurY = 0;
            }
            if (VecteurGravit�.X != 0 || VecteurResultant.Z != 0) //gauche/droite/avant/arriere
            {
                CompteurX = 0;
            }
            if (VecteurResultant.Y > 0)//monter
            {
                CompteurX = 1;
            }
            if (VecteurResultant.Y < 0) //descend
            {
                CompteurX = 2;
            }
            Cr�erTableauPointsTexture();
        }
        */


     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {
            return Vector3.Zero;
        }

        
        private void GererFrottement()
        {
            ///-----------------------A METTRE LE FROTTEMENT DYNAMIQUE ET STATIQUE------------------------------------
                     
            




            //frottement air

            Vector3 vitesseCal = Vitesse;
            Vector3 fAir = Vector3.Multiply(Vector3.Multiply(vitesseCal, vitesseCal), 0.010f*2*0.5f * DENSITER_AIR * DRAG_COEFFICIENT);

            VecteurResultantForce += fAir;
        }

        //----A MODIFIER----
        void GererCollision()
        {
            EstEnCollision = false;

            Vector3 V = VecteurResultantForce;
            // FAIRE EN SORTE QUELLE NE VEFIE QUE LES ELEMENTS PROCHES
            foreach(GameComponent G in Game.Components.Where(x=>x is ICollisionable).ToList())
            {
                


                VecteurResultantForce+=((G as ICollisionable).DonnerVectorCollision(this));     
    
            }
            if(V!=VecteurResultantForce)
            {
                EstEnCollision = true;
            }
           
        }

       void CreerHitbox()
        {
            Vector3 minHB = new Vector3(-0.5f * Delta.X, -0.4f * Delta.Y, -0.1f);
            Vector3 maxHB = new Vector3(0.5f * Delta.X, 0.5f * Delta.Y, 0.1f);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homoth�tie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homoth�tie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(Position));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(Position));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));

            HitBoxG�n�rale = new BoundingBox(minHB,maxHB);

           
           
        }
       void BougerHitbox()
       {
           Vector3 diff =Position- HitBoxG�n�rale.Min +Vector3.Multiply((HitBoxG�n�rale.Max - HitBoxG�n�rale.Min),0.5f);

           HitBoxG�n�rale = new BoundingBox(HitBoxG�n�rale.Min + diff, HitBoxG�n�rale.Max + diff);


       }

       void CalculerForcesExercees()
       {
           // faire pour les force de rotations aussi


           VecteurResultantForce += Vector3.Multiply(VecteurGravit�,MASSE_SOLDAT_KG);
           VecteurResultantForce += Commande;
           Commande = Vector3.Zero;
           GererCollision();
           GererFrottement();


       }
       void CalculerAcceleration()
       {
           Vector3 vitesseCal = Vitesse;
           Vector3 accCal = Acceleration;
           Vector3 vecForceCal = VecteurResultantForce;

           Vector3 accelerationPrecedente = Acceleration;
           Vector3 varPosition = Vector3.Multiply(vitesseCal,INTERVALLE_CALCUL_PHYSIQUE)+Vector3.Multiply(Vector3.Multiply(accelerationPrecedente,0.5f),(float)Math.Pow(INTERVALLE_CALCUL_PHYSIQUE,2));
           Position = new Vector3(Position.X+varPosition.X,Position.Y+varPosition.Y,Position.Z+varPosition.Z);
           Acceleration = Vector3.Multiply(vecForceCal,1f/MASSE_SOLDAT_KG);



           Acceleration = new Vector3((float)Math.Round(Acceleration.X, 2), (float)Math.Round(Acceleration.Y, 2), (float)Math.Round(Acceleration.Z, 2));
     
           if (Acceleration != Vector3.Zero)
           {

               Acceleration = Vector3.Multiply(accelerationPrecedente + Acceleration, 0.5f);
           }


           Vitesse+=Acceleration*INTERVALLE_CALCUL_PHYSIQUE;

           Vitesse = new Vector3((float)Math.Round(Vitesse.X, 2), (float)Math.Round(Vitesse.Y, 2), (float)Math.Round(Vitesse.Z, 2));
           Acceleration = new Vector3((float)Math.Round(Acceleration.X, 2), (float)Math.Round(Acceleration.Y, 2), (float)Math.Round(Acceleration.Z, 2));
     

           BougerHitbox();
       }









       void ModifierIntervalle()
       {

       }
       void InitialiserListe()
       {

       }
       void CalculerVecteurResultant()
       {

       }

    }
}
