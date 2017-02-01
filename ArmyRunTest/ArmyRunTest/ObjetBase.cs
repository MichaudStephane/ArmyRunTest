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

    public class ObjetBase : PrimitiveDeBaseAnimée
    {
        float Distance;
        RessourcesManager<Model> GestionnairesDeModele { get; set; }
        Vector3 Dimension { get; set; }
        string NomModele { get; set; }
        Model Objet3D { get; set; }
        protected Matrix[] TransformationsModèle { get; private set; }
        protected BasicEffect EffetDeBase { get; private set; }
        
   


        public ObjetBase(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, string nomModel)
            : base(jeu,homothétieInitiale,rotationInitiale,positionInitiale,intervalleMAJ)
        {
            NomModele = nomModel;
            Distance = 50;
        }

   
        public override void Initialize()
        {  

            base.Initialize();
            InitialiserEffetDeBase();
        }
        protected void InitialiserEffetDeBase()
        {
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            

        }
        protected override void LoadContent()
        {
            base.LoadContent();
            GestionnairesDeModele = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Objet3D = GestionnairesDeModele.Find(NomModele);
            EffetDeBase = new BasicEffect(GraphicsDevice);
            TransformationsModèle = new Matrix[Objet3D.Bones.Count];
            Objet3D.CopyAbsoluteBoneTransformsTo(TransformationsModèle);
           


        }
        public override void Draw(GameTime gameTime)
        {

            if (Vector3.Distance(CaméraJeu.Position, Position)<Distance)
            {
                DepthStencilState dss = new DepthStencilState();
                DepthStencilState ancien = GraphicsDevice.DepthStencilState;
                dss.DepthBufferEnable = true;

                GraphicsDevice ancienGraphics = GraphicsDevice;

                EffetDeBase.World = GetMonde();
                EffetDeBase.View = CaméraJeu.Vue;
                EffetDeBase.Projection = CaméraJeu.Projection;

                GraphicsDevice.BlendState = BlendState.AlphaBlend;


                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                GraphicsDevice.DepthStencilState = dss;
                foreach (ModelMesh maille in Objet3D.Meshes)
                {


                    Matrix mondeLocal = TransformationsModèle[maille.ParentBone.Index] * GetMonde();
                    foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
                    {
                        BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
                        effet.EnableDefaultLighting();
                        effet.Projection = CaméraJeu.Projection;
                        effet.View = CaméraJeu.Vue;
                        effet.World = mondeLocal;
                    }
                    maille.Draw();
                }

                GraphicsDevice.BlendState = ancienGraphics.BlendState;
                GraphicsDevice.DepthStencilState = ancienGraphics.DepthStencilState;
            }

        }
      
    }
}
