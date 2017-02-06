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
   public class TuileTextureeAnime : TuileTexturée
    {
        public const float INTERVALLE_ANIMATION_STANDARD=1f/20;

        Vector2 DescriptionImage { get; set; }
        protected Vector2 Delta { get; set; }
        int CompteurX { get; set; }
        int CompteurY { get; set; }
        float IntervalleAnimation { get; set; }
        float TempsDepuisDerniereAnimation { get; set; }



        public TuileTextureeAnime(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile,Vector2 descriptionImage, float intervalleMAJ)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,étendue,nomTextureTuile,intervalleMAJ)
        {
            DescriptionImage = descriptionImage;
            
            
        }

 
        public override void Initialize()
        {
            

            base.Initialize();
            Delta =new Vector2( 1f / DescriptionImage.X, 1f/DescriptionImage.Y);
            TempsDepuisDerniereAnimation = 0;
            CompteurX = 0;
            CompteurY = 0;
            IntervalleAnimation = INTERVALLE_ANIMATION_STANDARD;
           
        }


        public override void Update(GameTime gameTime)
        {

            TempsDepuisDerniereAnimation += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(TempsDepuisDerniereAnimation>=IntervalleAnimation)
            {
                AnimerImage();
                TempsDepuisDerniereAnimation = 0;

            }
            base.Update(gameTime);
        }
        protected virtual void AnimerImage()
        {
            CompteurX++;

           
            if(!(CompteurX<DescriptionImage.X))
            {
                CompteurX= 0;
                CompteurY++;
                if(!(CompteurY<DescriptionImage.Y))
                {
                    CompteurY = 0;
                }
            }
            CréerTableauPointsTexture();
            InitialiserSommets();
        }

        protected override void CréerTableauPointsTexture()
        {
            // 0 1
            PtsTexture[0, 0] = new Vector2(Delta.X * CompteurX, Delta.Y * (CompteurY+1));

            // 1  1
            PtsTexture[1, 0] = new Vector2(Delta.X*(CompteurX+1), Delta.Y * (CompteurY+1));

            // 0  0
            PtsTexture[0, 1] = new Vector2(Delta.X * CompteurX, Delta.Y * CompteurY);

            // 1 0
            PtsTexture[1, 1] = new Vector2(Delta.X * (CompteurX + 1), Delta.Y * CompteurY);

        }

    }
}
