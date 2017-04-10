﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class TuileTexturée : Tuile
    {

        protected RessourcesManager<Texture2D> gestionnaireDeTextures { get; set; }
        protected Texture2D textureTuile { get; private set; }
        protected VertexPositionTexture[] Sommets { get; set; }
        protected Vector2[,] PtsTexture { get; set; }
        protected string NomTextureTuile { get; set; }
        protected BlendState GestionAlpha { get; set; }

        public TuileTexturée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, string nomTextureTuile, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, intervalleMAJ)
        {
            NomTextureTuile = nomTextureTuile;
        }

        protected override void CréerTableauSommets()
        {
            PtsTexture = new Vector2[2, 2];
            Sommets = new VertexPositionTexture[NbSommets];
            CréerTableauPointsTexture();
        }

        protected virtual void CréerTableauPointsTexture()
        {
            PtsTexture[0, 0] = new Vector2(0, 1);
            PtsTexture[1, 0] = new Vector2(1, 1);
            PtsTexture[0, 1] = new Vector2(0, 0);
            PtsTexture[1, 1] = new Vector2(1, 0);
        }

        protected virtual void InitialiserSommets() // Est appelée par base.Initialize()
        {
            int NoSommet = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[i, j]);
                    Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[i, j + 1]);
                }
            }
        }

        protected override void LoadContent()
        {

            gestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            textureTuile = gestionnaireDeTextures.Find(NomTextureTuile);
            base.LoadContent();
        }

        protected override void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = textureTuile;
            GestionAlpha = BlendState.AlphaBlend;
          
        }

        public override void Draw(GameTime gameTime)
        {
            DepthStencilState dss = new DepthStencilState();
            DepthStencilState ancien = GraphicsDevice.DepthStencilState;
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;

            BlendState oldBlendState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = GestionAlpha;
            base.Draw(gameTime);
            GraphicsDevice.BlendState = oldBlendState;

            GraphicsDevice.DepthStencilState = ancien;
        }

        protected override void DessinerTriangleStrip()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
        }
        public virtual void ModifierPosition(Vector3 a)
        {
            Position=a;
            CalculerMatriceMonde();
        }
    }
}
