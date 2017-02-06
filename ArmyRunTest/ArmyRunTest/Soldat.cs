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
    class Soldat : Humanoide
    {
        const float CONSTANTE_SAUT = 20f;
        const float CONSTANTE_GRAVIT� = 9.8f;
        const int NB_PIXEL_D�PLACEMENT = 2;
        List<Vector3> ListeVecteurs { get; set; }
        Vector3 VecteurGravit� { get; set; }
        int PosX { get; set; }
        int PosY { get; set; }
        Vector3 VecteurResultant
        {
            get { return CalculerVecteurR�sultant(); }
        }
        Vector3 CalculerVecteurR�sultant()
        {
            Vector3 vecteurR�sultant = Vector3.Zero;
            foreach (Vector3 v in ListeVecteurs)
            {
                vecteurR�sultant += v;
            }

            return vecteurR�sultant;
        }


        public Soldat(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 �tendue, string nomImage, Vector2 descriptionImage, float intervalleMAJ)
         : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, �tendue, nomImage, descriptionImage, intervalleMAJ) 
        {
            VecteurGravit� = new Vector3(0, -CONSTANTE_GRAVIT�, 0);//IBougeable changer Y
        }
        public override void Initialize()
        {
            PosY = 0;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            G�rerClavier();
            base.Update(gameTime);
        }
        

        protected override void G�rerClavier()
        {
            float d�placementGaucheDroite = G�rerTouche(Keys.D) - G�rerTouche(Keys.A); //� inverser au besoin
            float d�placementAvantArri�re = G�rerTouche(Keys.S) - G�rerTouche(Keys.W);
            if(GestionInput.EstNouvelleTouche(Keys.Space))
            {
                AjouterVecteur(CONSTANTE_SAUT);
            }
            if (d�placementGaucheDroite != 0 || d�placementAvantArri�re != 0)
            {
                AjouterVecteur(d�placementAvantArri�re, d�placementGaucheDroite);
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

        int G�rerTouche(Keys k)
        {
            return GestionInput.EstEnfonc�e(k) ? NB_PIXEL_D�PLACEMENT : 0;
        }

        protected override void AnimerImage()
        {
            PosY++;
            if(PosY <=4)
            {
                PosY = 0;
            }
            if (VecteurResultant.X != 0 || VecteurResultant.Z != 0) //gauche/droite/avant/arriere
            {
                PosX = 0;
            }
            if (VecteurResultant.Y >0)//monter
            {
                PosX = 1;
            }
            if (VecteurResultant.Y < 0) //descend
            {
                PosX = 2;
            }
            Cr�erTableauPointsTexture();
        }
        protected override void Cr�erTableauPointsTexture()
        {

            // 0 1
            PtsTexture[0, 0] = new Vector2(Delta.X * PosX, Delta.Y * (PosY + 1));

            // 1  1
            PtsTexture[1, 0] = new Vector2(Delta.X * (PosX + 1), Delta.Y * (PosY + 1));

            // 0  0
            PtsTexture[0, 1] = new Vector2(Delta.X * PosX, Delta.Y * PosY);

            // 1 0
            PtsTexture[1, 1] = new Vector2(Delta.X * (PosX + 1), Delta.Y * PosY);
        }
    }
}
