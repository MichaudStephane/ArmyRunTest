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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SoldatDeArmée : Soldat, ICollisionable
    {
        const int HAUTEUR_MINIMAL = -50;
        int NuméroSoldat { get; set; }
        GameComponent[][] ObjetCollisionné { get; set; }
        int Compteur { get; set; }
        Vector2[][] GrillePosition { get; set; }
        Vector2 Déplacement { get; set; }

        public SoldatDeArmée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, 
                            Vector2 étendue, string nomImageDos, string nomImageFace, Vector2 descriptionImageDos, 
                            Vector2 DescriptionImageFace, float intervalleMAJ, GameComponent[][] objetCollisionné)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos, nomImageFace, descriptionImageDos, DescriptionImageFace, intervalleMAJ)
        {
            ObjetCollisionné = objetCollisionné;
        }

        public override void Initialize()
        {
            GrillePosition = new Vector2[ObjetCollisionné.GetLength(0)][];  // a changer
            InitialiserPositionsGrille();
            Déplacement = new Vector2(7,7);// valeur test
            Compteur = 0;
            base.Initialize();
        }

        //protected override void GererCollision()
        //{

        //}

        public override void Update(GameTime gameTime)
        {
            //TempsEcouleDepuisMajMouvement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
             ReplacementPositionSoldat();
            
            base.Update(gameTime);
        }

        void ReplacementPositionSoldat()
        {
            for (int i = 0; i < ObjetCollisionné.GetLength(0); i++)
            {
                for (int j = 0; j < ObjetCollisionné.GetLength(1); j++)
                {
                    if(!EstÀLaBonnePosition(i,j))
                    {
                        //Super calcul à Lesage
                        float deltaX = GrillePosition[i][j].X - (ObjetCollisionné[i][j] as Soldat).Position.X;
                        float deltaZ = GrillePosition[i][j].Y - (ObjetCollisionné[i][j] as Soldat).Position.Z;
                        VecteurResultantForce += new Vector3(deltaX * Déplacement.X, 0, deltaZ * Déplacement.Y);
                        //pt changer vitesse??
                    }
                }
            }
        }

        protected override void GérerClavier()
        {
            float déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A); 
            float déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);


            if (GestionInput.EstNouvelleTouche(Keys.R))
            {
                Position = PositionInitiale;
            }
            if (!GestionInput.EstEnfoncée(Keys.LeftShift))
            {
                if (déplacementGaucheDroite != 0 || déplacementAvantArrière != 0)
                {
                    ChangerPositionsDeGrille(déplacementGaucheDroite, déplacementAvantArrière);
                }
            }
        }
        void InitialiserPositionsGrille()
        {
            for (int i = 0; i < GrillePosition.GetLength(0); i++)
            {
                for (int j = 0; j < GrillePosition.GetLength(1); j++)
                {
                    GrillePosition[i][j] = new Vector2((ObjetCollisionné[i][j] as Soldat).Position.X, (ObjetCollisionné[i][j] as Soldat).Position.Z);
                }
            }
        }

        void ChangerPositionsDeGrille(float x, float z)
        {
            for (int i = 0; i < GrillePosition.GetLength(0); i++)
            {
                for (int j = 0; j < GrillePosition.GetLength(1); j++)
                {
                    GrillePosition[i][j].X += x;
                    GrillePosition[i][j].Y += z;
                }
            }
        }

        float GérerTouche(Keys k)
        {
            return GestionInput.EstEnfoncée(k) ? NB_PIXEL_DÉPLACEMENT : 0;
        }

        protected void Saut()
        {
            for (int i = 0; i < ObjetCollisionné.GetLength(0); i++)
            {
                for (int j = 0; j < ObjetCollisionné.GetLength(1); j++)
                {
                    if ((ObjetCollisionné[i][j] as Soldat).EstSurTerrain)
                    {
                        VecteurResultantForce = new Vector3(VecteurResultantForce.X, VecteurResultantForce.Y + CONSTANTE_SAUT, VecteurResultantForce.Z);
                    }
                }
            }
        }

        void MourrirChute()
        {
            for (int i = 0; i < ObjetCollisionné.GetLength(0); i++)
            {
                for (int j = 0; j < ObjetCollisionné.GetLength(1); j++)
                {
                    if ((ObjetCollisionné[i][j] as Soldat).Position.Y <= HAUTEUR_MINIMAL)
                    {
                        Game.Components.Remove(ObjetCollisionné[i][j]);
                    }
                }
            }
        }

        void AjouterVecteurDéplacement()
        {

        }

        Vector3 GetPosition(int i, int j) //méthode pour trouver la position d'un soldat de l'armé sans avoir a utiliser les double boucles à chaque fois
        {
            Vector3 position = Vector3.Zero;

            position = (ObjetCollisionné[i][j] as Soldat).Position;

            return position;
        }
        bool EstÀLaBonnePosition(int i,int j)
        {
            bool estÀLaBonnePLace = false;
            if ((ObjetCollisionné[i][j] as Soldat).Position.X == GrillePosition[i][j].X &&
                ((ObjetCollisionné[i][j]as Soldat).Position.Z == GrillePosition[i][j].Y))
            {
                estÀLaBonnePLace = true;
            }
            return estÀLaBonnePLace;
        }
    }
}