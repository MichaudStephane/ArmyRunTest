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

    public class Atelier : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;
        public const float INTERVALLE_STANDARD = 1f / 60;
        public const float INTERVALLE_CALCUL_FPS = 1f;

        CaméraSubjective CaméraJeu { get; set; }
        InputManager GestionInput;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnairesDeModele { get; set; }

        public Atelier()
        {

            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;


        }

  
        protected override void Initialize()
        {
            //staf path hauteur=0.5f(y) de base, (z)0.7x 2(x)


            const float DELTA_X = 3.3f;
            Vector3 positionDragon = new Vector3(0, 0, 3);
            Vector3 positionCaméra = new Vector3(0, 0, 15);
            Vector3 positionTuileDragon = positionDragon + Vector3.Right * (DELTA_X * 2);

            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnairesDeModele = new RessourcesManager<Model>(this, "Modeles");
            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            CaméraJeu = new CaméraSubjective(this, positionCaméra, positionDragon, Vector3.Up, INTERVALLE_STANDARD);
            Components.Add(new Afficheur3D(this));
            Components.Add(CaméraJeu);
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(Random), new Random());


            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnairesDeModele);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
            Components.Add(new ObjetBase(this, 10, new Vector3(0, 0, 0), new Vector3(0, 15, 0), INTERVALLE_STANDARD, "StefAxe"));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));
      
            Components.Add(new ObjetBase(this, 1, new Vector3(0, 0, 0), new Vector3(0, 0, 1), INTERVALLE_STANDARD, "stefpath"));

            Components.Add(new ObjetBase(this, 1, new Vector3(0, 0, 0), new Vector3(0, 0, 1.7f), INTERVALLE_STANDARD, "stefpath"));

            Components.Add(new TuileTextureeAnime(this, 1, Vector3.Zero, new Vector3(2, 0, 0), new Vector2(1, 0.5f), "LoupGarou", new Vector2(4, 4), 1f / 60));
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);






            base.Initialize();
        }

  
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

   
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

     
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

  
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
