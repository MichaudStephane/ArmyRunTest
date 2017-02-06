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
        const float CONSTANTE_GRAVITÉ = 9.8f;
        const int NB_PIXEL_DÉPLACEMENT = 2;
        List<Vector3> ListeVecteurs { get; set; }
        Vector3 VecteurGravité { get; set; }
        int PosX { get; set; }
        int PosY { get; set; }
        Vector3 VecteurResultant
        {
            get { return CalculerVecteurRésultant(); }
        }
        Vector3 CalculerVecteurRésultant()
        {
            Vector3 vecteurRésultant = Vector3.Zero;
            foreach (Vector3 v in ListeVecteurs)
            {
                vecteurRésultant += v;
            }

            return vecteurRésultant;
        }


        public Soldat(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImage, Vector2 descriptionImage, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImage, descriptionImage, intervalleMAJ) 
        {
            VecteurGravité = new Vector3(0, -CONSTANTE_GRAVITÉ, 0);//IBougeable changer Y
        }
        public override void Initialize()
        {
            PosY = 0;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            GérerClavier();
            base.Update(gameTime);
        }
        

        protected override void GérerClavier()
        {
            float déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A); //à inverser au besoin
            float déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);
            if(GestionInput.EstNouvelleTouche(Keys.Space))
            {
                AjouterVecteur(CONSTANTE_SAUT);
            }
            if (déplacementGaucheDroite != 0 || déplacementAvantArrière != 0)
            {
                AjouterVecteur(déplacementAvantArrière, déplacementGaucheDroite);
            }
        }

        void AjouterVecteur(float déplacementAvantArrière, float déplacementGaucheDroite)
        {
            Vector3 vecteur = new Vector3(déplacementGaucheDroite, 0, déplacementAvantArrière);
            
            ListeVecteurs.Add(vecteur);
        }
        void AjouterVecteur(float déplacementSaut)
        {
            ListeVecteurs.Add(new Vector3(0, déplacementSaut, 0));
        }

        int GérerTouche(Keys k)
        {
            return GestionInput.EstEnfoncée(k) ? NB_PIXEL_DÉPLACEMENT : 0;
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
            CréerTableauPointsTexture();
        }
        protected override void CréerTableauPointsTexture()
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
