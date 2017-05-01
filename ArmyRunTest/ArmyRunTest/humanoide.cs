using System.Threading.Tasks;
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
   public abstract class Humanoide:TuileTextureeAnime
    {
        const int NB_SOLDAT_DIFFÉRENTS = 5;
       Random GénérateurAléatoire { get; set; }
       string NomImage2 { get; set; }
       Vector2 DescriptionImage2 { get; set; }
       Texture2D Image2 { get; set; }
       VertexPositionTexture[] Sommets2 { get; set; }
       protected Vector2[,] PtsTexture2 { get; set; }
       protected Vector3[,] PtsSommets2 { get; set; }
        Vector2 DeltaHumanoide { get; set; }
        Vector2 DescriptionHumanoide { get; set; }
        int VersionSoldat { get; set; }


        public Humanoide(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomImageDos,string nomImageFace, Vector2 descriptionImageDos,Vector2 DescriptionImageDevant, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, nomImageDos, descriptionImageDos, intervalleMAJ) 
         {
             NomImage2 = nomImageFace;
             DescriptionImage2 = DescriptionImageDevant;
            GénérateurAléatoire = new Random();
             VersionSoldat = GénérateurAléatoire.Next(0, NB_SOLDAT_DIFFÉRENTS);

             DeltaImage = new Vector2(1f / DescriptionImage.X, 1f / DescriptionImage.Y);
             DeltaHumanoide = new Vector2(1f / NB_SOLDAT_DIFFÉRENTS, 1);
            DescriptionHumanoide = new Vector2(DescriptionImage.X / 5, DescriptionImage.Y);
            
        }
        protected virtual void GererCollision() { }
        protected override void CréerTableauPointsTexture()
        {
            float c = VersionSoldat * DeltaHumanoide.X;

            PtsTexture[0, 0] = new Vector2(1f / 20 * CompteurX+c, 1);
            PtsTexture[1, 0] = new Vector2(1f/20*(CompteurX+1)+c, 1);
            PtsTexture[0, 1] = new Vector2(1f / 20 * CompteurX+c, 0);
            PtsTexture[1, 1] = new Vector2(1f/20*(CompteurX+1)+c, 0);


            //for (int j = 0; j < 2; ++j)
            //{
            //    for (int i = 0; i < 2; ++i)
            //    {
            //        PtsTexture[i, j] = new Vector2(DeltaHumanoide.X * VersionSoldat + DeltaImage.X*(CompteurX+i), DeltaHumanoide.Y * j);
            //    }
            //}
        }
        protected override void AnimerImage()
        {
            CompteurX++;


            if (!(CompteurX < DescriptionHumanoide.X))
            {
                CompteurX = 0;
                CompteurY++;
                if (!(CompteurY < DescriptionHumanoide.Y))
                {
                    CompteurY = 0;
                }
            }
            CréerTableauPointsTexture();
            InitialiserSommets();
        }


    }
}
