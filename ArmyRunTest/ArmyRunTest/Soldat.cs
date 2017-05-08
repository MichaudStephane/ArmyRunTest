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
   public class Soldat : Humanoide,IDeletable
    {
        public const float CONSTANTE_SAUT = 6000f;
        const float CONSTANTE_GRAVITE = 9.81F;
        protected const float NB_PIXEL_DÉPLACEMENT = 80f;
        const float INTERVALLE_DE_DEPART_STANDARD = 1f/30;
        const float MASSE_SOLDAT_KG = 10;
        const float DENSITER_AIR =1.225F;  //KG/M CUBE
        const float DRAG_COEFFICIENT = 40.05F;
        const float NORMALE = (MASSE_SOLDAT_KG * 9.8f) ;
        const float FROTTEMENT = 0.75F * NORMALE * INTERVALLE_CALCUL_PHYSIQUE*5 ;
        const float INTERVALLE_CALCUL_PHYSIQUE = 1f / 60;
        public Vector3 VarPosition { get; set; }
        public BoundingBox HitBoxGénérale { get; protected set; }   
        Vector3 VecteurGravité { get; set; }
        Vector3 AnciennePosition { get; set; }
        public float Intervalle_MAJ_Mouvement { get;  set; }
        public float TempsEcouleDepuisMajMouvement { get; set; }
        float TempsEcoulerDepuisMAJCalcul { get; set; }
        public bool EstEnCollision { get; set; }
        public Vector3 VecteurResultantForce { get; protected set; }
       protected Vector3 Acceleration { get; set; }
        public Vector3 Vitesse { get; set; }
        protected Vector3 Commande { get; set; }
        public bool EstSurTerrain { get; set; }
        SoundEffect SonSaut { get; set; }
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }
        CaméraAutomate Caméra { get; set; }



        public Soldat(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImageDos,string nomImageFace ,Vector2 descriptionImageDos,Vector2 DescriptionImageFace, float intervalleMAJ)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos,nomImageFace, descriptionImageDos,DescriptionImageFace, intervalleMAJ) 
        {
            Caméra = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
        }
        public override void Initialize()
        {
            base.Initialize();
            VarPosition = Position;
            TempsEcoulerDepuisMAJCalcul = 0;
            Commande = Vector3.Zero;
            VecteurGravité = new Vector3(0, -CONSTANTE_GRAVITE, 0);
            VecteurResultantForce = Vector3.Zero;
            Vitesse = Vector3.Zero;
            Acceleration = Vector3.Zero;
            EstEnCollision = false;
            EstSurTerrain = false; 
          
            Intervalle_MAJ_Mouvement = INTERVALLE_DE_DEPART_STANDARD;
            TempsEcouleDepuisMajMouvement = 0;

            CreerHitbox();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            VecteurResultantForce = Vector3.Zero;

            TempsEcouleDepuisMajMouvement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsEcoulerDepuisMAJCalcul += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(TempsEcoulerDepuisMAJCalcul>=INTERVALLE_CALCUL_PHYSIQUE)
            {
                CalculerForcesExercees();
                CalculerAcceleration();
                AnciennePosition = Position;
                BougerHitbox();
                TempsEcoulerDepuisMAJCalcul = 0;
            }
          
            if (TempsEcouleDepuisMajMouvement >= Intervalle_MAJ_Mouvement)
            {
                Position = VarPosition;
                CalculerMatriceMonde();
                TempsEcouleDepuisMajMouvement = 0;            
            }
        }

       //TEMPORAIRE
        protected override void GérerClavier()
        {
            
            /// POUR TESTER HITBOX SEULEUMENT SOLDAT DE LARMER NE SE CONTROLE PAS DIRECTEMENT
            float déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A); //à inverser au besoin
            float déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);


            if(GestionInput.EstNouvelleTouche(Keys.R))
            {
                Position = PositionInitiale;
                VecteurResultantForce = Vector3.Zero;
                Vitesse = Vector3.Zero;
                Acceleration = Vector3.Zero;
                VarPosition = Position;
                CreerHitbox();
            }

            if (!GestionInput.EstEnfoncée(Keys.LeftShift))
            {
                if (GestionInput.EstNouvelleTouche(Keys.Space))
                {
                    if (EstSurTerrain)
                    {
                        BougerHitbox();
                        
                                          
                    }
                }
                if (déplacementGaucheDroite != 0 || déplacementAvantArrière != 0)
                {
                    AjouterVecteur(déplacementAvantArrière, déplacementGaucheDroite);
                   
                }
                
            }
            
        }
   

        void AjouterVecteur(float déplacementAvantArrière, float déplacementGaucheDroite)
        {     
            Commande = new Vector3(déplacementGaucheDroite, Commande.Y, déplacementAvantArrière);          
        }
        void AjouterVecteur(float déplacementSaut)
        {
            Commande = new Vector3(Commande.X, déplacementSaut, Commande.Y);
        }

        float GérerTouche(Keys k)
         {
            return GestionInput.EstEnfoncée(k) ? NB_PIXEL_DÉPLACEMENT : 0;
        }
       public override void ModifierPosition(Vector3 NouvellePosition)
        {
            VarPosition = NouvellePosition;
        }
     
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {
            return Vector3.Zero;
        }
        


        
        private void GererFrottement()
        {
            //-----------------------A METTRE LE FROTTEMENT DYNAMIQUE ET STATIQUE------------------------------------
            //frottement avec sol (peut etre changer la valeur de la division)
            if (EstSurTerrain)
            {
                if (Vitesse.X != 0)
                {
                    VecteurResultantForce -= new Vector3(FROTTEMENT * (Vitesse.X / Math.Abs(Vitesse.X)), 0, 0);
                }

                if (Vitesse.Z != 0)
                {
                    VecteurResultantForce -= new Vector3(0, 0, FROTTEMENT * (Vitesse.Z / Math.Abs(Vitesse.Z)));
                }
            }

        
            Vector3 vitesseCal = Vitesse;
            Vector3 fAir = Vector3.Multiply(Vector3.Multiply(vitesseCal, vitesseCal), 0.010f * 2 * 0.5f * DENSITER_AIR * DRAG_COEFFICIENT);


            VecteurResultantForce += new Vector3(Convert.ToInt32(Vitesse.X>0)*-fAir.X + Convert.ToInt32(Vitesse.X < 0) * fAir.X, Convert.ToInt32(Vitesse.X > 0) * -fAir.Y + Convert.ToInt32(Vitesse.Y < 0) * fAir.Y, Convert.ToInt32(Vitesse.Z > 0) * -fAir.Z + Convert.ToInt32(Vitesse.Z < 0) * fAir.Z);
        }

        //----A MODIFIER----
        protected override void GererCollision()
        {

            EstEnCollision = false; ;
            EstSurTerrain = false;
            Vector3 V = VecteurResultantForce;
            // FAIRE EN SORTE QUELLE NE VEFIE QUE LES ELEMENTS PROCHES
            foreach (GameComponent G in Game.Components.Where(x => x is ICollisionable).ToList())
            {
                VecteurResultantForce += ((G as ICollisionable).DonnerVectorCollision(this));
            }
            if (V != VecteurResultantForce)
            {
                EstEnCollision = true;
            }
        }

      protected void CreerHitbox()
        {
            Vector3 minHB = new Vector3(-0.5f * Delta.X, -0.4f * Delta.Y, -0.1f);
            Vector3 maxHB = new Vector3(0.5f * Delta.X, 0.5f * Delta.Y, 0.1f);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homothétie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homothétie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(Position));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(Position));

            HitBoxGénérale = new BoundingBox(minHB,maxHB);

           
           
        }
       void BougerHitbox()
       {


            Vector3 minHB = new Vector3(-0.5f * Delta.X, -0.4f * Delta.Y, -0.2f);
            Vector3 maxHB = new Vector3(0.5f * Delta.X, 0.5f * Delta.Y, 0.2f);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homothétie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homothétie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(VarPosition));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(VarPosition));

            HitBoxGénérale = new BoundingBox(minHB, maxHB);



       }
      

       void CalculerForcesExercees()
       {
            VecteurResultantForce += Commande;
            Commande = Vector3.Zero;
            GererCollision();
            if (!EstSurTerrain)
            {
                VecteurResultantForce += Vector3.Multiply(VecteurGravité, MASSE_SOLDAT_KG);
            }
            GererFrottement();
       }
       void CalculerAcceleration()
       {
           Vector3 accelerationPrecedente = Acceleration;
           Vector3 varPosition = Vector3.Multiply(Vitesse, INTERVALLE_CALCUL_PHYSIQUE) + Vector3.Multiply(Vector3.Multiply(accelerationPrecedente, 0.5f), (float)Math.Pow(INTERVALLE_CALCUL_PHYSIQUE, 2));
           VarPosition = new Vector3(VarPosition.X + varPosition.X, VarPosition.Y + varPosition.Y, VarPosition.Z + varPosition.Z);
           Acceleration = Vector3.Multiply(VecteurResultantForce,1f/MASSE_SOLDAT_KG);

           Vitesse+=Acceleration*INTERVALLE_CALCUL_PHYSIQUE;
           BougerHitbox();
       }
    }
}
