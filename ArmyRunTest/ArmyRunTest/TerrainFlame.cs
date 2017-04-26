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
  
    public class TerrainFlame : TerrainDeBase
    {
        const int INTERVALLE_FLAMME = 2;
        bool Droite { get; set; }
        float TempsÉcouléDepuisMAJFlamme { get; set; }
        BoundingBox HitboxFlameGauche { get; set; }
        BoundingBox HitboxFlameDroite { get; set; }
        TuileTextureeAnime FlammeGauche { get; set; }
        TuileTextureeAnime FlammeDroite { get; set; }
        public TerrainFlame(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModele)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,intervalleMAJ,nomModele)
        {
            FlammeGauche = new TuileTextureeAnime(Game, 1F, Vector3.Zero, Position - new Vector3(1,0,0),new Vector2(1,1), "FeuFollet", new Vector2(20, 1), 1f / 60);
            FlammeDroite = new TuileTextureeAnime(Game, 1F, Vector3.Zero, Position + new Vector3(1, 0, 0), new Vector2(1, 1), "FeuFollet", new Vector2(20, 1), 1f / 60);
            jeu.Components.Add(FlammeGauche);
            jeu.Components.Add(FlammeDroite);
            FlammeGauche.Visible = false;

            TempsÉcouléDepuisMAJFlamme = 0;
            Droite = true;
        }

    
        public override void Initialize()
        {
          

            base.Initialize();
            CréerHitBoxFlame();
        }

   
        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJFlamme += (float)gameTime.TotalGameTime.TotalSeconds;

            if(TempsÉcouléDepuisMAJFlamme>INTERVALLE_FLAMME)
            {
                ModifierFlamme();
                TempsÉcouléDepuisMAJFlamme = 0;
            }


            base.Update(gameTime);
        }
        void CréerHitBoxFlame()
        {
            Vector3 diff = HitBoxGénérale.Max - HitBoxGénérale.Min;

            HitboxFlameGauche = new BoundingBox(HitBoxGénérale.Min, HitBoxGénérale.Min + new Vector3(0.5f * diff.X, 2*diff.Y, 0.5f * diff.Z));
            HitboxFlameDroite = new BoundingBox(HitBoxGénérale.Min + new Vector3(0.5f * diff.X, 0, 0.5f * diff.Z),new Vector3(HitBoxGénérale.Max.X,HitBoxGénérale.Max.Y+diff.Y, HitBoxGénérale.Max.Z)); ;

        }
        void ModifierFlamme()
        {
            Droite = !Droite;

            if(!Droite)
            {
                FlammeGauche.Visible = true;
                FlammeDroite.Visible = false;
            }
            else
            {
                FlammeGauche.Visible = false;
                FlammeDroite.Visible = true;
            }
        }
        public override Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a)
        {
            Vector3 flamme = Vector3.Zero;
            if(!Droite)
            if((a as Soldat).HitBoxGénérale.Intersects(HitboxFlameGauche))
                {
                    flamme = new Vector3(0, 600, 0);
                }

            if(Droite)
            {
                if ((a as Soldat).HitBoxGénérale.Intersects(HitboxFlameDroite))
                {
                    flamme = new Vector3(0, 600, 0);
                }
            }


            return base.DonnerVectorCollision(a)+flamme;

        }
    }
}
