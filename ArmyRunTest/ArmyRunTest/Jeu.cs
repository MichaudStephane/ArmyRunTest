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
    class Jeu : Microsoft.Xna.Framework.GameComponent
    {
        const int SECTION_NIVEAU_TUTORIEL = 15;
        const int NOMBRE_SOLDATS_TUTORIEL = 25;
        const float INTERVAL_MAJ_MOYEN = 1 / 60f;
        Armée Armées { get; set; }
        Niveau _Niveau { get; set; }
        CaméraAutomate CaméraJeu { get; set; }
        int NombreSectionsNiveau { get; set; }
        Vector3 PositionInitialeNiveau { get; set; }
        int NombreSoldats { get; set; }
        float IntervalleMaj { get; set; }
        Song ChansonJeu { get; set; }
        RessourcesManager<Song> GestionnaireDeMusiques { get; set; }
        int NombreSoldatsVivant { get; set; }
        bool JouerMusique { get; set; }


        public Jeu(Game jeu, int nombreSectionsNiveau, Vector3 positionInitialeNiveau, int nombreSoldats, float intervalleMaj)
            : base(jeu)
        {
            NombreSectionsNiveau = nombreSectionsNiveau;
            PositionInitialeNiveau = positionInitialeNiveau;
            NombreSoldats = nombreSoldats;
            IntervalleMaj = intervalleMaj;
        }
        public Jeu(Game jeu)
            : base(jeu)
        {
            NombreSectionsNiveau = SECTION_NIVEAU_TUTORIEL;
            PositionInitialeNiveau = new Vector3(0, 0, 0);
            NombreSoldats = NOMBRE_SOLDATS_TUTORIEL;
            IntervalleMaj = INTERVAL_MAJ_MOYEN;
        }
        
        public override void Initialize()
        {
            base.Initialize();

            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
            Game.Components.Add(CaméraJeu);
            JouerMusique = true;


            _Niveau = new Niveau(Game, NombreSectionsNiveau, PositionInitialeNiveau);
            Armées = new Armée(Game, NombreSoldats, PositionInitialeNiveau + new Vector3(0, 2, -30), IntervalleMaj, _Niveau.GetTableauListObjetCollisionables(), _Niveau.GetListSectionNiveau());
            Game.Components.Add(Armées);
            GestionnaireDeMusiques = Game.Services.GetService(typeof(RessourcesManager<Song>)) as RessourcesManager<Song>;
            ChansonJeu = GestionnaireDeMusiques.Find("Starboy");

            MediaPlayer.Play(ChansonJeu);
        }

        public void ChangerDeNiveau()
        {
            for (int i = 0; i < Armées.Armés.GetLength(0); i++)
            {

                for (int j = 0; j < Armées.Armés.GetLength(1); j++)
                {

                    if (Armées.Armés[i, j] != null)
                    {
                        if (Armées.Armés[i, j].EstVivant)
                        {
                            if (Armées.Armés[i, j].Position.Z <= _Niveau.Position.Z)
                            {
                                ++NombreSoldatsVivant;
                                Armées.Armés[i, j].EstVivant = false;
                            }
                        }
                    }
                }
            }
            _Niveau.DétruireNiveau();
        }

        public override void Update(GameTime gameTime)
        {
            ChangerDeNiveau();
            base.Update(gameTime);
        }

        public void FaireJouerMusique()
        {
            JouerMusique = !JouerMusique;
            if (JouerMusique)
            {
                MediaPlayer.Resume();
            }
            else
            {
                MediaPlayer.Pause();
            }
        }
    }
}

