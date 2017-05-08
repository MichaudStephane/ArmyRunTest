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
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }
        Soldat[,] Soldats { get; set; }
        GraphicsDeviceManager graphics;
        ContentManager content;

        SoundEffect SonJeu { get; set; }
        int Compteur { get; set; }

        public Atelier()
        {
            P�riph�riqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            P�riph�riqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
          
            content = new ContentManager(Services);
            P�riph�riqueGraphique.PreferredBackBufferWidth = 1920;
            P�riph�riqueGraphique.PreferredBackBufferHeight = 1080;
            P�riph�riqueGraphique.PreferMultiSampling = false;
            P�riph�riqueGraphique.IsFullScreen = true;
        }

  
        protected override void Initialize()
        {
            //staf path hauteur=0.5f(y) de base, (z)0.7x 2(x)
            Compteur = 0;
            const float DELTA_X = 3.3f;
            Vector3 postiontemp = new Vector3(0, 0, 2.5f);
            Vector3 positionCam�ra = new Vector3(0, 5, -10);
            Vector3 positionTuileDragon = postiontemp + Vector3.Right * (DELTA_X * 2);

            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnairesDeModele = new RessourcesManager<Model>(this, "Modeles");
            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            // Cam�raJeu = new Cam�raSubjective(this, positionCam�ra, positionDragon, Vector3.Up, INTERVALLE_STANDARD);
            Cam�raJeuAutomate = new Cam�raAutomate(this, postiontemp, positionCam�ra, Vector3.Up, INTERVALLE_STANDARD);
            Soldats = new Soldat[3,3];

            //Components.Add(Cam�raJeu);
           
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(Random), new Random());

            Services.AddService(typeof(RessourcesManager<SoundEffect>), new RessourcesManager<SoundEffect>(this, "Sounds"));
            GestionnaireDeSons = new RessourcesManager<SoundEffect>(this, "Sounds");
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Song>), new RessourcesManager<Song>(this, "Chansons"));
           
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnairesDeModele);
            //Services.AddService(typeof(Cam�ra), Cam�raJeu);
            Services.AddService(typeof(Cam�ra), Cam�raJeuAutomate);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
           
            Components.Add(new Arri�rePlan(this, "fond ecran"));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));

            Components.Add(new Menu(this));

    


            base.Initialize();
 
        }

        void NettoyerListeComponents()
        {
            for (int i = Components.Count - 1; i >= 0; --i)
            {
                if (Components[i] is IDestructible && ((IDestructible)Components[i]).AD�truire)
                {
                    Components.RemoveAt(i);
                }
            }
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
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
