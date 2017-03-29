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
        const float HAUTEUR_HACHE = 3f;

        SpriteBatch spriteBatch;
        public const float INTERVALLE_STANDARD = 1f / 60;
        public const float INTERVALLE_CALCUL_FPS = 1f;

        Cam�raSubjective Cam�raJeu { get; set; }
        Cam�raAutomate Cam�raJeuAutomate{ get; set; }
        InputManager GestionInput;
        GraphicsDeviceManager P�riph�riqueGraphique { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnairesDeModele { get; set; }
        RessourcesManager<Song> GestionnaireDeMusiques { get; set; }
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }
        Soldat[,] Soldats { get; set; }

        GraphicsDeviceManager graphics;
        ContentManager content;

        Song ChansonJeu { get; set; }
        SoundEffect SonJeu { get; set; }

        public Atelier()
        {

            P�riph�riqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            P�riph�riqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
          
            content = new ContentManager(Services);

           // P�riph�riqueGraphique.PreferredBackBufferWidth = 1920;
           // P�riph�riqueGraphique.PreferredBackBufferHeight = 1080;
          //  P�riph�riqueGraphique.PreferMultiSampling = false;
           // P�riph�riqueGraphique.IsFullScreen = true;
        }

  
        protected override void Initialize()
        {
            //staf path hauteur=0.5f(y) de base, (z)0.7x 2(x)

            const float DELTA_X = 3.3f;
            Vector3 positionDragon = new Vector3(0, 0, 3);
            Vector3 positionCam�ra = new Vector3(0, 15, 15);
            Vector3 positionTuileDragon = positionDragon + Vector3.Right * (DELTA_X * 2);

            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnairesDeModele = new RessourcesManager<Model>(this, "Modeles");
            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            Cam�raJeu = new Cam�raSubjective(this, positionCam�ra, positionDragon, Vector3.Up, INTERVALLE_STANDARD);
           // Cam�raJeuAutomate = new Cam�raAutomate(this, positionCam�ra, positionDragon, Vector3.Up, INTERVALLE_STANDARD);
            Soldats = new Soldat[3,3];

            //Components.Add(Cam�raJeu);
            Components.Add(Cam�raJeu);
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(Random), new Random());

            Services.AddService(typeof(RessourcesManager<SoundEffect>), new RessourcesManager<SoundEffect>(this, "Sounds"));
            GestionnaireDeSons = new RessourcesManager<SoundEffect>(this, "Sounds");
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Song>), new RessourcesManager<Song>(this, "Chansons"));
            GestionnaireDeMusiques = new RessourcesManager<Song>(this, "Chansons");
            ChansonJeu = GestionnaireDeMusiques.Find("Starboy");
         //   MediaPlayer.Play(ChansonJeu);

            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnairesDeModele);
            //Services.AddService(typeof(Cam�ra), Cam�raJeu);
            Services.AddService(typeof(Cam�ra), Cam�raJeu);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));

           
         //   Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(0, 0, 0), INTERVALLE_STANDARD, "stefpath"));
            SectionRepos test = new SectionRepos(this, new Vector3(0, 0, 0), "A");


            List<PrimitiveDeBase>[] ObjetCollisionn� = new List<PrimitiveDeBase>[1];
            ObjetCollisionn�[0] = test.GetListeCollisions();
            //for (int i = 0; i < 1000; i++)
            //{
            //    Components.Add(new Soldat(this, 0.7F, Vector3.Zero, new Vector3(0, 5, 5), new Vector2(1, 2), "LoupGarou", string.Empty, new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //}

             Components.Add(new Arm�e(this, 100, new Vector3(0, 2, 12), INTERVALLE_STANDARD, ObjetCollisionn�));


            Components.Add(new Afficheur3D(this));

            //?????
            Components.Add(new TuileTextur�e(this, 100F, Vector3.Zero, Vector3.Zero, new Vector2(1, 1), "FeuFollet", 1f / 60));


            base.Initialize();
 
        }

        private void G�rerClavier()
        {
            if (GestionInput.EstEnfonc�e(Keys.Escape))
            {
                Exit();
            }
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

   

     
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            G�rerClavier();

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
