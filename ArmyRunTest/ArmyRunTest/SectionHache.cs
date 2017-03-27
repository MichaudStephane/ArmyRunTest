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
    public class SectionHache : SectionDeNiveau
    {
        const float TAILLE_TERRAIN_Z = 0.785F;
        const int HOMOTHÉTIE_INITIALE_TERRAIN = 10;
        const int HOMOTHÉTIE_INITIALE = 1;
        Vector3 ROTATION_INITIALE =new Vector3(0,0,0);
        const float INTERVAL_MAJ = 1 / 60F;

        HachePendule Hache { get; set; }

        List<TerrainDeBase> ListeTerrains { get; set; }

        public SectionHache(Game jeu, Vector3 positionInitiale, string nomSection)
            : base(jeu, positionInitiale, nomSection)
        {
            ListeTerrains = new List<TerrainDeBase>();
            CréerSection();
            AjouterAuComponents();
        }

        private void AjouterAuComponents()
        {
            Jeu.Components.Add(Hache);
            ObjetCollisionables.Add(Hache);
            foreach (TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
        }

        private void CréerSection()
        {
            Hache = new HachePendule(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN,Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y + 3.5f, PositionInitiale.Z + 7),INTERVAL_MAJ,"StefAxe");
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y , PositionInitiale.Z), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z+TAILLE_TERRAIN_Z*HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z+TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN*2), INTERVAL_MAJ, "stefpath"));
       
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
