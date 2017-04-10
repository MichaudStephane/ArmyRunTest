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
        CaméraAutomate CaméraJeu { get; set; }
        int NombreSectionsNiveau { get; set; }
        Vector3 PositionInitialeNiveau { get; set; }
        int NombreSoldats { get; set; }
        float IntervalleMaj { get; set; }
        Song ChansonJeu { get; set; }
        RessourcesManager<Song> GestionnaireDeMusiques { get; set; }


        public Jeu(Game jeu, int nombreSectionsNiveau, Vector3 positionInitialeNiveau, int nombreSoldats, float intervalleMaj)
            :base(jeu)
        {
            NombreSectionsNiveau = nombreSectionsNiveau;
            PositionInitialeNiveau = positionInitialeNiveau;
            NombreSoldats = nombreSoldats;
            IntervalleMaj = intervalleMaj;
        }
        public override void Initialize()
        {
            base.Initialize();

            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
            Game.Components.Add(CaméraJeu);

            Niveau niveau = new Niveau(Game, NombreSectionsNiveau, PositionInitialeNiveau);
            Game.Components.Add(new Armée(Game, NombreSoldats, new Vector3(0, 2, PositionInitialeNiveau.Z-50), IntervalleMaj, niveau.GetTableauListObjetCollisionables(),niveau.GetListSectionNiveau()));
            GestionnaireDeMusiques = Game.Services.GetService(typeof(RessourcesManager<Song>)) as RessourcesManager<Song>;
            ChansonJeu = GestionnaireDeMusiques.Find("Starboy");
            //MediaPlayer.Play(ChansonJeu);
        }
    }
}
