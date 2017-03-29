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
        const int INTERVALLE_VERIFICATION = 2;
        Vector2 DimensionCase = new Vector2(2, 2);
        List<Humanoide> Soldats { get; set; }
        Vector3[,] Positions { get; set; }
        SoldatDeArmée[,] Armés { get; set; }
        float TempsÉcoulé { get; set; }
        float TempsEcouleVerification { get; set; }
        float IntervalleMAJ { get; set; }
        int NombreSoldat { get; set; }
        Vector3 PosFlag { get; set; }
        Vector3 PosFlagInitial { get; set; }
        public float INTERVALLE_STANDARD { get; private set; }
        bool test { get; set; }
        List<PrimitiveDeBase>[] ObjetCollisionné { get; set; }
        CaméraAutomate Caméra { get; set; }
        Vector2 Espacement = new Vector2(1, 1);
        Vector3 AnciennePosition { get; set; }
        Vector3 IntervalPosition { get; set; }
        protected InputManager GestionInput { get; private set; }

        SoldatDeArmée Flag { get; set; }
        public int NbVivants { get; private set; }



        public Armée(Game game, int nombreSoldats, Vector3 posFlag, float intervalleMAJ, List<PrimitiveDeBase>[] objetCollisionné)
        : base(game)
        {
            AnciennePosition = posFlag;
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            NbVivants = NombreSoldat;
            PosFlag = posFlag;
            PosFlagInitial = PosFlag;
            Positions = new Vector3[(int)Math.Ceiling(Math.Sqrt(NombreSoldat)), (int)Math.Ceiling(Math.Sqrt(NombreSoldat))]; // pour l'instant dépend du nbSoldats
            Armés = new SoldatDeArmée[(int)Math.Ceiling(Math.Sqrt(NombreSoldat)), (int)Math.Ceiling(Math.Sqrt(NombreSoldat))];
            test = true;
            Caméra = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
            ObjetCollisionné = objetCollisionné;
            TempsEcouleVerification = 0;
           

        }
        public override void Initialize()
        {
            CréerPositionsSoldats();
         //   Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile,Vector2 descriptionImage, float intervalleMAJ)
            Flag = new SoldatDeArmée(Game, 0.7F, Vector3.Zero, PosFlag, new Vector2(1, 2), "FeuFollet", string.Empty, new Vector2(20, 1), new Vector2(20, 1), 1f / 30, ObjetCollisionné);
            Game.Components.Add(Flag);
            CréerSoldats();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {

           
            
            TempsÉcoulé += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsEcouleVerification+=(float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 Pos = new Vector3(Armés[0, 0].Position.X, Armés[0, 0].Position.Y, Armés[0, 0].Position.Z);
            
            if (AnciennePosition != Pos)
            {
                IntervalPosition = Pos - AnciennePosition;
                AnciennePosition = Pos;
            }
             if (TempsÉcoulé >= IntervalleMAJ)
            {
                GererClavier();
                OptimiserPosition();
                ReformerArmee();
                TempsÉcoulé = 0;
            }
            if(TempsEcouleVerification>=INTERVALLE_VERIFICATION)
            {
                if(VerifierLesMorts())
                {
                    ReformerRang();
                }
            }
             Flag.ModifierPosition(new Vector3(PosFlag.X,7,PosFlag.Z));
            //ModifierPositionCaméra();
            GameWindow a =Game.Window;
           
             
        }

        void OptimiserPosition()
        {
            bool EstPair = NbSoldatPair();
            bool EstCarré = NbSoldatCarré();
        }
        void ReformerArmee()
        {
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {
                    if (Armés[i, j] != null)
                    {
                        if (new Vector3(Armés[i, j].VarPosition.X, 5, Armés[i, j].VarPosition.Z) != Positions[i, j])
                        {

                            Armés[i, j].ModifierPositionCible(PosFlag + Positions[i, j]);
                        }
                    }
                }
            }
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
                    Positions[i, j] = new Vector3((- DimensionCase.X / 2 * (Positions.GetLength(0)/2 ) + DimensionCase.X / 2 * i), 5, - DimensionCase.Y / 2 * (Positions.GetLength(1)/2 ) + DimensionCase.Y / 2 * j);
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
                    Armés[i, j] = new SoldatDeArmée(Game, 0.7F, Vector3.Zero, Positions[i, j] + new Vector3(PosFlag.X, 0, PosFlag.Z), new Vector2(1, 2), "LoupGarou", string.Empty, new Vector2(4, 4), new Vector2(4, 4), 1f / 30, ObjetCollisionné);
                    Game.Components.Add(Armés[i, j]);
                    soldatscréees++;
                }           
            }
           

        }

        void GererClavier()
        {
            if (!GestionInput.EstEnfoncée(Keys.LeftShift))
            {
                float déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A); //à inverser au besoin
                float déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);

                Vector3 direction = new Vector3(déplacementGaucheDroite, 0, déplacementAvantArrière);

                PosFlag = PosFlag + 0.1f * direction;


                //Pour les tests
                if (GestionInput.EstNouvelleTouche(Keys.R))
                {
                    PosFlag = PosFlagInitial;
                    ToutDetruire();
                    CréerPositionsSoldats();
                    CréerSoldats();
                }
            }

           
        }
        float GérerTouche(Keys k)
        {
            return GestionInput.EstEnfoncée(k) ? 1 : 0;
        }
        bool VerifierLesMorts()
        {
            NbVivants = 0;
            bool doitReformer = false;
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {
                   if(Armés[i,j]!=null &&!Armés[i,j].EstVivant)
                   {
                     //  Game.Components.Remove(Armés[i, j]);
                       doitReformer = true;
                   }
                   else
                   {
                       NbVivants++;
                   }
                }
            }
            return doitReformer;
        }
        void ToutDetruire()
        {
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {                   
                     Game.Components.Remove(Armés[i, j]); 
                                    
                }
            }
            Armés = new SoldatDeArmée[(int)Math.Ceiling(Math.Sqrt(NombreSoldat)), (int)Math.Ceiling(Math.Sqrt(NombreSoldat))];
            NbVivants = NombreSoldat;
        }
        void ReformerRang()
        {
            SoldatDeArmée[,] temp =new SoldatDeArmée[(int)Math.Ceiling(Math.Sqrt(NbVivants)), (int)Math.Ceiling(Math.Sqrt(NbVivants))];

            int tempA=0;
            int tempB=0;

            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {
                    if(Armés[i,j]!=null&&Armés[i,j].EstVivant)
                    {
                        temp[tempA, tempB] = Armés[i, j];
                        tempA++;

                        if(tempA==temp.GetLength(0))
                        {
                            tempA = 0;
                            tempB++;
                        }
                    }

                }
            }
            Armés = temp;

            NombreSoldat = NbVivants;
            CréerPositionsSoldats();




        }


    }
}