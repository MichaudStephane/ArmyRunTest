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
   public abstract class Humanoide:TuileTextureeAnime
    {
       string NomImage2 { get; set; }
       Vector2 DescriptionImage2 { get; set; }
       Texture2D Image2 { get; set; }
       VertexPositionTexture[] Sommets2 { get; set; }
       protected Vector2[,] PtsTexture2 { get; set; }
       protected Vector3[,] PtsSommets2 { get; set; }

         public Humanoide(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImageDos,string nomImageFace, Vector2 descriptionImageDos,Vector2 DescriptionImageDevant, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos, descriptionImageDos, intervalleMAJ) 
         {
             NomImage2 = nomImageFace;
             DescriptionImage2 = DescriptionImageDevant;
         }
        protected virtual void GererCollision() { }
        

    }
}
