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
    public class SectionMobileHorizontale : SectionDeNiveau
    {

        public SectionMobileHorizontale(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        { }


        protected override void Cr�erSection()
        {
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTH�TIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainMobileSin(Jeu, HOMOTH�TIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X,PositionInitiale.Y, PositionInitiale.Z - HitBoxBase.Z), 1 / 60f, "stefpath", "Droite", 1 / 60f, 3));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTH�TIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z -HitBoxBase.Z* 2), INTERVAL_MAJ, "stefpath"));

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
        protected override void Cr�erHitboxSection()
        {
            HitBoxSection = new BoundingSphere(new Vector3(PositionInitiale.X + TAILLE_TERRAIN_X, PositionInitiale.Y, PositionInitiale.Z - LongueurNiveau / 4f), LongueurNiveau/ 2f +4);

        }
    }
}
