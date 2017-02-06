using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AtelierXNA
{
    abstract class Humanoide : PrimitiveDeBaseAnimée
    {
        public Humanoide(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ) { }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
