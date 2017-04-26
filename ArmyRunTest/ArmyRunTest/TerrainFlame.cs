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
        float Temps�coul�DepuisMAJFlamme { get; set; }
        BoundingBox HitboxFlameGauche { get; set; }
        BoundingBox HitboxFlameDroite { get; set; }
        TuileTextureeAnime FlammeGauche { get; set; }
        TuileTextureeAnime FlammeDroite { get; set; }
        public TerrainFlame(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModele)
            : base(jeu,homoth�tieInitiale,rotationInitiale,positionInitiale,intervalleMAJ,nomModele)
        {
            FlammeGauche = new TuileTextureeAnime(Game, 1F, Vector3.Zero, Position - new Vector3(1,0,0),new Vector2(1,1), "FeuFollet", new Vector2(20, 1), 1f / 60);
            FlammeDroite = new TuileTextureeAnime(Game, 1F, Vector3.Zero, Position + new Vector3(1, 0, 0), new Vector2(1, 1), "FeuFollet", new Vector2(20, 1), 1f / 60);
            jeu.Components.Add(FlammeGauche);
            jeu.Components.Add(FlammeDroite);
            FlammeGauche.Visible = false;

            Temps�coul�DepuisMAJFlamme = 0;
            Droite = true;
        }

    
        public override void Initialize()
        {
          

            base.Initialize();
            Cr�erHitBoxFlame();
        }

   
        public override void Update(GameTime gameTime)
        {
            Temps�coul�DepuisMAJFlamme += (float)gameTime.TotalGameTime.TotalSeconds;

            if(Temps�coul�DepuisMAJFlamme>INTERVALLE_FLAMME)
            {
                ModifierFlamme();
                Temps�coul�DepuisMAJFlamme = 0;
            }


            base.Update(gameTime);
        }
        void Cr�erHitBoxFlame()
        {
            Vector3 diff = HitBoxG�n�rale.Max - HitBoxG�n�rale.Min;

            HitboxFlameGauche = new BoundingBox(HitBoxG�n�rale.Min, HitBoxG�n�rale.Min + new Vector3(0.5f * diff.X, 2*diff.Y, 0.5f * diff.Z));
            HitboxFlameDroite = new BoundingBox(HitBoxG�n�rale.Min + new Vector3(0.5f * diff.X, 0, 0.5f * diff.Z),new Vector3(HitBoxG�n�rale.Max.X,HitBoxG�n�rale.Max.Y+diff.Y, HitBoxG�n�rale.Max.Z)); ;

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
        public override Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {
            Vector3 flamme = Vector3.Zero;
            if(!Droite)
            if((a as Soldat).HitBoxG�n�rale.Intersects(HitboxFlameGauche))
                {
                    flamme = new Vector3(0, 600, 0);
                }

            if(Droite)
            {
                if ((a as Soldat).HitBoxG�n�rale.Intersects(HitboxFlameDroite))
                {
                    flamme = new Vector3(0, 600, 0);
                }
            }


            return base.DonnerVectorCollision(a)+flamme;

        }
    }
}
