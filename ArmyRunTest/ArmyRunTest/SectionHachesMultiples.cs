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

        List<HachePendule> ListeHaches { get; set; }
        int NbHaches { get; set; }

        public SectionHachesMultiples(Game jeu, Vector3 positionInitiale, int nbHaches, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        {
            NbHaches = nbHaches;
            ListeHaches = new List<HachePendule>();
        }

        protected override void AjouterAuComponents()
        {
            base.AjouterAuComponents();
            foreach(HachePendule b in ListeHaches)
            {
                Jeu.Components.Add(b);
                ObjetCollisionables.Add(b);
            }
        }

        protected override void CréerSection()
        {
            base.CréerSection();
            float distance = 3;
            float angle = 0;
            for(int i =0;i< NbHaches;++i)
            {
                ListeHaches.Add(new HachePendule(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y + 3.5f, PositionInitiale.Z - 2*HitBoxBase.Z -distance*i), INTERVAL_MAJ, "StefAxe", angle));
                angle += MathHelper.PiOver2;
            }

        }

        protected override void CréerHitboxSection()
        {
            HitBoxSection = new BoundingSphere(new Vector3(PositionInitiale.X + TAILLE_TERRAIN_X, PositionInitiale.Y, PositionInitiale.Z - LongueurNiveau / 4f), LongueurNiveau);

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