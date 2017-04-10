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
        const int MAX_DISTANCE_CAMÉRA = 20;
        const int MIN_DISTANCE_CAMÉRA = 10;
        const int INTERVALLE_VERIFICATION = 2;
        Vector2 DimensionCase = new Vector2(0.7F, 0.7F);
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
        AfficheurNbVivant AfficheurNbVivant { get; set; }
        



        public Armée(Game game, int nombreSoldats, Vector3 posFlag, float intervalleMAJ, List<PrimitiveDeBase>[] objetCollisionné, List<SectionDeNiveau> listeSections)
        : base(game)
        {
            ListeSections = listeSections;
            IntervalleMAJ = intervalleMAJ;
            NombreSoldat = nombreSoldats;
            NbVivants = NombreSoldat;
            PosFlag = posFlag;
            PosFlagInitial = PosFlag;
            CalculerFormation(); // pour l'instant dépend du nbSoldats
            Armés = new SoldatDeArmée[Positions.GetLength(0), Positions.GetLength(1)];
            test = true;
            Caméra = Game.Services.GetService(typeof(Caméra)) as CaméraAutomate;
            ObjetCollisionné = objetCollisionné;
            TempsEcouleVerification = 0;
        }

        public override void Initialize()
        {
            CréerPositionsSoldats();
            
            //   Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile,Vector2 descriptionImage, float intervalleMAJ)
            Flag = new Flag(Game, 1F, Vector3.Zero, new Vector3(PosFlag.X,5,PosFlag.Z), new Vector2(1, 1), "FeuFollet",new Vector2(20,1),1f/60);
            Game.Components.Add(Flag);
            CréerSoldats();
            CalculerMoyennePosition();
            AnciennePosition = MoyennePosition;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            AfficheurNbVivant = new AfficheurNbVivant(Game, "185281", Color.Red, NbVivants, INTERVALLE_STANDARD);
            Game.Components.Add(AfficheurNbVivant);
            base.Initialize();
            Caméra.SetPosCaméra(new Vector3(0, 9f,PosFlag.Z));
        }

        public override void Update(GameTime gameTime)
        {
            TempsÉcoulé += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcoulé2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsEcouleVerification += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Vector3 Pos = new Vector3(Armés[0, 0].Position.X, Armés[0, 0].Position.Y, Armés[0, 0].Position.Z);
            AfficheurNbVivant.ChangerNombreVivant(NbVivants);
            if (TempsÉcoulé >= IntervalleMAJ)
            {
                GererClavier();
                OptimiserPosition();
                ReformerArmee();
                if (TempsÉcoulé2 >= IntervalleMAJ)
                {
                    if (AnciennePosition != MoyennePosition)
                    {
                        //IntervalPosition = MoyennePosition - AnciennePosition;
                        //AnciennePosition = MoyennePosition;
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

            Vector3 limGaucheHaut = new Vector3(-0.5F * largeurArmé + 0.5F * DimensionCase.X, 0, -0.5F * largeurArmé + 0.5F * DimensionCase.Y);

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
                        Armés[i, j] = new SoldatDeArmée(Game, 0.5F, Vector3.Zero, PosFlag + Positions[i, j], new Vector2(1, 2), "LoupGarou", string.Empty, new Vector2(4, 4), new Vector2(4, 4), 1f / 60, ObjetCollisionné, ListeSections);
                        Game.Components.Add(Armés[i, j]);
                        nbSoldatsCreés++;
                    }
                }
            }

        }

        void GererClavier()
        {
     
            
            

            if (!GestionInput.EstEnfoncée(Keys.LeftShift))
            {
                float déplacementGaucheDroite = 0;
                float déplacementAvantArrière = 0;
                
                      //à inverser au besoin
                     déplacementAvantArrière = GérerTouche(Keys.S) - GérerTouche(Keys.W);
                
             
                déplacementGaucheDroite = GérerTouche(Keys.D) - GérerTouche(Keys.A);

                Vector3 direction;


             
                    direction = new Vector3(déplacementGaucheDroite*2, 0, déplacementAvantArrière);
                
                
                PosFlag = PosFlag + 0.1f * direction;

                
               //  PosFlag = new Vector3(PosFlag.X, PosFlag.Y, Math.Max(MoyennePosition.Z - 10, PosFlag.Z));
                PosFlag = new Vector3(PosFlag.X, PosFlag.Y,  PosFlag.Z);

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
        void ReformerRang()
        {
            CalculerFormation();
            RecrerArme();

            CréerPositionsSoldats();



        }
        void CalculerFormation()
        {
            int a = 0;
            if (NbVivants <= 9)
            {
                Positions = new Vector3[3, (NbVivants / 3) + 1];
            }
            else
            {
                if (NbVivants <= 30)
                {
                    Positions = new Vector3[(NbVivants / 4) + 1, 4];
                }
                else
                {
                    Positions = new Vector3[(NbVivants / 5) + 1, 5];
                }
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
                        if (EstDansLimiteTerrain(Armés[i, j])) // fonctionne pas ???
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
                   if(Math.Abs(-soldatDeArmée.Position.X+PosFlag.X)<20)
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
  
            if(temp.Center.Z<0)
            {
                int a = 1;
            }
            Caméra.DonnerBoundingSphere(BoundingSphere.CreateMerged(temp,Flag.ViewFlag));
        }

    }
}