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
    public abstract class SectionDeNiveau : Microsoft.Xna.Framework.GameComponent,IDeletable
    {
        protected float LONGUEUR_SECTION_NORMALE = HitBoxBase.Z * 3f;
        protected const int NB_REPOS = 3;
        protected const float TAILLE_TERRAIN_X = 1f;
        protected const int HOMOTHÉTIE_INITIALE_TERRAIN = 15;
        protected const float INTERVAL_MAJ = 1 / 60F;

        static public Vector3 HitBoxBase = TerrainDeBase.TAILLE_HITBOX_STANDARD * HOMOTHÉTIE_INITIALE_TERRAIN;
        protected Vector3 ROTATION_INITIALE = new Vector3(0, 0, 0);
        private Game game;

        protected List<TerrainDeBase> ListeTerrains { get; set; }

        public List<PrimitiveDeBase> ObjetCollisionables { get; set; }
        protected Game Jeu { get; set;}
        public Vector3 PositionInitiale { get; private set; }

        public virtual float LongueurNiveau
        {
            get { return LONGUEUR_SECTION_NORMALE; }
        }
        public BoundingSphere HitBoxSection { get;protected set; }
        protected float TailleSectionNiveau { get; set; }

        public int IndexTableau { get;private set; }

        public SectionDeNiveau(Game jeu,Vector3 positionInitiale, int indexTableau)
            : base(jeu)
        {
            ListeTerrains = new List<TerrainDeBase>();
            IndexTableau = indexTableau;
            PositionInitiale = positionInitiale;
            Jeu = jeu;
            ObjetCollisionables = new List<PrimitiveDeBase>();

        }
        protected virtual void AjouterAuComponents()
        {
            foreach (TerrainDeBase a in ListeTerrains)
            {
                Jeu.Components.Add(a);
                ObjetCollisionables.Add(a);
            }
        }

        protected virtual void CréerSection()
        {
            for (int i = 0; i < NB_REPOS; ++i)
            {
                ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - HitBoxBase.Z * i), INTERVAL_MAJ, "stefpath"));
            }

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            CréerSection();
            AjouterAuComponents();
            CréerHitboxSection();

        }

        protected virtual void CréerHitboxSection()
        {
            HitBoxSection = new BoundingSphere(new Vector3(PositionInitiale.X , PositionInitiale.Y, PositionInitiale.Z - LongueurNiveau/4f),LongueurNiveau /2f +3);
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

        public List<PrimitiveDeBase> GetListeCollisions()
        {
            return ObjetCollisionables;
        }
    }
}
