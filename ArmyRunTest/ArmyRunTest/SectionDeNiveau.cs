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
    public abstract class SectionDeNiveau : Microsoft.Xna.Framework.GameComponent
    {
        protected const float TAILLE_TERRAIN_Z = 0.724F;
        protected const float TAILLE_TERRAIN_X = 1F;

        protected const int HOMOTHÉTIE_INITIALE_TERRAIN = 10;
        protected const int HOMOTHÉTIE_INITIALE = 1;
        protected const float INTERVAL_MAJ = 1 / 60F;
        protected Vector3 ROTATION_INITIALE = new Vector3(0, 0, 0);
        public List<PrimitiveDeBase> ObjetCollisionables { get; set; }
        protected Game Jeu { get; set;}
        protected Vector3 PositionInitiale { get; set; }

        public float LongueurNiveau
        {
            get { return (GetListeCollisions().Where(x => x is TerrainDeBase).Count())*TAILLE_TERRAIN_Z*10; }
        }
        protected BoundingSphere HitBoxSection { get; set; }
        protected float TailleSectionNiveau { get; set; }


        public SectionDeNiveau(Game jeu,Vector3 positionInitiale)
            : base(jeu)
        {
            PositionInitiale = positionInitiale;
            Jeu = jeu;
            ObjetCollisionables = new List<PrimitiveDeBase>();
          //  DéterminerSection();
        }

        private void DéterminerSection()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            

            base.Initialize();
        }

        protected abstract void CréerHitboxSection();
        

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
