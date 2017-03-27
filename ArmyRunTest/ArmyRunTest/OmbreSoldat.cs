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
    public class OmbreSoldat : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Ray LazerCollision { get; set; }
        ObjetBase[] ListeDobjetACollisioner { get; set; }
        Vector3 PositionDeDépart { get; set; }
        public OmbreSoldat(Game game,Vector3 Position,ObjetBase[] listeDobjetACollisioner)
            : base(game)
        {
            ListeDobjetACollisioner = listeDobjetACollisioner;
        }

      
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
