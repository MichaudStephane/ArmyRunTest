using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
    class AfficheurTexte : Microsoft.Xna.Framework.DrawableGameComponent, IDeletable
    {
        const int MARGE_BAS = 50;
        const int MARGE_GAUCHE = 15;
        const float AUCUNE_ROTATION = 0f;
        const float AUCUNE_HOMOTHÉTIE = 1f;

        float IntervalleMAJ { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        SpriteFont Font { get; set; }
        Color CouleurTexte { get; set; }
        Vector2 Position { get; set; }
        string Texte { get; set; }

        public AfficheurTexte(Game game, Color couleurTexte, Vector2 position, string texte, float intervalleMAJ)
         : base(game)
        {
            CouleurTexte = couleurTexte;
            IntervalleMAJ = intervalleMAJ;
            Position = position;
            Texte = texte;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GestionSprites = new SpriteBatch(Game.GraphicsDevice);
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = GestionnaireDeFonts.Find("GrosArial");
            Position = new Vector2(Position.X - Font.MeasureString(Texte).X / 2, Position.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.DrawString(Font, Texte, Position, CouleurTexte);
            GestionSprites.End();
        }
    }
}