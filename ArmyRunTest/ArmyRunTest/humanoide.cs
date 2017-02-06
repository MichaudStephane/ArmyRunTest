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
    abstract class Humanoide:TuileTextureeAnime
    {
         public Humanoide(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImage, Vector2 descriptionImage, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImage, descriptionImage, intervalleMAJ) { }
    }
}
