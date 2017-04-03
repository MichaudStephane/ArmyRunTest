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
   
    public abstract class TerrainDeBaseMobile : TerrainDeBase
    {
        const string DROITE_NOM = "Droite";
        const string GAUCHE_NOM = "Gauche";
        const string DEVANT_NOM = "Devant";
        const string DERRIERE_NOM = "Derriere";
        const string HAUT_NOM = "Haut";
        const string BAS_NOM = "Bas";

        string ChoixUtilisateur { get; set; }
        float IntervalleDeplacement { get;set; }
       protected Vector3 Direction { get; set; }
        float TempsEcouleDepuisMAJDeplacement { get; set; }
        GameComponent[][] ObjetsACollisionner { get; set; }




        public TerrainDeBaseMobile(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel, string direction,float intervalleDeplacement) 
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ, nomModel)
        {
            ChoixUtilisateur = direction;
            IntervalleDeplacement = intervalleDeplacement;
        }

  
        public override void Initialize()
        {
         //   ChoisirDirection();
            TempsEcouleDepuisMAJDeplacement = 0;
            ChoisirDirection();
            base.Initialize();
        }

     
        public override void Update(GameTime gameTime)
        {
            TempsEcouleDepuisMAJDeplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(TempsEcouleDepuisMAJDeplacement>=IntervalleDeplacement)
            {
                BougerTerrain(gameTime);
                UpdateHitbox();
                CalculerMatriceMonde();
                TempsEcouleDepuisMAJDeplacement = 0;
            }
            base.Update(gameTime);
        }

        void ChoisirDirection()
        {
           
            Direction = VECTOR_GAUCHE;

            if(ChoixUtilisateur==DROITE_NOM)
            {
                Direction = VECTOR_DROITE;
            }
            if (ChoixUtilisateur == DEVANT_NOM)
            {
                Direction = VECTOR_DEVANT;
            }
            if (ChoixUtilisateur == DERRIERE_NOM)
            {
                Direction = VECTOR_DERRIERE;
            }
            if (ChoixUtilisateur == HAUT_NOM)
            {
                Direction = VECTOR_HAUT;
            }
            if (ChoixUtilisateur == BAS_NOM)
            {
                Direction = VECTOR_BAS;
            }
        }

       protected abstract void BougerTerrain(GameTime gameTime);


        void UpdateHitbox()
        {
            CréerHitboxGénérale();
        }
       protected abstract Vector3 DonnerVectorMouvement();
      

    }
}
