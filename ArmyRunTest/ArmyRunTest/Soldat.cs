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
   public class Soldat : Humanoide, IBougeable, ICollisionable
    {
        const float CONSTANTE_SAUT = 20f;
        const float CONSTANTE_GRAVITE = 9.81F;
        const float NB_PIXEL_D�PLACEMENT = 0.00000000000005f;
        const float INTERVALLE_DE_DEPART_STANDARD = 1f/60;
     

        public BoundingBox HitBoxG�n�rale { get; protected set; }
        protected List<Vector3> ListeVecteurs { get; set; }
        Vector3 VecteurGravit� { get; set; }
        public Vector3 VecteurResultant { get; protected set; }
        Vector3 VecteurDirectant { get; set; }
       
        Vector3 AnciennePosition { get; set; }
        public bool EstEnCollision { get; set; }
       public float Intervalle_MAJ_Mouvement { get;  set; }
       public float TempsEcouleDepuisMajMouvement { get; set; }

     


        public Soldat(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 �tendue, string nomImage, Vector2 descriptionImage, float intervalleMAJ)
            : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, �tendue, nomImage, descriptionImage, intervalleMAJ)
        {
           
        }
        public override void Initialize()
        {
            base.Initialize();
            VecteurGravit� = new Vector3(0, -CONSTANTE_GRAVITE, 0);
            VecteurResultant = Vector3.Zero;
            VecteurDirectant = Vector3.Zero;
            ListeVecteurs = new List<Vector3>();
            Intervalle_MAJ_Mouvement = INTERVALLE_DE_DEPART_STANDARD;
            TempsEcouleDepuisMajMouvement = 0;

            CreerHitbox();
           
          
        }
        public override void Update(GameTime gameTime)
        {
            InitialiserListe();
            base.Update(gameTime);
            TempsEcouleDepuisMajMouvement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsEcouleDepuisMajMouvement >= Intervalle_MAJ_Mouvement)
            {
                
                AnciennePosition = Position;
              
                ModifierIntervalle();
                CreerHitbox();
                CalculerVecteurResultant();
                GererCollision();
                GererMouvement();
                CalculerMatriceMonde();
                TempsEcouleDepuisMajMouvement = 0;            
            }
            CreerHitbox();

           
        }


        protected override void G�rerClavier()
        {
            float d�placementGaucheDroite = G�rerTouche(Keys.D) - G�rerTouche(Keys.A); //� inverser au besoin
            float d�placementAvantArri�re = G�rerTouche(Keys.S) - G�rerTouche(Keys.W);


            if(GestionInput.EstNouvelleTouche(Keys.R))
            {
                Position = PositionInitiale;
                VecteurResultant = Vector3.Zero;
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
            
        }

        void AjouterVecteur(float d�placementAvantArri�re, float d�placementGaucheDroite)
        {
            Vector3 vecteur = new Vector3(d�placementGaucheDroite, 0, d�placementAvantArri�re);

            ListeVecteurs.Add(vecteur);
        }
        void AjouterVecteur(float d�placementSaut)
        {
              ListeVecteurs.Add(new Vector3(0, d�placementSaut, 0));
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


        protected override void Cr�erTableauPointsTexture()
        {

            // 0 1
            PtsTexture[0, 0] = new Vector2(Delta.X * CompteurX, Delta.Y * (CompteurY + 1));

            // 1  1
            PtsTexture[1, 0] = new Vector2(Delta.X * (CompteurX + 1), Delta.Y * (CompteurY + 1));

            // 0  0
            PtsTexture[0, 1] = new Vector2(Delta.X * CompteurX, Delta.Y * CompteurY);

            // 1 0
            PtsTexture[1, 1] = new Vector2(Delta.X * (CompteurX + 1), Delta.Y * CompteurY);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {
            return Vector3.Zero;
        }

        public void CalculerVecteurResultant()
        {
            Vector3 vTotal = Vector3.Zero;
            foreach(Vector3 v in ListeVecteurs)
            {
                vTotal += v;
            }
            VecteurResultant = vTotal;
        }
        public void InitialiserListe()
        {
        
            ListeVecteurs = new List<Vector3>();
            ListeVecteurs.Add(VecteurResultant);
           ListeVecteurs.Add(VecteurGravit�);
            VecteurResultant = Vector3.Zero;
        }

        public void ModifierIntervalle()
        {
            //--A MODIFIRER -----


        }
        void GererMouvement()
        {
            CalculerVecteurResultant();
            Vector3 v = VecteurResultant;
           
           // v.Normalize();
            GererFrottement();

            if(v!=Vector3.Zero)
            {
                v.Normalize();              
            }

            v = Vector3.Multiply(v, 0.3f);
            Position = new Vector3(Position.X+v.X, Position.Y + v.Y, Position.Z+v.Z);
            if(Position.Y<-10)
            {
                Position = PositionInitiale;
                VecteurResultant = Vector3.Zero;
            }
            CreerHitbox();
          
           
        }

        private void GererFrottement()
        {

            VecteurResultant = new Vector3(VecteurResultant.X - 0.2f * VecteurResultant.X / VecteurResultant.X, VecteurResultant.Y, VecteurResultant.Z - 0.2f * VecteurResultant.Z / VecteurResultant.Z);
        }

        //----A MODIFIER----
        void GererCollision()
        {

            // FAIRE EN SORTE QUELLE NE VEFIE QUE LES ELEMENTS PROCHES
            if(Position.Y<3)
            {
                int a = 1;
            }


            foreach(GameComponent G in Game.Components.Where(x=>x is ICollisionable).ToList())
            {
                ListeVecteurs.Add((G as ICollisionable).DonnerVectorCollision(this));
                CalculerVecteurResultant();
            }
           
        }

       void CreerHitbox()
        {
            Vector3 minHB = new Vector3(-0.5f * Delta.X, -0.5f * Delta.Y, -0.1f);
            Vector3 maxHB = new Vector3(0.5f * Delta.X, 0.5f * Delta.Y, 0.1f);

            minHB = Vector3.Transform(minHB, Matrix.CreateScale(Homoth�tie));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateScale(Homoth�tie));

            minHB = Vector3.Transform(minHB, Matrix.CreateTranslation(Position));
            maxHB = Vector3.Transform(maxHB, Matrix.CreateTranslation(Position));

            minHB = new Vector3((float)Math.Round(minHB.X, 3), (float)Math.Round(minHB.Y, 3), (float)Math.Round(minHB.Z, 3));
            maxHB = new Vector3((float)Math.Round(maxHB.X, 3), (float)Math.Round(maxHB.Y, 3), (float)Math.Round(maxHB.Z, 3));

            HitBoxG�n�rale = new BoundingBox(minHB,maxHB);

            int a = 1;
           
        }




    }
}
