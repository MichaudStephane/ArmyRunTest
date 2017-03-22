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

        CaméraSubjective CaméraJeu { get; set; }
        InputManager GestionInput;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnairesDeModele { get; set; }
        RessourcesManager<Song> GestionnaireDeMusiques { get; set; }
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }
        Soldat[][] ObjetCollisionné { get; set; }

        GraphicsDeviceManager graphics;
        ContentManager content;

        Song ChansonJeu { get; set; }
        SoundEffect SonJeu { get; set; }

        public Atelier()
        {

            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
          
            content = new ContentManager(Services);

           // PériphériqueGraphique.PreferredBackBufferWidth = 1920;
           // PériphériqueGraphique.PreferredBackBufferHeight = 1080;
          //  PériphériqueGraphique.PreferMultiSampling = false;
           // PériphériqueGraphique.IsFullScreen = true;
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
            ObjetCollisionné = new Soldat[3][];
            Components.Add(CaméraJeu);
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(Random), new Random());

            Services.AddService(typeof(RessourcesManager<SoundEffect>), new RessourcesManager<SoundEffect>(this, "Sounds"));
            GestionnaireDeSons = new RessourcesManager<SoundEffect>(this, "Sounds");

            Services.AddService(typeof(RessourcesManager<Song>), new RessourcesManager<Song>(this, "Chansons"));
            GestionnaireDeMusiques = new RessourcesManager<Song>(this, "Chansons");
            ChansonJeu = GestionnaireDeMusiques.Find("Starboy");
            MediaPlayer.Play(ChansonJeu);

            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnairesDeModele);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));
            // Components.Add(new TerrainBasePlanIncline(this,10, new Vector3(0, 0, 0), new Vector3(0.47f,0.2f,-0.33f),Color.Black, INTERVALLE_STANDARD));
            // Components.Add(new TerrainBasePlanIncline(this, 10, new Vector3(0, 0, 0), new Vector3(0,0,0), Color.Black, INTERVALLE_STANDARD));

            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(0, 0, 0), INTERVALLE_STANDARD, "stefpath"));
            Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(-5, -3, 0), INTERVALLE_STANDARD, "stefpath"));
            //Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(0, 0, -7), INTERVALLE_STANDARD, "stefpath"));
            //Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(0, 0, -14), INTERVALLE_STANDARD, "stefpath"));
            //Components.Add(new TerrainDeBase(this, 10, new Vector3(0, 0, 0), new Vector3(-3, 3, -15), INTERVALLE_STANDARD, "stefpath"));

            Components.Add(new TerrainMobileSin(this, 10, new Vector3(0, 0, 0), new Vector3(0, 0, -8), INTERVALLE_STANDARD, "stefpath", "Haut", 1f / 60, 5));


            //Components.Add(new Ventilateur(this, 1, new Vector3(0, 0, 0), new Vector3(-7, 3, 0), INTERVALLE_STANDARD, "stefpath"));
            //Components.Add(new HéliceVentilateur(this, 1, new Vector3(0, 0, 0), new Vector3(-7, 3, 0), INTERVALLE_STANDARD, "stefpath"));

            //List<Humanoide> Soldats = new List<Humanoide>();
            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(-1, 10, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(1, 10, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(0, 10, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));

            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(1, 10, 0), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(-1, 10, 0), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Soldats.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(0, 50, 0), new Vector2(2, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Components.Add(new Armée(this,Soldats,Soldats.Count,INTERVALLE_STANDARD));
            //for (int i = 0; i < ObjetCollisionné.GetLength(0); i++)
            //{
            //    ObjetCollisionné[i] = new Soldat[0];
            //    for (int j = 0; j <= 0; j++)
            //    {
            //        ObjetCollisionné[i][j] = new Soldat(this, 1f, Vector3.Zero, new Vector3(i-1, 40, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30);
            //    }
            //}
            //Components.Add(new SoldatDeArmée(this,0,Vector3.Zero,new Vector3(0,0,0),new Vector2(1,2),"LoupGarou", "LoupGarou",new Vector2(4,4),new Vector2(4, 4),INTERVALLE_STANDARD,)
          //  Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(-1, 40, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
           // Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(1, 40, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(0, 10, -1), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));

            //Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(1, 10, 0), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(-1, 10, 0), new Vector2(1, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
            //Components.Add(new Soldat(this, 1f, Vector3.Zero, new Vector3(0, 50, 0), new Vector2(2, 2), "LoupGarou", "LoupGarou", new Vector2(4, 4), new Vector2(4, 4), 1f / 30));
           

            Components.Add(new Afficheur3D(this));
            

            base.Initialize();
        }

        private void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.Escape))
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
            GérerClavier();

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
