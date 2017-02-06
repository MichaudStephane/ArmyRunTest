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
        const float CONSTANTE_GRAVIT� = 9.8f;
        const int NB_PIXEL_D�PLACEMENT = 2;
        List<Vector3> ListeVecteurs { get; set; }
        Vector3 VecteurGravit� { get; set; }
        Vector3 VecteurResultants
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


        public Soldat(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            VecteurGravit� = new Vector3(0, CONSTANTE_GRAVIT�, 0);//IBougeable changer Y
        }

        public override void Update(GameTime gameTime)
        {
            G�rerClavier();
            base.Update(gameTime);
        }

        protected override void G�rerClavier()
        {
            int d�placementGaucheDroite = G�rerTouche(Keys.D) - G�rerTouche(Keys.A); //� inverser au besoin
            int d�placementAvantArri�re = G�rerTouche(Keys.S) - G�rerTouche(Keys.W);
            if(GestionInput.EstNouvelleTouche(Keys.Space))
            {
                AjouterVecteur(10);
            }
            if (d�placementGaucheDroite != 0 || d�placementAvantArri�re != 0)
            {
                AjouterVecteur(d�placementAvantArri�re, d�placementGaucheDroite);
            }
        }

        private void AjouterVecteur(int d�placementAvantArri�re, int d�placementGaucheDroite)
        {
            Vector3 vecteur = new Vector3(d�placementGaucheDroite, 0, d�placementAvantArri�re);
            
            ListeVecteurs.Add(vecteur + VecteurGravit�);
        }
        private void AjouterVecteur(int d�placementSaut)
        {

        }

        private int G�rerTouche(Keys k)
        {
            return GestionInput.EstEnfonc�e(k) ? NB_PIXEL_D�PLACEMENT : 0;
        }
    }
}
