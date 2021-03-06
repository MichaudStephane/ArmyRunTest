﻿using System;
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
    class Armée : Microsoft.Xna.Framework.GameComponent,IDeletable
    {
        const int MAX_DISTANCE_CAMÉRA = 20;
        const int MIN_DISTANCE_CAMÉRA = 10;
        const int INTERVALLE_VERIFICATION = 2;
        const int MARGE_BAS = 70;
        Vector2 DimensionCase = new Vector2(1.1F, 0.7F);
        List<Humanoide> Soldats { get; set; }
        Vector3[,] Positions { get; set; }
        public SoldatDeArmée[,] Armés { get; set; }
        float TempsÉcoulé { get; set; }
        float TempsÉcoulé2 { get; set; }
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
        Vector3 DifférencePositionCaméraAvecArmée { get; set; }
        protected InputManager GestionInput { get; private set; }

        Flag Flag { get; set; }
        public int NbVivants { get; private set; }
        Vector3 MoyennePosition { get; set; }
        List<SectionDeNiveau> ListeSections { get; set; }
        bool EstFormationStandard { get; set; }
        bool EstFormationLigne { get; set; }
        AfficheurNb AfficheurNbVivant { get; set; }

        public Armée(Game game, int nombreSoldats, Vector3 posFlag, float intervalleMAJ, List<PrimitiveDeBase>[] objetCollisionné, List<SectionDeNiveau> listeSections)
        : base(game)
        {
            EstFormationLigne = false;
            EstFormationStandard = true;
            ListeSections = listeSections;
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            NbVivants = NombreSoldat;
            PosFlag = posFlag;
            PosFlagInitial = PosFlag;
            CalculerFormation(); 
            Armés = new SoldatDeArmée[Positions.GetLength(0), Positions.GetLength(1)];
            test = true;
            Caméra = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
            ObjetCollisionné = objetCollisionné;
            TempsEcouleVerification = 0;
        }

        public override void Initialize()
        {
            CréerPositionsSoldats();
            
            Flag = new Flag(Game, 1F, Vector3.Zero, new Vector3(PosFlag.X,5,PosFlag.Z), new Vector2(1, 1), "FeuFollet",new Vector2(20,1),1f/60);
            Game.Components.Add(Flag);
            CréerSoldats();
            CalculerMoyennePosition();
            AnciennePosition = MoyennePosition;
            AfficheurNbVivant = new AfficheurNb(Game, Color.Red, NbVivants, new Vector2(0, Game.Window.ClientBounds.Height - MARGE_BAS), "Nombre Soldats :", INTERVALLE_STANDARD);
            Game.Components.Add(AfficheurNbVivant);

            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
            Caméra.SetPosCaméra(new Vector3(0, 9f,PosFlag.Z));
        }

        public override void Update(GameTime gameTime)
        {
            TempsÉcoulé += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcoulé2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsEcouleVerification += (float)gameTime.ElapsedGameTime.TotalSeconds;

            ReformerArmee();
            if (TempsÉcoulé >= IntervalleMAJ)
            {
                GererClavier();
                OptimiserPosition();
                if (TempsÉcoulé2 >= IntervalleMAJ)
                {
                    if (AnciennePosition != MoyennePosition)
                    {
                        DifférencePositionCaméraAvecArmée = new Vector3(0, 0, -Caméra.Position.Z +MoyennePosition.Z) /10 ;
                        AnciennePosition = MoyennePosition;
                        DéplacerCaméra();
                    }
                    
                    TempsÉcoulé2 = 0;
                }
                
                TempsÉcoulé = 0;
            }

            if (TempsEcouleVerification >= INTERVALLE_VERIFICATION)
            {
                
                if (VerifierLesMorts())
                {
                    ReformerRang();
                }
            }
            if(AfficheurNbVivant.NbAfficheur != NbVivants)
            {
                Game.Components.Remove(Game.Components.Where(x => x is AfficheurNb).ToList().Last());
                AfficheurNbVivant = new AfficheurNb(Game, Color.Red, NbVivants, new Vector2(0, Game.Window.ClientBounds.Height - MARGE_BAS), "Nombre Soldats :", INTERVALLE_STANDARD);
                Game.Components.Add(AfficheurNbVivant);
            }
           
            Flag.ModifierPosition(new Vector3(PosFlag.X, 7, PosFlag.Z));

            CalculerMoyennePosition();
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
            float largeurArmé = Positions.GetLength(1) * DimensionCase.X;
            float longeurArmé = Positions.GetLength(0) * DimensionCase.Y;

            Vector3 limGaucheHaut = new Vector3(-0.5F * largeurArmé + 0.5F * DimensionCase.X, 0, -0.5F * longeurArmé + 0.5F * DimensionCase.Y);

            for (int j = 0; j < Positions.GetLength(1); j++)
            {
                for (int i = 0; i < Positions.GetLength(0); i++)
                {

                    Positions[i, j] = new Vector3(limGaucheHaut.X + j * DimensionCase.X, 3, limGaucheHaut.Y + i * DimensionCase.Y);
                }
            }
            int A = 1;


        }

        void CréerSoldats()
        {
            int nbSoldatsCreés = 0;
            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {
                    if (nbSoldatsCreés < NombreSoldat)
                    {
                        Armés[i, j] = new SoldatDeArmée(Game, 0.5F, Vector3.Zero, PosFlag + Positions[i, j], new Vector2(2, 3), "Soldat", string.Empty, new Vector2(20, 1), new Vector2(20, 1), 1f / 60, ObjetCollisionné, ListeSections);
                        Game.Components.Add(Armés[i, j]);
                        nbSoldatsCreés++;
                    }
                }
            }

        }

        void GererClavier()
        {
                float déplacementGaucheDroite = 0;
                float déplacementAvantArrière = 0;              
                     déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);
                
             
                déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A);

                Vector3 direction;
                    direction = new Vector3(déplacementGaucheDroite*2, 0, déplacementAvantArrière);
                
                
                PosFlag = PosFlag + 0.12f * direction;

                
                 PosFlag = new Vector3(PosFlag.X, PosFlag.Y,  PosFlag.Z);
       
            if(GestionInput.EstEnfoncée(Keys.D1))
            {
                EstFormationStandard = true;
                EstFormationLigne = false;
                ReformerRang();
            }
            if (GestionInput.EstEnfoncée(Keys.D2))
            {
                EstFormationStandard = false;
                EstFormationLigne = true;
                ReformerRang();
            }


        }
        float GérerTouche(Keys k)
        {
            return GestionInput.EstEnfoncée(k) ? 1 : 0;
        }
       public bool VerifierLesMorts()
        {
            bool aReformer = false;
            int soldatCompte = 0;
            int tempNbVivants = 0;
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {
                    if (soldatCompte < NbVivants)
                    {
                        if (Armés[i, j] == null || !Armés[i, j].EstVivant)
                        {
                            aReformer = true;

                        }
                        else
                        {
                            tempNbVivants++;
                        }
                    }
                    soldatCompte++;

                }
            }
            NbVivants = tempNbVivants;
            return aReformer;
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
       public void ReformerRang()
        {
            CalculerFormation();
            RecrerArme();

            CréerPositionsSoldats();



        }
        void CalculerFormation()
        {
            if (EstFormationStandard)
            {
                if (NbVivants <= 9)
                    Positions = new Vector3[ (NbVivants / 3) + 1, 3];
                else
                {
                    if (NbVivants <= 30)
                        Positions = new Vector3[ (NbVivants / 4) + 1, 4];
                    else
                        Positions = new Vector3[(NbVivants / 5) + 1,5];
                }
            }
            if(EstFormationLigne)
            {
                if (NbVivants <= 9)
                    Positions = new Vector3[(NbVivants / 1) + 1, 1];
                else
                {
                    if (NbVivants <= 30)
                        Positions = new Vector3[(NbVivants / 2) + 1, 2];
                    else
                        Positions = new Vector3[(NbVivants / 3) + 1, 3];
                }

            }
            if(false)
            {



            }

       
        }
        public void DéplacerCaméra()
        {
            CréerHitboxCaméra();


        }
        void CalculerMoyennePosition()
        {
            Vector3 moyenne = Vector3.Zero;
            int soldatsCompte = 0;
            int temp = NbVivants;
            for (int i = 0; i < Positions.GetLength(0); i++)
            {
                for (int j = 0; j < Positions.GetLength(1); j++)
                {
                    if (soldatsCompte < NbVivants && Armés[i,j] != null)
                    {
                        if (EstDansLimiteTerrain(Armés[i, j])) 
                        {
                            moyenne = new Vector3(moyenne.X + Armés[i, j].VarPosition.X, moyenne.Y + Armés[i, j].VarPosition.Y, moyenne.Z + Armés[i, j].Position.Z);
                            soldatsCompte++;
                        }
                    }
                }
            }

            MoyennePosition = new Vector3(moyenne.X / temp, moyenne.Y / temp, (moyenne.Z / temp));
        }

        private bool EstDansLimiteTerrain(SoldatDeArmée soldatDeArmée)
        {
            bool estDansLimite = false;
            Vector3 limite = TerrainDeBase.TAILLE_HITBOX_STANDARD * 10;
         
                if(soldatDeArmée.VarPosition.Y < 25 && soldatDeArmée.VarPosition.Y > -7)
                {
                   if(Math.Abs(-soldatDeArmée.Position.X)<10)
                    estDansLimite = true;
                }
                
            
            return estDansLimite;
        }

        void RecrerArme()
        {

            SoldatDeArmée[,] temp = new SoldatDeArmée[Positions.GetLength(0), Positions.GetLength(1)];
            int soldatsAjoute = 0;
            int tempI = 0;
            int tempJ = 0;
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {

                    if (soldatsAjoute < NbVivants)
                    {
                        if (Armés[i, j] != null && Armés[i, j].EstVivant)
                        {

                            temp[tempI, tempJ] = Armés[i, j];
                            tempJ++;
                            if (tempJ == Positions.GetLength(1))
                            {
                                tempJ = 0;
                                tempI++;
                            }
                            soldatsAjoute++;
                        }
                    }

                }
            }
            Armés = temp;
        }
        void CréerHitboxCaméra()
        {
            BoundingSphere temp = new BoundingSphere();
            bool firstTime = true;
            int soldatsComptées = 0; ;
            for (int i = 0; i < Armés.GetLength(0); i++)
            {
                for (int j = 0; j < Armés.GetLength(1); j++)
                {
                    if (soldatsComptées <= NbVivants)
                    {
                        if (Armés[i, j] != null)
                        {
                            if (Armés[i, j].EstVivant)
                            {
                                if (EstDansLimiteTerrain(Armés[i, j]))
                                {
                                    if(firstTime)
                                     {
                                        temp = BoundingSphere.CreateFromBoundingBox(Armés[i, j].HitBoxGénérale);
                                        firstTime = false;
                                        soldatsComptées++;
                                    }
                                    temp = BoundingSphere.CreateMerged(temp, BoundingSphere.CreateFromBoundingBox(Armés[i, j].HitBoxGénérale));
                                    soldatsComptées++;
                                }
                            }
                        }
                    }
                }
            }


             Caméra.DonnerBoundingSphere(BoundingSphere.CreateMerged(temp,Flag.ViewFlag));

          
        }

    }
}