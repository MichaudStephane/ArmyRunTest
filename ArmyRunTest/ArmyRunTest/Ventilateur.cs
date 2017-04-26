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
    public class Ventilateur : ObjetBase, ICollisionable
    {
        const float Distance_Avec_Terrain_X = (float)(0.724*15)/2f;
        const float Distance_Avec_Terrain_Y = 2*1.5f;
        const float Grandeur_HitBox_Z = 7*1.5f;
        const float Distance_Avec_min_Z = 3*1.5f;
        BoundingBox HitBoxG�n�rale { get; set; }
        Vector3 Min { get; set; }
        Vector3 Max { get; set; }
        string C�t�Ventilateur { get; set; }
        public Ventilateur(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModelH�lice,Vector3 min,Vector3 max,string c�t�Ventilateur)
            : base(jeu,homoth�tieInitiale,rotationInitiale,positionInitiale,intervalleMAJ,nomModelH�lice)
        {
            C�t�Ventilateur = c�t�Ventilateur;
            Max = max;
            Min = min;
            PlacerVentilateur();
        }

        private void PlacerVentilateur()
        {
            
            Monde *= Matrix.CreateRotationY(MathHelper.PiOver2);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Cr�erHitboxG�n�rale();
        }
        
        void Cr�erHitboxG�n�rale()
        {
            HitBoxG�n�rale = new BoundingBox(Min, Max);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {

            Vector3 v = Vector3.Zero;

      if((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxG�n�rale))
            {
                v = new Vector3(42, 0, 0);

         if(C�t�Ventilateur == "Droite")
            {
                    v = -v;
            }
            }
            return v;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    }
}
