using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    class AfficheurNbVivant : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_BAS = 50;
        const int MARGE_GAUCHE = 15;
        const float AUCUNE_ROTATION = 0f;
        const float AUCUNE_HOMOTHÉTIE = 1f;

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        string NomImage { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        SpriteFont Font { get; set; }
        Color CouleurTexte { get; set; }
        Rectangle RectanglePosition { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        Texture2D Texture { get; set; }
        int NbVivant { get; set; }

        public AfficheurNbVivant(Game game, string nomImage, Color couleurTexte, int nbVivant, float intervalleMAJ)
         : base(game)
        {
            NomImage = nomImage;
            CouleurTexte = couleurTexte;
            IntervalleMAJ = intervalleMAJ;
            NbVivant = nbVivant;
        }

        public override void Initialize()
        {
            TempsÉcouléDepuisMAJ = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GestionSprites = new SpriteBatch(Game.GraphicsDevice);
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Texture = GestionnaireDeTextures.Find(NomImage);
            RectanglePosition = new Rectangle(0 + MARGE_GAUCHE, Game.Window.ClientBounds.Height - MARGE_BAS, Texture.Width/25, Texture.Height/25);
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = GestionnaireDeFonts.Find("Arial");
        }

        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(Texture,RectanglePosition,Color.White);
            GestionSprites.DrawString(Font, NbVivant.ToString(), new Vector2(RectanglePosition.X, RectanglePosition.Y), CouleurTexte);
            GestionSprites.End();
        }
        public void ChangerNombreVivant(int nb)
        {
            NbVivant = nb;
        }
    }
}
