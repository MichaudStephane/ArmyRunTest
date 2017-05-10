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
        HachePendule Hache { get; set; }

        public SectionHache(Game jeu, Vector3 positionInitiale,int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        {}

        protected override void AjouterAuComponents()
        {
            Jeu.Components.Add(Hache);
            ObjetCollisionables.Add(Hache);
            base.AjouterAuComponents(); 
        }

        protected override void CréerSection()
        {
            Hache = new HachePendule(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN,Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y + 3.5f, PositionInitiale.Z - 7),INTERVAL_MAJ,"StefAxe",0);
            base.CréerSection();
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
