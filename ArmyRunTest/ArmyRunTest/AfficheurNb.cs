using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    class AfficheurNb : Microsoft.Xna.Framework.DrawableGameComponent, IDeletable
    {
        const int MARGE_BAS = 50;
        const int MARGE_GAUCHE = 15;
        const float AUCUNE_ROTATION = 0f;
        const float AUCUNE_HOMOTHÉTIE = 1f;

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        SpriteFont Font { get; set; }
        Color CouleurTexte { get; set; }
        Vector2 PositionTexte { get; set; }
        Vector2 PositionNb { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        public int NbAfficheur { get; private set; }
        Vector2 Position { get; set; }
        string Texte { get; set; }

        public AfficheurNb(Game game, Color couleurTexte, int nbAfficheur,Vector2 position, string texte, float intervalleMAJ)
         : base(game)
        {
            CouleurTexte = couleurTexte;
            IntervalleMAJ = intervalleMAJ;
            NbAfficheur = nbAfficheur;
            Position = position;
            Texte = texte;
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
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = GestionnaireDeFonts.Find("Arial");
            Vector2 temp = Font.MeasureString(Texte.ToString());
            PositionTexte = Position;
            PositionNb = new Vector2(PositionTexte.X + temp.X / 4, PositionTexte.Y + temp.Y);
        }

        //public override void Update(GameTime gameTime)
        //{
        //    float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    TempsÉcouléDepuisMAJ += tempsÉcoulé;
        //    if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
        //    {
        //        TempsÉcouléDepuisMAJ = 0;
        //    }
        //}

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            //GestionSprites.Draw(Texture,RectanglePosition,Color.White);
            GestionSprites.DrawString(Font, Texte, PositionTexte, CouleurTexte);
            GestionSprites.DrawString(Font, NbAfficheur.ToString(), PositionNb, CouleurTexte);
            GestionSprites.End();
        }
    }
}
