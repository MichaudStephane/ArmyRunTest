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
    public class SectionHachesMultiples : SectionDeNiveau
    {
        HachePendule Hache { get; set; }

        List<TerrainDeBase> ListeTerrains { get; set; }
        List<HachePendule> ListeHaches { get; set; }
        int NbHaches { get; set; }

        public SectionHachesMultiples(Game jeu, Vector3 positionInitiale, int nbHaches)
            : base(jeu, positionInitiale)
        {
            NbHaches = nbHaches;
            ListeTerrains = new List<TerrainDeBase>();
            ListeHaches = new List<HachePendule>();
            CréerSection();
            AjouterAuComponents();
        }

        private void AjouterAuComponents()
        {

            foreach (TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
            foreach(HachePendule b in ListeHaches)
            {
                Jeu.Components.Add(b);
                ObjetCollisionables.Add(b);
            }
            int c = 1;
        }

        private void CréerSection()
        {
            float distance = 3;
            float angle = 0;
            for(int i =0;i< NbHaches;++i)
            {
                ListeHaches.Add(new HachePendule(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y + 3.5f, PositionInitiale.Z +12 -distance*i), INTERVAL_MAJ, "StefAxe", angle));
                angle += MathHelper.PiOver2;
            }
            
            //Hache1 = new HachePendule(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y + 3.5f, PositionInitiale.Z + 7), INTERVAL_MAJ, "StefAxe", 0);
           

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
        protected override void CréerHitboxSection()
        {


        }
    }
}