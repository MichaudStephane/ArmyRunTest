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
    class Boutton : DrawableGameComponent
    {
        Vector2 Dimension { get; set; }
        string Texte { get; set; }
        Vector2 Position { get; set; }
        RessourcesManager<SpriteFont> GestionnaireFont {get;set;}
        SpriteFont Font { get; set; }
        SpriteBatch GestionSprite { get; set; }
        Color Couleur { get; set; }

        public Boutton(Game jeu, string texte, Vector2 position, Color couleur)
            : base(jeu)
        {
            Texte = texte;
            Position = position;
            Couleur = couleur;
        }

        public override void Initialize()
        {
            base.Initialize();
            Dimension = Font.MeasureString(Texte);
        }

        protected override void LoadContent()
        {
            GestionSprite = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireFont = new RessourcesManager<SpriteFont>(Game, "Fonts");
            Font = GestionnaireFont.Find("Arial");
            base.LoadContent();
        }

        public Rectangle GetDimensionBoutton()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimension.X, (int)Dimension.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            GestionSprite.DrawString(Font, Texte, Position, Couleur);
            GestionSprite.End();
            base.Draw(gameTime);
        }
    }
}
