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



        public SoldatDeArmée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImageDos, string nomImageFace, Vector2 descriptionImageDos, Vector2 DescriptionImageFace, float intervalleMAJ, int numéroSoldat, GameComponent[][] objetCollisionné)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos, nomImageFace, descriptionImageDos, DescriptionImageFace, intervalleMAJ)
        {
            ObjetCollisionné = objetCollisionné;
            NuméroSoldat = numéroSoldat;

        }

        public override void Initialize()
        {
            Compteur = 0;
            base.Initialize();
        }

        //protected override void GererCollision()
        //{

        //}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void GérerClavier() { }

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

        Vector3 GetPosition(int i) //méthode pour trouver la position d'un soldat de l'armé sans avoir a utiliser les double boucles à chaque fois
        {
            Vector3 v = Vector3.Zero;
            return v;
        }
    }
}