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
        static public Vector3 TAILLE_HITBOX_STANDARD = new Vector3(1, 0.555f, 0.724f);

        Ventilateur Ventilateur1 { get; set; }
        Vector3 HitBoxTerrain = TAILLE_HITBOX_STANDARD * HOMOTHÉTIE_INITIALE_TERRAIN;
        HéliceVentilateur Hélice { get; set; }
        List<TerrainDeBase> ListeTerrains { get; set; }

        public SectionVentilateur(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        {
            ListeTerrains = new List<TerrainDeBase>();
            CréerSection();
            AjouterAuComponents();
        }

        private void AjouterAuComponents()
        {
            Jeu.Components.Add(Ventilateur1);
            ObjetCollisionables.Add(Ventilateur1);
            Jeu.Components.Add(Hélice);

            foreach(TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
        }

        private void CréerSection()
        {
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN * 2), INTERVAL_MAJ, "stefpath"));

            Vector3 p = ListeTerrains[1].PositionInitiale;

            Vector3 min = new Vector3(PositionInitiale.X - HitBoxTerrain.X,PositionInitiale.Y - HitBoxTerrain.Y,PositionInitiale.Z - 1.25f*HitBoxTerrain.Z);
            Vector3 max = new Vector3(PositionInitiale.X + HitBoxTerrain.X,PositionInitiale.Y + 2*HitBoxTerrain.Y, PositionInitiale.Z - 0.25f*HitBoxTerrain.Z);

            Ventilateur1 = new Ventilateur(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X - TAILLE_TERRAIN_X*HOMOTHÉTIE_INITIALE_TERRAIN, PositionInitiale.Y + 4, PositionInitiale.Z - TAILLE_TERRAIN_Z* HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath",min,max);
            Hélice = new HéliceVentilateur(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X - TAILLE_TERRAIN_X * HOMOTHÉTIE_INITIALE_TERRAIN, PositionInitiale.Y + 4.5f, PositionInitiale.Z - TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath");

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