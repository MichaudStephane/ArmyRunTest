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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Niveau
    {
        List<PrimitiveDeBase>[] TabListObjetCollisionables { get; set; }
        const int NbrSectionsDisponibles = 5;
        List<SectionDeNiveau> ListSections { get; set; }
        Random GénérateurAléatoire { get; set; }
        int NbrSections { get; set; }
        Game Jeu { get; set; }
        Vector3 Position { get; set; }
        public Niveau(Game jeu, int nbrSections, Vector3 positionInitiale)
        {
            Jeu = jeu;
            NbrSections = nbrSections;
            TabListObjetCollisionables = new List<PrimitiveDeBase>[NbrSections];
            for(int cpt=0; cpt< NbrSections;++cpt)
            {
                TabListObjetCollisionables[cpt] = new List<PrimitiveDeBase>();
            }
            ListSections = new List<SectionDeNiveau>();
            GénérateurAléatoire = new Random();
            Position = positionInitiale;
            CréerNiveau();
        }

        private void CréerNiveau()
        {
            for (int i = 0; i < NbrSections; ++i)
            {
                int nombreAléatoire = GénérateurAléatoire.Next(0, NbrSectionsDisponibles + 1);
                if (nombreAléatoire == 0)
                {
                    SectionRepos a = new SectionRepos(Jeu, Position, i);
                    ListSections.Add(a);
                    Jeu.Components.Add(a);
                    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                    {
                        TabListObjetCollisionables[i].Add(b);
                    }
                    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);
                }
                else if (nombreAléatoire == 1)
                {
                    SectionHache a = new SectionHache(Jeu, Position, i);
                    ListSections.Add(a);
                    Jeu.Components.Add(a);
                    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                    {
                        TabListObjetCollisionables[i].Add(b);
                    }
                    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);
                }
                else if (nombreAléatoire == 2)
                {
                    SectionHachesMultiples a = new SectionHachesMultiples(Jeu, Position, 4, i);
                    ListSections.Add(a);
                    Jeu.Components.Add(a);
                    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                    {
                        TabListObjetCollisionables[i].Add(b);
                    }
                    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);
                }
                else if (nombreAléatoire == 3)
                {
                    SectionVentilateur a = new SectionVentilateur(Jeu, Position, i);
                    ListSections.Add(a);
                    Jeu.Components.Add(a);
                    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                    {
                        TabListObjetCollisionables[i].Add(b);
                    }
                    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);

                }
                else if (nombreAléatoire == 4)
                {
                    SectionMobileHorizontale a = new SectionMobileHorizontale(Jeu, Position, i);
                    ListSections.Add(a);
                    Jeu.Components.Add(a);
                    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                    {
                        TabListObjetCollisionables[i].Add(b);
                    }
                    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);
                }

            }
        }
            public List<PrimitiveDeBase>[] GetTableauListObjetCollisionables()
           {
             return TabListObjetCollisionables;
           }
         public List<SectionDeNiveau> GetListSectionNiveau()
        {
            return ListSections;
        }
        }
    }

