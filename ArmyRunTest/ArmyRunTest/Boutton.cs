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
        RessourcesManager<SpriteFont> GestionnaireFont { get; set; }
        SpriteFont Font { get; set; }
        SpriteBatch GestionSprite { get; set; }
        Color Couleur { get; set; }
        Texture2D ArrièreBouttonSansSouris { get; set; }
        String NomImageAvant { get; set; }
        Texture2D ArrièreBouttonAvecSouris { get; set; }
        String NomImageAprès { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        bool ChangerDeCouleur { get; set; }
        Rectangle DimensionRectangle { get; set; }

        public Boutton(Game jeu, string texte, Vector2 position, Color couleur, string nomImageAvant, string nomImgaeAprès)
            : base(jeu)
        {
            Texte = texte;
            Position = position;
            Couleur = couleur;
            NomImageAvant = nomImageAvant;
            NomImageAprès = nomImgaeAprès;
        }

        public override void Initialize()
        {
            base.Initialize();
            ChangerDeCouleur = false;
            Dimension = Font.MeasureString(Texte);
            DimensionRectangle = new Rectangle((int)Position.X - 20, (int)Position.Y - 20, (int)Dimension.X + 40, (int)Dimension.Y + 40);
        }

        protected override void LoadContent()
        {
            GestionSprite = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionnaireFont = new RessourcesManager<SpriteFont>(Game, "Fonts");
            Font = GestionnaireFont.Find("Arial");
            ArrièreBouttonSansSouris = GestionnaireDeTextures.Find(NomImageAvant);
            ArrièreBouttonAvecSouris = GestionnaireDeTextures.Find(NomImageAprès);
            base.LoadContent();
        }

        public Rectangle GetDimensionBoutton()
        {
            return DimensionRectangle;
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            if (ChangerDeCouleur)
            {
                GestionSprite.Draw(ArrièreBouttonAvecSouris, DimensionRectangle, Color.White);
            }
            else
            {
                GestionSprite.Draw(ArrièreBouttonSansSouris, DimensionRectangle, Color.Gray);
            }
            GestionSprite.DrawString(Font, Texte, Position, Couleur);

            GestionSprite.End();
            base.Draw(gameTime);
        }

        public bool ChangerDeCouleurTexture
        {
            get
            {
                return ChangerDeCouleur;
            }
            set
            {
                ChangerDeCouleur = value;
            }
        }
        public Color ChangerCouleurTexte
        {
            get
            {
                return Couleur;
            }
            set
            {
                Couleur = value;
            }
        }
    }
}
