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
    public class TerrainBasePlanIncline: PrimitiveDeBaseAnim�e,ICollisionable
    {
        const float HAUTEUR = 0.785f; 
        Color Couleur { get; set; }
        VertexPositionColor[] Sommets1 { get; set; }
        VertexPositionColor[] Sommets2 { get; set; }
        float IntervalleMAJ { get; set; }
        Vector3 Origine { get; set; }
        Vector3 Dimension { get; set; }
        BasicEffect EffetDeBase { get; set; }
        RasterizerState NouveauJeuRasterizerState { get; set; }
        Random NombreAl�atoire { get; set; }
        BoundingBox HitBoxPlanInclin� { get; set; }


        public TerrainBasePlanIncline(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,Color couleur, float intervalleMAJ)
            :base(jeu,homoth�tieInitiale,rotationInitiale,positionInitiale,intervalleMAJ)
        {
            Origine = positionInitiale;
            Couleur = couleur;
            NouveauJeuRasterizerState = new RasterizerState();
            NouveauJeuRasterizerState.CullMode = CullMode.None;
        }

        public override void Initialize()
        {
            double nombre = Cr�erNombreAl�atoire();
            if(nombre == 0)
            {
                nombre = Cr�erNombreAl�atoire();
            }
            CalculerDimension(nombre);
            Sommets1 = new VertexPositionColor[6];
            Sommets2 = new VertexPositionColor[6];
            InitialiserSommets();
            InitialiserHitBoxPlanInclin�();
            base.Initialize();
        }

       void InitialiserHitBoxPlanInclin�()
        {
            List<Vector3> point = new List<Vector3>();
            point.Add(Origine); point.Add(new Vector3(Origine.X + Dimension.X, Origine.Y + Dimension.Y, Origine.Z + Dimension.Z));
            HitBoxPlanInclin� = BoundingBox.CreateFromPoints(point);
        }

        public Vector3 DonnerVectorCollision(PrimitiveDeBaseAnim�e a)
        {
            Vector3 v = Vector3.Zero;
           
            if ((a as Soldat).HitBoxG�n�rale.Intersects(HitBoxPlanInclin�))
            {
                if ((a as Soldat).HitBoxG�n�rale.Min.Y == CalculerHauteur((a as Soldat).HitBoxG�n�rale.Min.Z))
                {
                    v = new Vector3(0, -(a as Soldat).VecteurResultantForce.Y, 0);
                }
            }
            return v;
        }

        double Cr�erNombreAl�atoire()
        {
            NombreAl�atoire = new Random();
            return NombreAl�atoire.NextDouble();
        }

        void CalculerDimension(double val)
        {
            Dimension = new Vector3(1,(float)val+Origine.Y,HAUTEUR);
        }
        float CalculerHauteur(float nb)
        {
            return nb * Dimension.Y;
        }


        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParam�tresEffetDeBase();
            base.LoadContent();
        }

        void InitialiserSommets()
        {
            Sommets1[0] = new VertexPositionColor(Origine,Couleur);

            Vector3 positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y, Origine.Z);
            Sommets1[1] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X, Origine.Y - Dimension.Y, Origine.Z - Dimension.Z);
            Sommets1[2] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y - Dimension.Y, Origine.Z - Dimension.Z);
            Sommets1[3] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X, Origine.Y - Dimension.Y, Origine.Z);
            Sommets1[4] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y - Dimension.Y, Origine.Z);
            Sommets1[5] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X, Origine.Y - Dimension.Y, Origine.Z - Dimension.Z);
            Sommets2[0] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X, Origine.Y - Dimension.Y, Origine.Z);
            Sommets2[1] = new VertexPositionColor(positionSommet, Couleur);

            Sommets2[2] = new VertexPositionColor(Origine, Couleur);

            positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y - Dimension.Y, Origine.Z);
            Sommets2[3] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y, Origine.Z);
            Sommets2[4] = new VertexPositionColor(positionSommet, Couleur);

            positionSommet = new Vector3(Origine.X - Dimension.X, Origine.Y - Dimension.Y, Origine.Z - Dimension.Z);
            Sommets2[5] = new VertexPositionColor(positionSommet, Couleur);

        }

        void InitialiserParam�tresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }
        public override void Draw(GameTime gameTime)
        {
            DepthStencilState dss = new DepthStencilState();
            RasterizerState JeuRasterizerState = new RasterizerState();
            JeuRasterizerState.FillMode = Game.GraphicsDevice.RasterizerState.FillMode;
            DepthStencilState ancien = GraphicsDevice.DepthStencilState;
            dss.DepthBufferEnable = true;
            GraphicsDevice ancienGraphics = GraphicsDevice;
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = Cam�raJeu.Vue;
            EffetDeBase.Projection = Cam�raJeu.Projection;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.RasterizerState = JeuRasterizerState;
            GraphicsDevice.RasterizerState = JeuRasterizerState;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.DepthStencilState = dss;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets1, 0, 4);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets2, 0, 4);
                
            }
            GraphicsDevice.BlendState = ancienGraphics.BlendState;
            GraphicsDevice.DepthStencilState = ancienGraphics.DepthStencilState;
            GraphicsDevice.RasterizerState = JeuRasterizerState;
        }
    }
}
