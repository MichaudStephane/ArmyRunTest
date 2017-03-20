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
 
    public class TerrainMobileSin : TerrainDeBaseMobile 
   
    {
        float Distance { get; set; }
        int sens { get; set; }
        Vector3 DifferencePosition { get; set; }


        public TerrainMobileSin(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel, string direction, float intervalleDeplacement, float distance) 
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel,direction,intervalleDeplacement)
        {
            Distance = distance;
            DifferencePosition = Vector3.Zero;
            sens = 1;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void BougerTerrain(GameTime gameTime)
        {
            Vector3 PositionPrecedente = Position;

            Position = PositionInitiale + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * Direction * Distance;



            DifferencePosition = -PositionPrecedente + Position;
           
        }

        protected override Vector3 DonnerVectorMouvement()
        {
            return Vector3.Zero;
        }
        protected override void SuivreTerrain(Soldat a)
        {
            a.ModifierPosition(a.VarPosition + DifferencePosition);
           
        }


    }
}
