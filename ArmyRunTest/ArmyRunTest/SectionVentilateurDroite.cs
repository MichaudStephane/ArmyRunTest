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
    public class SectionVentilateurDroite : SectionDeNiveau
    {
        Ventilateur Ventilateur1 { get; set; }
        H�liceVentilateur H�lice { get; set; }

        public SectionVentilateurDroite(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        { }

        protected override void AjouterAuComponents()
        {
            Jeu.Components.Add(Ventilateur1);
            ObjetCollisionables.Add(Ventilateur1);
            Jeu.Components.Add(H�lice);

            base.AjouterAuComponents();
        }

        protected override void Cr�erSection()
        {
            base.Cr�erSection();

            Vector3 min = new Vector3(PositionInitiale.X - HitBoxBase.X, PositionInitiale.Y - HitBoxBase.Y, PositionInitiale.Z - 1.25f * HitBoxBase.Z);
            Vector3 max = new Vector3(PositionInitiale.X + HitBoxBase.X, PositionInitiale.Y + 2 * HitBoxBase.Y, PositionInitiale.Z - 0.25f * HitBoxBase.Z);

            Ventilateur1 = new Ventilateur(Jeu, HOMOTH�TIE_INITIALE_TERRAIN/100, Vector3.Zero, new Vector3(PositionInitiale.X + HitBoxBase.X, PositionInitiale.Y + 4, PositionInitiale.Z - HitBoxBase.Z), INTERVAL_MAJ, "stefpath", min, max,"Droite");
            H�lice = new H�liceVentilateur(Jeu, HOMOTH�TIE_INITIALE_TERRAIN/2, Vector3.Zero, new Vector3(PositionInitiale.X + HitBoxBase.X, PositionInitiale.Y + 4.5f, PositionInitiale.Z - HitBoxBase.Z), INTERVAL_MAJ, "h�lice");

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