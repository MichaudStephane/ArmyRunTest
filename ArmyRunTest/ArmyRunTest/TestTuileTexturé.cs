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
    public class TestTuileTexturé : TuileTextureeAnime
    {
        BoundingBox HitBoxTest { get; set; }
        public TestTuileTexturé(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, Vector2 descriptionImage, float intervalleMAJ)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,étendue,nomTextureTuile,descriptionImage,intervalleMAJ)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            CreerHitBox();

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {

            ChangerPosition();

            base.Update(gameTime);
        }
        void ChangerPosition()
        {
            Position = new Vector3(Position.X, Position.Y - 2, Position.Z);
            CreerHitBox();
            foreach (GameComponent g in Game.Components)
            {
                if(g is TerrainDeBase)
                {
                    if(HitBoxTest.Intersects((g as TerrainDeBase).HitBoxGénérale))
                    Position = new Vector3(Position.X, Position.Y + 2, Position.Z);
                }
            }
            CreerHitBox();
        }
        void CreerHitBox()
        {
            HitBoxTest = new BoundingBox(Position - new Vector3(Delta.X, Delta.Y, 1), Position + new Vector3(Delta.X, Delta.Y, 1));
        }
    }
}
