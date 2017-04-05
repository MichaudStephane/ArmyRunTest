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
using System.Windows.Forms;

namespace AtelierXNA
{
   public class Menu :  Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float INTERVALLE_MOYEN = 1 / 60f;
        Vector2 PosSouris { get; set; }
        InputManager GestionnaireManager { get; set; }
        CaméraAutomate CaméraJeuAutomate { get; set; }
        Boutton Start{ get; set; }

        public Menu(Game jeu)
            : base(jeu)
        {  }

        public override void Initialize()
        {
            Start = new Boutton(Game, "Hello world", new Vector2(0, 0), Color.Blue);
            Game.Components.Add(Start);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            CaméraJeuAutomate = Game.Services.GetService(typeof(CaméraAutomate)) as CaméraAutomate;
            GestionnaireManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
           
        }
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (tempsÉcoulé >= INTERVALLE_MOYEN)
            {
                Cursor.Show();
                Point point = GestionnaireManager.GetPositionSouris();
                PosSouris = new Vector2(point.X, point.Y);
                if (EstDansBoutton())
                {
                    SectionRepos test = new SectionRepos(Game, new Vector3(0, 0, 20), 1);
                    SectionVentilateur test2 = new SectionVentilateur(Game, new Vector3(0, 0, -40), 2);
                    SectionRepos test3 = new SectionRepos(Game, new Vector3(0, 0, 0), 1);
                    SectionRepos test4 = new SectionRepos(Game, new Vector3(0, 0, -20), 1);
                    SectionHache test5 = new SectionHache(Game, new Vector3(0, 0, -60), 1);
                    SectionRepos test6 = new SectionRepos(Game, new Vector3(0, 0, 40), 1);
                    SectionRepos test7 = new SectionRepos(Game, new Vector3(0, 0, -80), 1);

                    List<PrimitiveDeBase>[] ObjetCollisionné = new List<PrimitiveDeBase>[1];
                    List<PrimitiveDeBase>[] ObjetCollisionné2 = new List<PrimitiveDeBase>[1];
                    List<PrimitiveDeBase>[] temp = new List<PrimitiveDeBase>[1];
                    temp[0] = new List<PrimitiveDeBase> { };
                    for (int i = 0; i < test.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test2.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test2.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test3.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test3.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test4.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test4.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test5.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test5.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test6.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test6.GetListeCollisions()[i]);
                    }
                    for (int i = 0; i < test7.GetListeCollisions().Count; i++)
                    {

                        temp[0].Add(test7.GetListeCollisions()[i]);
                    }

                    ObjetCollisionné[0] = test.GetListeCollisions();
                    Game.Components.Add(new Armée(Game, 50, new Vector3(0, 2, -60), Atelier.INTERVALLE_STANDARD, temp));
                    Game.Components.Add(CaméraJeuAutomate);

                    Game.Components.Add(new Afficheur3D(Game));

                    Game.Components.Add(new TuileTexturée(Game, 100F, Vector3.Zero, Vector3.Zero, new Vector2(1, 1), "FeuFollet", 1f / 60));

                }

            }
            base.Update(gameTime);
        }

        bool EstDansBoutton()
        {
            bool estDansBoutton = false;
            if (PosSouris.X >= Start.GetDimensionBoutton().Left && PosSouris.X <= Start.GetDimensionBoutton().Right && PosSouris.Y >= Start.GetDimensionBoutton().Top && PosSouris.X <= Start.GetDimensionBoutton().Bottom) 
            {
                estDansBoutton = true;
            }
            return estDansBoutton;
        }
    }
}
