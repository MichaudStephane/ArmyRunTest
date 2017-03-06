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
        Vector3[,] Positions { get; set; }
        float TempsÉcoulé { get; set; }
        float IntervalleMAJ { get; set; }
        int NombreSoldat { get; set; }

        public Armée(Game game, List<Humanoide> soldats, int nombreSoldats, float intervalleMAJ)
        :base(game)
        {
            Soldats = soldats;
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            Positions = new Vector3[6,6]; // pour l'instant
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
        
    }
}