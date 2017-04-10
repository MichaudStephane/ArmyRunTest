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
    public class ArrièrePlan : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch GestionSprites { get; set; }
        string NomImage { get; set; }
        Rectangle ZoneAffichage { get; set; }
        Texture2D ImageDeFond { get; set; }
        public ArrièrePlan(Game jeu, string nomImage)
        : base(jeu)
        {
            NomImage = nomImage;
        }
        public override void Initialize()
        {
            ZoneAffichage = new Rectangle(0, 0,
                        Game.Window.ClientBounds.Width,
                            Game.Window.ClientBounds.Height);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            ImageDeFond = Game.Content.Load<Texture2D>("Textures/" + NomImage);
        }
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(ImageDeFond, ZoneAffichage, Color.White);
            GestionSprites.End();
        }
    }
}