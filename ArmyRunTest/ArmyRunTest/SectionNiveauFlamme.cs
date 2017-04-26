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
    public class SectionNiveauFlamme :  SectionDeNiveau
    {
        List<TerrainDeBase> ListeTerrains { get; set; }

        public SectionNiveauFlamme(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu,positionInitiale,indexTableau)
        {
            ListeTerrains = new List<TerrainDeBase>();
            CréerSection();
            AjouterAuComponents();
        }

    
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
        private void CréerSection()
        {
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainFlame(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN), INTERVAL_MAJ, "stefpath"));
            ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - TAILLE_TERRAIN_Z * HOMOTHÉTIE_INITIALE_TERRAIN * 2), INTERVAL_MAJ, "stefpath"));

        }
        private void AjouterAuComponents()
        {
            foreach (TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
        }

        public override void Update(GameTime gameTime)
        {
         

            base.Update(gameTime);
        }
    }
}
