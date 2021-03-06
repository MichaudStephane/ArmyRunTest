﻿using System;
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
        const int INTERVALLE_VERIFICATION=1;
        const int DISTANCE_MAX = 60;
        int NuméroSoldat { get; set; }
        List<PrimitiveDeBase>[] ObjetCollisionné { get; set; }
        int Compteur { get; set; }
        float TempsDepuisDernierCible { get; set; }
        Vector3 PositionCible { get; set; }
        Vector2 Déplacement { get; set; }
        float TempsVerification { get; set; }
        public bool EstVivant{get; set;}

        List<SectionDeNiveau> ListeSections { get; set; }

        public SoldatDeArmée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, 
                            Vector2 étendue, string nomImageDos, string nomImageFace, Vector2 descriptionImageDos,
                            Vector2 DescriptionImageFace, float intervalleMAJ, List<PrimitiveDeBase>[] objetCollisionné, List<SectionDeNiveau> listeSections)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos, nomImageFace, descriptionImageDos, DescriptionImageFace, intervalleMAJ)
        {
            ListeSections = listeSections;
            ObjetCollisionné = objetCollisionné;
        }
        public override void Initialize()
        {  
            Déplacement = new Vector2(7,7);
            Compteur = 0;
            TempsDepuisDernierCible = 0;
            TempsVerification = 0;
            EstVivant = true;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (EstVivant)
            {
                TempsDepuisDernierCible += (float)gameTime.ElapsedGameTime.TotalSeconds;
                TempsVerification += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (TempsDepuisDernierCible > Intervalle_MAJ_Mouvement)
                {
                    TenterDAtteindrePositionCible();
                    TempsDepuisDernierCible = 0;

                }
                if (TempsVerification >= INTERVALLE_VERIFICATION)
                {
                    VerifierSiMort();
                    TempsVerification = 0;
                }
                base.Update(gameTime);
            }          
        }
        void TenterDAtteindrePositionCible()
        {
           Vector3 difference = new Vector3(PositionCible.X - VarPosition.X, 0,2* (PositionCible.Z - VarPosition.Z));
            float intensiteDifference = difference.Length();

           if (difference != Vector3.Zero)
           {
               difference.Normalize();
           }
           intensiteDifference = 1.5f*Math.Min(Math.Max(intensiteDifference, 30), 200);
            if(EstSurTerrain)
            {
                Commande = intensiteDifference * difference;
                Commande = new Vector3(1.5F * Commande.X, Commande.Y, Commande.Z);
            }
            if( Commande.X<0 && PositionCible.X-Position.X >0)
            {
                Commande = new Vector3(1.5F * Commande.X, Commande.Y, Commande.Z);
            }
        }
        public void ModifierCompteur(int nouveauCompteur)
        {
            Compteur = nouveauCompteur;
        }
        public void ModifierPositionCible(Vector3 positionCible)
        {
            PositionCible = positionCible;
        }
        protected override void GererCollision()
        {
            EstEnCollision = false; ;
            EstSurTerrain = false;
            Vector3 V = VecteurResultantForce;
            //int index = 0;
            foreach (SectionDeNiveau a in ListeSections)
            {
                if (HitBoxGénérale.Intersects(a.HitBoxSection))
                {
                    foreach (ICollisionable g in ObjetCollisionné[a.IndexTableau])
                    {
                        VecteurResultantForce += ((g as ICollisionable).DonnerVectorCollision(this));

                    }

                }
            }

            if (V != VecteurResultantForce)
            {
                EstEnCollision = true;
            }
        }
        protected override void GérerClavier()
        {
            if (GestionInput.EstNouvelleTouche(Keys.R))
            {
                Position = PositionInitiale;
                VecteurResultantForce = Vector3.Zero;
                Vitesse = Vector3.Zero;
                Acceleration = Vector3.Zero;
                VarPosition = Position;
                CreerHitbox();
            }
        }
        public void VerifierSiMort()
        {
            if (VarPosition.Y < -20 || DISTANCE_MAX < Vector3.Distance(VarPosition, PositionCible))
            {
                EstVivant = false;
            }         
        }
        public override void Draw(GameTime gameTime)
        {
            if(EstVivant)
            base.Draw(gameTime);
        }

    }
}