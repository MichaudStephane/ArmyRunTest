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
        const int LARGEUR_RECTANGLE = 120;
        const int HAUTEUR_RECTANGLE = 70;

        Vector2 Dimension { get; set; }
        string Texte { get; set; }
        Rectangle RectangleAffichage { get; set; }
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

        public Boutton(Game jeu, string texte, Rectangle rectangleAffichage, Color couleur, string nomImageAvant, string nomImgaeAprès)
            : base(jeu)
        {
            Texte = texte;
            Couleur = couleur;
            RectangleAffichage = rectangleAffichage;
            NomImageAvant = nomImageAvant;
            NomImageAprès = nomImgaeAprès;
        }

        public override void Initialize()
        {
            base.Initialize();
            ChangerDeCouleur = false;
            Dimension = Font.MeasureString(Texte);
            Position = new Vector2(RectangleAffichage.X + (RectangleAffichage.Width / 2) - Dimension.X / 2, RectangleAffichage.Y + (RectangleAffichage.Height / 2) - Dimension.Y/2);
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
            return RectangleAffichage;
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            if (ChangerDeCouleur)
            {
                GestionSprite.Draw(ArrièreBouttonAvecSouris, RectangleAffichage, Color.White);
            }
            else
            {
                GestionSprite.Draw(ArrièreBouttonSansSouris, RectangleAffichage, Color.Gray);
            }
            GestionSprite.DrawString(Font, Texte, Position, Couleur);

            GestionSprite.End();
            base.Draw(gameTime);
        }

        public void ChangerDeCouleurTexture()
        {
            ChangerDeCouleur = !ChangerDeCouleur;
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
