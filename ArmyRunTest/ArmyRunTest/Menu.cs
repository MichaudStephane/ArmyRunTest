﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_DROITE = 500;
        const int MARGE_BAS = 140;
        const int MARGE_GAUCHE = 150;
        const int MAXIMUM_NOMBRE_SOLDAT = 30;
        const int MINIMUM_NOMBRE_SECTION = 5;
        const int LARGEUR_BOUTTON = 180;
        const int HAUTEUR_BOUTTON = 90;
        const int DISTANCE_ENTRE_BOUTTON_QUITTER = 70;
        const int AUGMENTATION_SECTION_NIVEAU = 2;
        const int AUGMENTATION_SOLDATS_NIVEAU = 5;
        const int DIMINUTION_SOLDTAS_RECOMMENCER = -2;
        const string NOM_IMAGE_AVANT = "fond écran blanc";
        const string NOM_IMAGE_APRÈS = "FondEcranGris";

        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        Vector2 AnciennePosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton DifficultéFacile { get; set; }
        Boutton DifficultéMoyenne { get; set; }
        Boutton DifficultéDifficile { get; set; }
        Boutton Tutoriel { get; set; }
        Boutton Mute { get; set; }
        Boutton Exit { get; set; }
        Vector3 PosInitialeNiveau { get; set; }
        List<Boutton> Bouttons { get; set; }
        Jeu PartieEnCours { get; set; }
        Rectangle RectangleAffichageMute { get; set; }
        int CompteurNiveau { get; set; }
        SpriteBatch GestionSprites { get; set; }
        SpriteFont Font { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        AfficheurNb Afficheur { get; set; }
        AfficheurTexte AfficheurTexte { get; set; }
        int NbSoldatsContinuer { get; set; }
        bool BouttonMute { get;set;}

        public Menu(Game jeu)
            : base(jeu)
        {  }
        /// <summary>
        /// Initialise les différents boutton de difficulté
        /// </summary>
        public override void Initialize()
        {
            string son = "icone son";
            string mute = "mute button";

            Rectangle temp = Game.Window.ClientBounds;
            RectangleAffichageMute = new Rectangle(0, temp.Height - MARGE_BAS, 60, 60);

            Bouttons = new List<Boutton>();
            DifficultéFacile = new Boutton(Game, "Facile", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS , LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS, 
                                            MAXIMUM_NOMBRE_SOLDAT, MINIMUM_NOMBRE_SECTION, INTERVALLE_MOYEN);
            DifficultéMoyenne = new Boutton(Game, "Moyen", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS*3, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS, 
                                            MAXIMUM_NOMBRE_SOLDAT,MINIMUM_NOMBRE_SECTION+10, INTERVALLE_MOYEN);
            DifficultéDifficile = new Boutton(Game, "Difficile", new Rectangle(temp.Width - MARGE_DROITE, temp.Top + MARGE_BAS*5, LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS, 
                                            MAXIMUM_NOMBRE_SOLDAT,MINIMUM_NOMBRE_SECTION+20, INTERVALLE_MOYEN);
            Mute = new Boutton(Game, " ", RectangleAffichageMute, Color.White, son, mute, 0,0, INTERVALLE_MOYEN);

            Exit = new Boutton(Game, "Quitter", new Rectangle(temp.Width / 2-LARGEUR_BOUTTON/2, temp.Height/2+DISTANCE_ENTRE_BOUTTON_QUITTER , LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS, 0, 0, INTERVALLE_MOYEN);

            Bouttons.Add(DifficultéFacile);
            Bouttons.Add(DifficultéMoyenne);
            Bouttons.Add(DifficultéDifficile);

            foreach (Boutton b in Bouttons)
            {
                Game.Components.Add(b);
            }

            PosInitialeNiveau = new Vector3(0, 0, 0);

            AnciennePosSouris = PosSouris;
            PartieEnCours = new Jeu(Game, 0, PosInitialeNiveau, 0, INTERVALLE_MOYEN);

            CompteurNiveau = 0;
            Afficheur = new AfficheurNb(Game, Color.Red, ++CompteurNiveau, new Vector2(0, 0), "Niveau :", INTERVALLE_MOYEN);
            Game.Components.Add(Afficheur);
            Vector2 PosTexte = new Vector2(Game.Window.ClientBounds.Width / 4, Game.Window.ClientBounds.Height / 4);
            AfficheurTexte = new AfficheurTexte(Game, Color.Red, PosTexte, "Army Run!", INTERVALLE_MOYEN);
            Game.Components.Add(AfficheurTexte);
            NbSoldatsContinuer = 30;
            BouttonMute = true;

            base.Initialize();
        }
        /// <summary>
        /// Load les différents services
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = new SpriteBatch(Game.GraphicsDevice);
            CaméraJeuAutomate = Game.Services.GetService(typeof(CaméraAutomate)) as CaméraAutomate;
            GestionnaireManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Font = GestionnaireDeFonts.Find("Arial");
        }
        /// <summary>
        /// À chaque tour de boucle il vérifie l'état du niveau et appelle la classe nécessaire
        /// </summary>
        /// <param name="gameTime">le temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            Game.IsMouseVisible = true;
            Point point = GestionnaireManager.GetPositionSouris();
            PosSouris = new Vector2(point.X, point.Y);

            if (PartieEnCours.EstRéussi)
            {
                InitialiserMenuContinuer();
            }

            if (PartieEnCours.EstÉchec)
            {
                Vector2 PosTexte = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 150);
                AfficheurTexte = new AfficheurTexte(Game, Color.Red, PosTexte, "Vous avez échoué.", INTERVALLE_MOYEN);
                Game.Components.Add(AfficheurTexte);
                //Boutton Recommencer = new Boutton(Game, "Recommencer Niveau", new Rectangle(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 50, 
                                                //LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, "fond écran blanc", "FondEcranGris", NbSoldatsContinuer,
                                                //PartieEnCours.GetNbSections(), INTERVALLE_MOYEN);
                //Bouttons.Add(Recommencer);
                Bouttons.Add(Exit);
                //Bouttons.Add(Réinitialiser);
                //Game.Components.Add(Réinitialiser);
                //Game.Components.Add(Recommencer);
                Game.Components.Add(Exit);
                Afficheur = new AfficheurNb(Game, Color.Red, CompteurNiveau, new Vector2(0, 0), "Niveau :", INTERVALLE_MOYEN);
                Game.Components.Add(Afficheur);
                PartieEnCours.EstÉchec = false;
            }

            if (GestionnaireManager.EstSourisActive)
            {
                GestionBouttonsDeLaListe();
                

                if (EstDansBoutton(Mute))
                {
                    if (GestionnaireManager.EstNouveauClicGauche())
                    {
                        Mute.ChangerDeCouleur();
                        PartieEnCours.FaireJouerMusique();
                    }
                }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Gère les boutons de la liste en fonction de la position et du clic de la souris 
        /// </summary>
        void GestionBouttonsDeLaListe()
        {
            for (int i = 0; i < Bouttons.Count; i++)
            {
                Boutton b = Bouttons[i];
                if (EstDansBoutton(b))
                {
                    b.ChangerDeCouleur(true);
                    if (GestionnaireManager.EstNouveauClicGauche())
                    {
                        if (Game.Components.Where(x => x is Jeu).ToList().Count != 0)
                        {
                            Game.Components.Remove(Game.Components.Where(x => x is Jeu).ToList().First());
                            Game.Services.RemoveService(typeof(Jeu));
                        }
                        if (b != Exit)
                        {
                            Game.Components.Remove(Game.Components.Where(x => x is AfficheurTexte).ToList().First());
                            b.CréerJeu();
                            if (BouttonMute)
                            {
                                Game.Components.Add(Mute);
                                BouttonMute = false;
                            }
                        }
                        else
                        {
                            Game.Exit();
                        }
                        PartieEnCours = Game.Services.GetService(typeof(Jeu)) as Jeu;

                        foreach (Boutton boutton in Bouttons)
                        {
                            Game.Components.Remove(boutton);
                            boutton.Enabled = false;
                        }
                        Bouttons.Clear();
                    }
                }
                else
                {
                    b.ChangerDeCouleur(false);
                }
            }
        }
        /// <summary>
        /// Initialise le bouton menu recommencer
        /// </summary>
        void InitialiserMenuRecommencer()
        {
            Vector2 PosTexte = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 150);
            AfficheurTexte = new AfficheurTexte(Game, Color.Red, PosTexte, "Vous avez échoué.", INTERVALLE_MOYEN);
            Game.Components.Add(AfficheurTexte);
            CalculerNbSoldats(DIMINUTION_SOLDTAS_RECOMMENCER);
            Boutton Recommencer = new Boutton(Game, "Recommencer", new Rectangle(Game.Window.ClientBounds.Width / 2 - LARGEUR_BOUTTON / 2, Game.Window.ClientBounds.Height / 2-HAUTEUR_BOUTTON/2,
                                            LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS, NbSoldatsContinuer,
                                            PartieEnCours.GetNbSections(), INTERVALLE_MOYEN);
            Bouttons.Add(Recommencer);
            Bouttons.Add(Exit);
            Game.Components.Add(Recommencer);
            Game.Components.Add(Exit);
            Afficheur = new AfficheurNb(Game, Color.Red, CompteurNiveau, new Vector2(0, 0), "Niveau :", INTERVALLE_MOYEN);
            Game.Components.Add(Afficheur);
            PartieEnCours.DéfinirÉtatJeu(PartieEnCours.EstRéussi, false);
        }
        /// <summary>
        /// initialise menu continuer
        /// </summary>
        void InitialiserMenuContinuer()
        {
            Vector2 PosTexte = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 150);
            AfficheurTexte = new AfficheurTexte(Game, Color.Red, PosTexte, "Bravo! Vous avez complété le niveau " + CompteurNiveau.ToString(), INTERVALLE_MOYEN);

            Game.Components.Add(AfficheurTexte);
            Boutton Continuer = new Boutton(Game, "Continuer", new Rectangle(Game.Window.ClientBounds.Width / 2 - LARGEUR_BOUTTON/2, Game.Window.ClientBounds.Height /2 - HAUTEUR_BOUTTON/2,
                                LARGEUR_BOUTTON, HAUTEUR_BOUTTON), Color.Blue, NOM_IMAGE_AVANT, NOM_IMAGE_APRÈS,
                                CalculerNbSoldats(AUGMENTATION_SOLDATS_NIVEAU), CalculerNbSection(), INTERVALLE_MOYEN);

            NbSoldatsContinuer = Continuer.NombreSoldats;
            Bouttons.Add(Continuer);
            Bouttons.Add(Exit);
            Game.Components.Add(Exit);
            Game.Components.Add(Continuer);
            Afficheur = new AfficheurNb(Game, Color.Red, ++CompteurNiveau, new Vector2(0, 0), "Niveau :", INTERVALLE_MOYEN);
            Game.Components.Add(Afficheur);
            PartieEnCours.DéfinirÉtatJeu(false,PartieEnCours.EstÉchec);
        }
        /// <summary>
        /// Change le nombre de soldats par niveau
        /// </summary>
        /// <param name="valeur">le nouveau nombre de soldats</param>

        int CalculerNbSoldats(int valeur)
        {
            return PartieEnCours.GetNbSoldat() + valeur;
        }
        /// <summary>
        /// Change le nombre de sections dans chaque niveau
        /// </summary>
        /// <returns>le nouveau nombre de section de niveaux</returns>
        int CalculerNbSection()
        {
            return PartieEnCours.GetNbSections() + CompteurNiveau * AUGMENTATION_SECTION_NIVEAU;
        }
        /// <summary>
        /// Détermine si la souris est dans les limites du bouton
        /// </summary>
        /// <param name="b">bouton de la liste</param>
        /// <returns>un booléen qui détermine si la souris est dans les limites du bouton</returns>
        bool EstDansBoutton(Boutton b)
        {
            bool estDansBoutton = false;
            Rectangle temp = b.GetDimensionBoutton();

            Point point = GestionnaireManager.GetPositionSouris();
            if (PosSouris.X >= temp.Left && PosSouris.X <= temp.Right)
            {
                if (PosSouris.Y >= temp.Top && PosSouris.Y <= temp.Bottom)
                {
                    estDansBoutton = true;
                }
            }
            return estDansBoutton;
        }
    }
}
