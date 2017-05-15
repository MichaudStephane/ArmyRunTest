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
        public const int NB_SECTION_FACILE = 10;
       
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
        int NombreSoldatArrivé { get; set; }
        Rectangle RectangleAffichageMute { get; set; }
        public bool JouerMusique { get; private set; }
        public bool EstRéussi { get; private set; }
        public bool EstÉchec { get; private set; }
        Boutton Mute { get; set; }


        public Jeu(Game jeu, int nombreSectionsNiveau, Vector3 positionInitialeNiveau, int nombreSoldats, float intervalleMaj)
            : base(jeu)
        {
            NombreSectionsNiveau = nombreSectionsNiveau;
            PositionInitialeNiveau = positionInitialeNiveau;
            NombreSoldats = nombreSoldats;
            IntervalleMaj = intervalleMaj;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            if (Game.Components.Where(x => x is CaméraAutomate).ToList().Count < 1)
            {
                CaméraJeu = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
                Game.Components.Add(CaméraJeu);
            }
            EstRéussi = false;
            EstÉchec = false;
            _Niveau = new Niveau(Game, NombreSectionsNiveau, PositionInitialeNiveau);
            Armées = new Armée(Game, NombreSoldats, PositionInitialeNiveau + new Vector3(0, 2, -30), IntervalleMaj, _Niveau.GetTableauListObjetCollisionables(), _Niveau.GetListSectionNiveau());
            Game.Components.Add(Armées);
            GestionnaireDeMusiques = Game.Services.GetService(typeof(RessourcesManager<Song>)) as RessourcesManager<Song>;
            ChansonJeu = GestionnaireDeMusiques.Find("Starboy");

            MediaPlayer.Play(ChansonJeu);
            JouerMusique = true;
            //Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute, 0, 0, INTERVAL_MAJ_MOYEN);
            //Game.Components.Add(Mute);
        }

        // Permet de changer faire le changement de niveau
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
                            Vector3 temp = _Niveau.GetListSectionNiveau().Last().PositionInitiale;
                            BoundingSphere temp2 = _Niveau.GetListSectionNiveau().Last().HitBoxSection;
                            if (Armées.Armés[i, j].Position.Z <= (temp.Z - temp2.Radius))
                            {
                                ++NombreSoldatArrivé;
                                Armées.Armés[i, j].EstVivant = false;
                                Armées.VerifierLesMorts();
                                Armées.ReformerRang();

                                if (Armées.NbVivants == 0)
                                {
                                    DétruireNiveau();


                                    if (NombreSoldatArrivé >= 1)
                                    {
                                        EstRéussi = true;
                                    }
                                }
                            }
                            else
                            {
                                Armées.Armés[i, j].VerifierSiMort();

                                Armées.VerifierLesMorts();
                                Armées.ReformerRang();

                                if (Armées.NbVivants == 0)
                                {
                                    EstÉchec = true;
                                    DétruireNiveau();
                                }
                            }
                        }
                    }
                }
            }
        }


        public void DétruireNiveau()
        {
            _Niveau.DétruireNiveau();
        }

        public int GetNbSections()
        {
            return NombreSectionsNiveau;
        }

        public override void Update(GameTime gameTime)
        {
            ChangerDeNiveau();
            base.Update(gameTime);
        }

        public void FaireJouerMusique()
        {
            if (JouerMusique)
            {
                MediaPlayer.Pause();
                JouerMusique = !JouerMusique;
            }
            else
            {
                MediaPlayer.Resume();
                JouerMusique = !JouerMusique;
            }
        }

        public int GetNbSoldat()
        {
            int nb = NombreSoldatArrivé;
            NombreSoldatArrivé = 0;
            return nb;
        }

        public void DéfinirÉtatJeu(bool estRéussi,bool estÉchec )
        {
            EstRéussi = estRéussi;
            EstÉchec = estÉchec;
         }

    }
}

