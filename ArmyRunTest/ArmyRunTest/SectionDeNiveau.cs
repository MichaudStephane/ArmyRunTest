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
    public class SectionDeNiveau : Microsoft.Xna.Framework.GameComponent
    {
        protected List<ObjetBase> ObjetCollisionables { get; set; }
        protected Game Jeu { get; set;}
        protected List<ObjetBase> PartiesDeSections { get; set; }
        protected Vector3 PositionInitiale { get; set; }
        public SectionDeNiveau(Game jeu,Vector3 positionInitiale ,string nomSection)
            : base(jeu)
        {
            PositionInitiale = positionInitiale;
            Jeu = jeu;
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
