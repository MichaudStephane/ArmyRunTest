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
    public class CaméraAutomate : Caméra
    {
        const float NORMALE = (9.8f);
        const float FROTTEMENT = 0.75F * NORMALE * INTERVALLE_CALCUL_PHYSIQUE * 5;
        const float INTERVALLE_CALCUL_PHYSIQUE = 1f / 60;

        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float VITESSE_INITIALE_ROTATION = 5f;
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;
        bool Avance { get; set; }
        bool Present { get; set; }
        bool Ancient { get; set; }
        BoundingSphere HitBoxArmée { get; set;}
        Vector3 Direction { get; set; }
        Vector3 Latéral { get; set; }
        float VitesseTranslation { get; set; }
        float VitesseRotation { get; set; }
        Vector3 Vitesse { get; set; }
        Vector3 Force { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }
        
        bool estEnZoom;
        bool EstEnZoom
        {
            get { return estEnZoom; }
            set
            {
                float ratioAffichage = Game.GraphicsDevice.Viewport.AspectRatio;
                estEnZoom = value;
                if (estEnZoom)
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF / 2, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
                else
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
            }
        }

        public CaméraAutomate(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
           : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
            EstEnZoom = false;
        }

        public override void Initialize()
        {
            Vitesse = Vector3.Zero;
            Force = Vector3.Zero;

            HitBoxArmée = new BoundingSphere();
            VitesseRotation = VITESSE_INITIALE_ROTATION;
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;

          
        }

        protected override void CréerPointDeVue()
        {
            // Méthode appelée s'il est nécessaire de recalculer la matrice de vue.
            // Calcul et normalisation de certains vecteurs
            // (à compléter)
            // a checker
            Direction = Vector3.Normalize(Direction);
            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
            GénérerFrustum();
        }

        protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
        {
            // À la construction, initialisation des propriétés Position, Cible et OrientationVerticale,
            // ainsi que le calcul des vecteur Direction, Latéral et le recalcul du vecteur OrientationVerticale
            // permettant de calculer la matrice de vue de la caméra subjective
            // (à compléter)
            Position = position;
            OrientationVerticale = orientation;
            Cible = cible;
            //   Direction = new Vector3(Cible.X - Position.X, Cible.Y - Position.Y, Cible.Z - Position.Z);
            // a checker
            Direction = Vector3.Normalize(Vector3.Subtract(Cible, Position));
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            OrientationVerticale = Vector3.Normalize(Vector3.Cross(Latéral, Direction));
            //Création de la matrice de vue (point de vue)
            CréerPointDeVue();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            GestionClavier();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                
                    DéterminerVitesse();
              
                   Position = new Vector3(Position.X + IntervalleMAJ * Vitesse.X, Position.Y + IntervalleMAJ * Vitesse.Y, Position.Z + IntervalleMAJ * Vitesse.Z);
                
                    CréerPointDeVue();
                
                TempsÉcouléDepuisMAJ = 0;
            }
             GameWindow a = Game.Window;
            //a.Title =
            //    " Position: [" + Math.Round(Position.X, 2) + "   " + Math.Round(Position.Y, 2) + "   " + Math.Round(Position.Z, 2) + "]" + "       CentreArmé: [ " + HitBoxArmée.Center.Z;
            a.Title =
                " Cible: [" + Cible.ToString() + "]" + "       CentreArmé: [ " + HitBoxArmée.Center.ToString();
            //base.Update(gameTime);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        private void GérerAccélération()
        {
            int valAccélération = (GérerTouche(Keys.Subtract) + GérerTouche(Keys.OemMinus)) - (GérerTouche(Keys.Add) + GérerTouche(Keys.OemPlus));
            if (valAccélération != 0)
            {
                IntervalleMAJ += ACCÉLÉRATION * valAccélération;
                IntervalleMAJ = MathHelper.Max(INTERVALLE_MAJ_STANDARD, IntervalleMAJ);
            }
        }
        public void DéplacerCaméra(Vector3 déplacement, Vector3 cible)
        {
            Position = new Vector3(Position.X + déplacement.X, Position.Y + déplacement.Y, Position.Z + déplacement.Z);
            //Position = Vector3.Normalize(Position);
            //Cible = new Vector3(Cible.X + déplacement.X, Cible.Y + déplacement.Y  ,Cible.Z+ déplacement.Z);
            Cible = new Vector3(cible.X, cible.Y ,cible.Z);
            //Cible = Vector3.Normalize(Cible);

        }
        public void SetPosCaméra(Vector3 pos)
        {
            Position = pos;
        }

        private void GérerDéplacement()
        {
            Vector3 nouvellePosition = Position;
            float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
            float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

            // Calcul du déplacement avant arrière
            nouvellePosition = Vector3.Add(nouvellePosition, Vector3.Multiply(Direction, déplacementDirection));
            // Calcul du déplacement latéral
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            nouvellePosition = Vector3.Subtract(nouvellePosition, Vector3.Multiply(Latéral, déplacementLatéral));

            // À compléter
            Position = nouvellePosition;
        }


        private void GestionClavier()
        {
            if (GestionInput.EstNouvelleTouche(Keys.Z))
            {
                EstEnZoom = !EstEnZoom;
            }
        }
         
        public void DonnerBoundingSphere(BoundingSphere a)
        {
            HitBoxArmée = a;
        }
        void DéterminerVitesse()
        {
           // Ancient = Avance;
           //// Vector3 direction = new Vector3(0, 0, -1);
           // Vector3 direction = Direction;
           // direction.Normalize();
           // direction = new Vector3(direction.X,  direction.Y, direction.Z);

           // float h = HitBoxArmée.Radius * (float)Math.Sqrt((1f / Math.Pow(Math.Tan(AngleOuvertureObjectif / 2), 2)));
           // Vector3 PositionAAtteindre = HitBoxArmée.Center - direction* (h+HitBoxArmée.Radius);
            if (HitBoxArmée.Center.Z < 0)
            {
                int a = 1;
            }
            if (Frustum.Contains(HitBoxArmée) == ContainmentType.Contains)
            {
                Force += 100 * IntervalleMAJ * (new Vector3(0, 0, -1));

            }
            else
            {
                Force -= 100 * IntervalleMAJ * (new Vector3(0, 0, -1));
            }


            if (Vitesse.Z != 0)
            {
                Force -= new Vector3(0, 0, FROTTEMENT * (Vitesse.Z / Math.Abs(Vitesse.Z)));
            }


            Force = Force - IntervalleMAJ * (1f / Force.Length()) * Force.LengthSquared() * Force;
            Force = new Vector3(Force.X, Math.Max(Force.Y, 0), Force.Z);

            Vitesse = Force;
        //    if (HitBoxArmée.Radius!=0)
        //    {
        //        int a = 1;
        //    }


        //    Position = new Vector3(Position.X, 7, PositionAAtteindre.Z);
        //    //Position = new Vector3(Position.X, 7, Math.Max((Position.Z + (-Direction * DISTANCE_PLAN_RAPPROCHÉ).Z) , PositionAAtteindre.Z));
        }
    }
}
