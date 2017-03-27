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
    class Armée : Microsoft.Xna.Framework.GameComponent
    {

        Vector2 DimensionCase = new Vector2(2, 2);
        List<Humanoide> Soldats { get; set; }
        Vector3[,] Positions { get; set; }
        Soldat[,] Armés { get; set; }
        float TempsÉcoulé { get; set; }
        float IntervalleMAJ { get; set; }
        int NombreSoldat { get; set; }
        Vector3 PosFlag { get; set; }
        public float INTERVALLE_STANDARD { get; private set; }
        bool test { get; set; }
        GameComponent[][] ObjetCollisionné { get; set; }

        Vector2 Espacement = new Vector2(1, 1);


        public Armée(Game game, int nombreSoldats, Vector3 posFlag, float intervalleMAJ)
        : base(game)
        {
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            PosFlag = posFlag;
            Positions = new Vector3[(int)Math.Ceiling(Math.Sqrt(NombreSoldat)), (int)Math.Ceiling(Math.Sqrt(NombreSoldat))]; // pour l'instant dépend du nbSoldats
            Armés = new Soldat[(int)Math.Ceiling(Math.Sqrt(NombreSoldat)), (int)Math.Ceiling(Math.Sqrt(NombreSoldat))];
            test = true;

        }
        public override void Initialize()
        {
        //    CréerPositionsSoldats();
          //  CréerSoldats();
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {

            if(test)
            {
                CréerPositionsSoldats();
                CréerSoldats();
                test = false;
            }
          


            float tempsÉcouléDepuisMAJ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcoulé += tempsÉcouléDepuisMAJ;
         
            if (TempsÉcoulé >= IntervalleMAJ)
            {
                OptimiserPosition();
                TempsÉcoulé = 0;
            }
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
            int nbCases = NombreSoldat + Convert.ToInt32(NbSoldatPair());


            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {
                    Positions[i, j] = new Vector3((PosFlag.X - DimensionCase.X / 2 * (Positions.GetLength(0) - 1) + DimensionCase.X / 2 * i), 5, PosFlag.Y - DimensionCase.Y / 2 * (Positions.GetLength(1) - 1) + DimensionCase.Y / 2 * j);


                }
            }



        }

        void CréerSoldats()
        {
            int soldatscréees = 0;
            for (int i = 0; i < Positions.GetLength(0) ; i++)
            {
                for (int j = 0; j < Positions.GetLength(1) && soldatscréees < NombreSoldat; j++)
                {
                    Armés[j,i] = new Soldat(Game,0.7F,Vector3.Zero,Positions[j,i],new Vector2(1,2),"LoupGarou",string.Empty, new Vector2(4, 4), new Vector2(4, 4), 1f / 30);
                    Game.Components.Add(Armés[j, i]);
                    soldatscréees++;
                }
                if(soldatscréees==NombreSoldat)
                {
                    break;
                }
            }



        }
    }
}