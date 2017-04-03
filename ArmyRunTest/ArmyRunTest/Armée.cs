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
        CaméraAutomate Caméra { get; set; }
        Vector2 Espacement = new Vector2(1, 1);
        Vector3 AnciennePosition { get; set; }
        Vector3 IntervalPosition { get; set; }


        public Armée(Game game, int nombreSoldats, Vector3 posFlag, float intervalleMAJ)
        : base(game)
        {
            AnciennePosition = posFlag;
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            PosFlag = posFlag;
            Positions = new Vector3[5, 5]; // pour l'instant dépend du nbSoldats
            Armés = new Soldat[5, 5];
            test = true;
            Caméra = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;

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
            Vector3 Pos = new Vector3(Armés[0, 0].Position.X, Armés[0, 0].Position.Y, Armés[0, 0].Position.Z);

            if (AnciennePosition != Pos)
            {
                IntervalPosition = Pos - AnciennePosition;
                AnciennePosition = Pos;
            }
                if (TempsÉcoulé >= IntervalleMAJ)
            {
                OptimiserPosition();
                
                TempsÉcoulé = 0;
            }
            //ModifierPositionCaméra();
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
            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {
                    Armés[i,j] = new Soldat(Game,0.7F,Vector3.Zero,Positions[i,j],new Vector2(1,2),"LoupGarou",string.Empty, new Vector2(4, 4), new Vector2(4, 4), 1f / 30);
                    Game.Components.Add(Armés[i, j]);
                }
            }
        }
        //void ModifierPositionCaméra()
        //{
        //    Vector3 pos = new Vector3(Armés[0, 0].Position.X, Armés[0, 0].Position.Y, Armés[0, 0].Position.Z);
        //    Caméra.DéplacerCaméra(IntervalPosition);
        //}
    }
}