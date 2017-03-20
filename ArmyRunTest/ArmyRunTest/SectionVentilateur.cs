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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SectionVentilateur : SectionDeNiveau
    {
        const float TAILLE_TERRAIN_Z = 0.785F;
        const int HOMOTHÉTIE_INITIALE_TERRAIN = 10;
        const int HOMOTHÉTIE_INITIALE = 1;
        Vector3 ROTATION_INITIALE = new Vector3(0, 0, 0);
        const float INTERVAL_MAJ = 1 / 60F;

        Ventilateur Ventilateur1 { get; set; }
        HéliceVentilateur Hélice { get; set; }
        List<TerrainDeBase> ListeTerrains { get; set; }

        public SectionVentilateur(Game jeu, Vector3 positionInitiale, string nomSection)
            : base(jeu, positionInitiale, nomSection)
        {
            ListeTerrains = new List<TerrainDeBase>();
            CréerSection();
            AjouterAuComponents();
        }

        private void AjouterAuComponents()
        {
            Jeu.Components.Add(Ventilateur1);
            Jeu.Components.Add(Hélice);

            foreach(TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
            }
        }

        private void CréerSection()
        {
            Ventilateur1 = new Ventilateur(Jeu, HOMOTHÉTIE_INITIALE, Vector3.Zero, new Vector3(PositionInitiale.X - 7, PositionInitiale.Y + 2, PositionInitiale.Z + TAILLE_TERRAIN_Z* HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath");
            Hélice = new HéliceVentilateur(Jeu, HOMOTHÉTIE_INITIALE, Vector3.Zero, new Vector3(PositionInitiale.X - 7, PositionInitiale.Y + 2.5f, PositionInitiale.Z + TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath");

            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z + TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z + TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN * 2), INTERVAL_MAJ, "stefpath"));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}