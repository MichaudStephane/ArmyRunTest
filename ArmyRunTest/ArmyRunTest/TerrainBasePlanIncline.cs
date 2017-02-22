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
    public class TerrainBasePlanIncline: PrimitiveDeBaseAnimée
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
        Random NombreAléatoire { get; set; }


        public TerrainBasePlanIncline(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,Color couleur, float intervalleMAJ)
            :base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,intervalleMAJ)
        {
            Origine = positionInitiale;
            Couleur = couleur;
            NouveauJeuRasterizerState = new RasterizerState();
            NouveauJeuRasterizerState.CullMode = CullMode.None;
        }

        public override void Initialize()
        {
            double nombre = CréerNombreAléatoire();
            if(nombre == 0)
            {
                nombre = CréerNombreAléatoire();
            }
            Dimension = CalculerDimension(nombre);
            Sommets1 = new VertexPositionColor[6];
            Sommets2 = new VertexPositionColor[6];
            InitialiserSommets();
            InitialiserHitBoxPlanIncliné();
            base.Initialize();
        }

       void InitialiserHitBoxPlanIncliné()
        {
        }

        double CréerNombreAléatoire()
        {
            NombreAléatoire = new Random();
            return NombreAléatoire.NextDouble();
        }

        Vector3 CalculerDimension(double val)
        {
            return new Vector3(1,(float)val+Origine.Y,HAUTEUR);
        }

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
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

        void InitialiserParamètresEffetDeBase()
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
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
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
