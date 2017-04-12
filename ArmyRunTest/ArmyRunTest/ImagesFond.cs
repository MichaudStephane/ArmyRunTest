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

    public class ImagesFond
    {
        Vector3 PositionInitialeNiveau { get; set; }
        string NomTextureTuile1 {get;set;}
        string NomTextureTuile2 { get; set; }
        string NomTextureTuile3 { get; set; }
        string NomTextureTuile4 { get; set; }
        string NomTextureTuile5 { get; set; }
        float IntervalleMAJ { get; set;}
        Game Jeu { get; set; }
        public ImagesFond(Game jeu, float homothétieInitiale,Vector3 positionInitialeNiveau, string nomTextureTuile1, string nomTextureTuile2, string nomTextureTuile3, string nomTextureTuile4, string nomTextureTuile5, float intervalleMAJ)
        {
            Jeu = jeu;
            IntervalleMAJ = intervalleMAJ;
            PositionInitialeNiveau = positionInitialeNiveau;
            NomTextureTuile1 = nomTextureTuile1;
            NomTextureTuile2 = nomTextureTuile2;
            NomTextureTuile3 = nomTextureTuile3;
            NomTextureTuile4 = nomTextureTuile4;
            NomTextureTuile5 = nomTextureTuile5;

            CréerBackGround();
        }

        private void CréerBackGround()
        {
            Jeu.Components.Add(new TuileTexturée(Jeu, 100, new Vector3(0,0,0), PositionInitialeNiveau + new Vector3(0,0,-50), new Vector2(1, 1), NomTextureTuile1, IntervalleMAJ));
            //new TuileTexturée(Jeu, 100, Vector3.Zero, PositionInitialeNiveau, new Vector2(1, 1), NomTextureTuile2, IntervalleMAJ);
            //new TuileTexturée(Jeu, 100, Vector3.Zero, PositionInitialeNiveau, new Vector2(1, 1), NomTextureTuile3, IntervalleMAJ);
            //new TuileTexturée(Jeu, 100, Vector3.Zero, PositionInitialeNiveau, new Vector2(1, 1), NomTextureTuile4, IntervalleMAJ);
            //new TuileTexturée(Jeu, 100, Vector3.Zero, PositionInitialeNiveau, new Vector2(1, 1), NomTextureTuile5, IntervalleMAJ);
           
        }
    }
}
