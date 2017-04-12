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
        const int Distance_Avec_Terrain_X = 4;
        const int Distance_Avec_Terrain_Y = 2;
        const int Grandeur_HitBox_Z = 7;
        const int Distance_Avec_min_Z = 3;
        BoundingBox HitBoxGénérale { get; set; }
       /// string NomModèleSupport { get; set; }
        public Ventilateur(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModelHélice)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,intervalleMAJ,nomModelHélice)
        {
            PlacerVentilateur();
            //  NomModèleSupport = nomModèleSupport;

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

            CréerHitboxGénérale();
        }
        
        void CréerHitboxGénérale()
        {
            Vector3 min = new Vector3(PositionInitiale.X + Distance_Avec_Terrain_X -1, PositionInitiale.Y - Distance_Avec_Terrain_Y*5, PositionInitiale.Z - Distance_Avec_min_Z);
            Vector3 max = new Vector3(PositionInitiale.X + Distance_Avec_Terrain_X +10 , PositionInitiale.Y + Distance_Avec_Terrain_Y *5, PositionInitiale.Z + Distance_Avec_min_Z);
            HitBoxGénérale = new BoundingBox(min, max);
        }
        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {

            Vector3 v = Vector3.Zero;

            if ((a as Soldat).HitBoxGénérale.Intersects(HitBoxGénérale))
            {
                v = new Vector3(30, 0, 0);
            }
            return v;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    }
}
