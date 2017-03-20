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
    class Armée: Microsoft.Xna.Framework.GameComponent
    {
        List<Humanoide> Soldats { get; set; }
        Vector2[,] Positions { get; set; }
        float TempsÉcoulé { get; set; }
        float IntervalleMAJ { get; set; }
        int NombreSoldat { get; set; }
        Vector2 PosFlag { get; set; }
        Vector2 Espacement = new Vector2(1, 1); 
       

        public Armée(Game game, int nombreSoldats,Vector2 posFlag, float intervalleMAJ)
        :base(game)
        {
           
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            PosFlag = posFlag;
            Positions = new Vector2[3,3]; // pour l'instant
        }


        public override void Update(GameTime gameTime)
        {
            float tempsÉcouléDepuisMAJ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcoulé += tempsÉcouléDepuisMAJ;
            if(TempsÉcoulé >= IntervalleMAJ)
            {
                OptimiserPosition();
                TempsÉcoulé = 0;
            }
        }

        public override void Initialize()
        {
            CréerPositionsSoldats();
            CréerSoldats();

            //foreach (Humanoide s in Soldats)
            //{
            //    Game.Components.Add(s as Soldat);
            //}
            base.Initialize();
        }

        void OptimiserPosition()
        {
            bool EstPair = NbSoldatPair();
            bool EstCarré = NbSoldatCarré();
        }

        bool NbSoldatPair()
        {
            return NombreSoldat % 2 == 0;
        }

        bool NbSoldatCarré()
        {
            bool EstCarré = false;
            for (int i = 0; i >= 6; i++) 
            {
                if (Math.Pow(i, 2) == NombreSoldat)
                {
                    EstCarré = true;
                }
            }
            return EstCarré;
        }

        void CréerPositionsSoldats()
        {
            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {
                    Positions[i, j] = new Vector2((((-Positions.GetLength(0) - 1) / 2) * Espacement.X + Espacement.X * i),
                                                  (((-Positions.GetLength(1) - 1) / 2) * Espacement.Y + Espacement.Y * j));
                }
            }
        }
        void CréerSoldats()
        {
            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {

                }
            }
        }
    }
}