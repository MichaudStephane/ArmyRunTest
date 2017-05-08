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
        CaméraAutomate CaméraJeuAutomate{ get; set; }
        InputManager GestionInput;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
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
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
          
            content = new ContentManager(Services);
            PériphériqueGraphique.PreferredBackBufferWidth = 1920;
            PériphériqueGraphique.PreferredBackBufferHeight = 1080;
            PériphériqueGraphique.PreferMultiSampling = false;
            PériphériqueGraphique.IsFullScreen = true;
        }

  
        protected override void Initialize()
        {
            //staf path hauteur=0.5f(y) de base, (z)0.7x 2(x)
            Compteur = 0;
            const float DELTA_X = 3.3f;
            Vector3 postiontemp = new Vector3(0, 0, 2.5f);
            Vector3 positionCaméra = new Vector3(0, 5, -10);
            Vector3 positionTuileDragon = postiontemp + Vector3.Right * (DELTA_X * 2);

            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnairesDeModele = new RessourcesManager<Model>(this, "Modeles");
            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            // CaméraJeu = new CaméraSubjective(this, positionCaméra, positionDragon, Vector3.Up, INTERVALLE_STANDARD);
            CaméraJeuAutomate = new CaméraAutomate(this, postiontemp, positionCaméra, Vector3.Up, INTERVALLE_STANDARD);
            Soldats = new Soldat[3,3];

            //Components.Add(CaméraJeu);
           
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(Random), new Random());

            Services.AddService(typeof(RessourcesManager<SoundEffect>), new RessourcesManager<SoundEffect>(this, "Sounds"));
            GestionnaireDeSons = new RessourcesManager<SoundEffect>(this, "Sounds");
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Song>), new RessourcesManager<Song>(this, "Chansons"));
           
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnairesDeModele);
            //Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(Caméra), CaméraJeuAutomate);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
           
            Components.Add(new ArrièrePlan(this, "fond ecran"));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Red, INTERVALLE_CALCUL_FPS));

            Components.Add(new Menu(this));

    


            base.Initialize();
 
        }

        void NettoyerListeComponents()
        {
            for (int i = Components.Count - 1; i >= 0; --i)
            {
                if (Components[i] is IDestructible && ((IDestructible)Components[i]).ADétruire)
                {
                    Components.RemoveAt(i);
                }
            }
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
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
