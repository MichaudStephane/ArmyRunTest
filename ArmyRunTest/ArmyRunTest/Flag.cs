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
    public class Flag : TuileTextureeAnime,IDeletable
    {
        public BoundingSphere ViewFlag { get;private set; }
        public Flag(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, Vector2 descriptionImage, float intervalleMAJ)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,étendue,nomTextureTuile,descriptionImage,intervalleMAJ)
        {
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ViewFlag = new BoundingSphere(Position, 1);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ViewFlag = new BoundingSphere(new Vector3(0,Position.Y,Position.Z), 1);

            base.Update(gameTime);
        }
    }
}
