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
    public class SectionMobileMultiples : SectionDeNiveau
    {
        List<TerrainDeBase> ListeTerrains { get; set; }

        public SectionMobileMultiples(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        {
            ListeTerrains = new List<TerrainDeBase>();
            Cr�erSection();
            AjouterAuComponents();
        }

        private void AjouterAuComponents()
        {
            foreach (TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
        }

        private void Cr�erSection()
        {
            int nbrTerrainsMobilesSur2 = 6/2;
            float demiTerrain_X = (TAILLE_TERRAIN_X / 2f);
            for(int i= 0; i < nbrTerrainsMobilesSur2;++i)
            {
                ListeTerrains.Add(new TerrainMobileSin(Jeu, HOMOTH�TIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X+demiTerrain_X*i, PositionInitiale.Y, PositionInitiale.Z - (TAILLE_TERRAIN_Z * HOMOTH�TIE_INITIALE_TERRAIN* i)), 1 / 60f, "stefpath", "Droite", 1 / 60f, 2*i));
            }
            for (int i = 0; i < nbrTerrainsMobilesSur2; ++i)
            {
                ListeTerrains.Add(new TerrainMobileSin(Jeu, HOMOTH�TIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X - demiTerrain_X * i, PositionInitiale.Y, PositionInitiale.Z - (TAILLE_TERRAIN_Z * HOMOTH�TIE_INITIALE_TERRAIN * i)+ 3* TAILLE_TERRAIN_Z * HOMOTH�TIE_INITIALE_TERRAIN), 1 / 60f, "stefpath", "Gauche", 1 / 60f, 2*i));
            }
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
            HitBoxSection = new BoundingSphere(new Vector3(PositionInitiale.X + TAILLE_TERRAIN_X, PositionInitiale.Y, PositionInitiale.Z + LongueurNiveau / 4f), LongueurNiveau );

        }
    }
}
