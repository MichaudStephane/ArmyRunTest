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
    class Armée
    {
        List<Humanoide> Soldats { get; set; }
        Vector3 Position { get; set; }

        public Armée(List<Humanoide> soldats, Vector3 position)
        {
            Soldats = soldats;
            Position = position;
        }
    }
}