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
        const float CONSTANTE_GRAVITÉ = 9.8f;
        const int NB_PIXEL_DÉPLACEMENT = 2;
        List<Vector3> ListeVecteurs { get; set; }
        Vector3 VecteurGravité { get; set; }
        Vector3 VecteurResultants
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


        public Soldat(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            VecteurGravité = new Vector3(0, CONSTANTE_GRAVITÉ, 0);//IBougeable changer Y
        }

        public override void Update(GameTime gameTime)
        {
            GérerClavier();
            base.Update(gameTime);
        }

        protected override void GérerClavier()
        {
            int déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A); //à inverser au besoin
            int déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);
            if(GestionInput.EstNouvelleTouche(Keys.Space))
            {
                AjouterVecteur(10);
            }
            if (déplacementGaucheDroite != 0 || déplacementAvantArrière != 0)
            {
                AjouterVecteur(déplacementAvantArrière, déplacementGaucheDroite);
            }
        }

        private void AjouterVecteur(int déplacementAvantArrière, int déplacementGaucheDroite)
        {
            Vector3 vecteur = new Vector3(déplacementGaucheDroite, 0, déplacementAvantArrière);
            
            ListeVecteurs.Add(vecteur + VecteurGravité);
        }
        private void AjouterVecteur(int déplacementSaut)
        {

        }

        private int GérerTouche(Keys k)
        {
            return GestionInput.EstEnfoncée(k) ? NB_PIXEL_DÉPLACEMENT : 0;
        }
    }
}
