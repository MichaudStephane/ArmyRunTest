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
    public class Niveau : IDestructible
    {
        List<PrimitiveDeBase>[] TabListObjetCollisionables { get; set; }
        const int NbrSectionsDisponibles = 5;
        List<SectionDeNiveau> ListSections { get; set; }
        Random GénérateurAléatoire { get; set; }
        int NbrSections { get; set; }
        Game Jeu { get; set; }
        public Vector3 Position { get; private set; }
        public virtual bool ADétruire { get; set; }
        public float LongueurNiveau
        {
            get
            {
                float longueur =0;
                foreach(SectionDeNiveau a in ListSections)
                {
                    longueur += a.LongueurNiveau;
                }
                return longueur;
            }
        }
        public Niveau(Game jeu, int nbrSections, Vector3 positionInitiale)
        {
            ADétruire = false;
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
        public void DétruireNiveau()
        {
            ADétruire = true;
            foreach (List<PrimitiveDeBase> p in TabListObjetCollisionables)
            {
                p.Clear();
            }
            ListSections.Clear();
            //GameComponent[] tab = (GameComponent[])Jeu.Components.ToArray();
            List<int> index = new List<int>();
            for (int i = 0; i < Jeu.Components.Count(); i++)
            {
                if(Jeu.Components[i] is PrimitiveDeBase)
                {
                    index.Add(i);
                    

                }
            }

            index.OrderByDescending(x =>x);
            //for(int j = 0; j < index.Count(); j++)
            //{
                
             
            //    tab.(index[j]);
            //}
        }

        void CréerNiveau()
        {
            SectionRepos c = new SectionRepos(Jeu, Position, 0);
            ListSections.Add(c);
            Jeu.Components.Add(c);
            foreach (PrimitiveDeBase b in c.ObjetCollisionables)
            {
                TabListObjetCollisionables[0].Add(b);
            }
            Position = new Vector3(Position.X, Position.Y, Position.Z - c.LongueurNiveau);

            for (int i = 1; i < NbrSections; ++i)
            {
                //int nombreAléatoire = GénérateurAléatoire.Next(0, NbrSectionsDisponibles + 1);
                int nombreAléatoire = 0;
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
                //else if (nombreAléatoire == 5)
                //{
                //    SectionMobileMultiples a = new SectionMobileMultiples(Jeu, Position, i);
                //    ListSections.Add(a);
                //    Jeu.Components.Add(a);
                //    foreach (PrimitiveDeBase b in a.ObjetCollisionables)
                //    {
                //        TabListObjetCollisionables[i].Add(b);
                //    }
                //    Position = new Vector3(Position.X, Position.Y, Position.Z - a.LongueurNiveau);
                //}

            }
        }
        public List<PrimitiveDeBase>[] GetTableauListObjetCollisionables()
        {
            List<PrimitiveDeBase>[] Copie = new List<PrimitiveDeBase>[TabListObjetCollisionables.Count()];
            for(int cpt=0; cpt < Copie.Count(); ++ cpt)
            {
                Copie[cpt] = new List<PrimitiveDeBase>();
            }


            for(int i=0; i < TabListObjetCollisionables.Count();++i )
            {
                foreach(PrimitiveDeBase a in TabListObjetCollisionables[i])
                {
                    Copie[i].Add(a);
                }
            }
             return Copie;
           }
         public List<SectionDeNiveau> GetListSectionNiveau()
        {
            List<SectionDeNiveau> Copie = new List<SectionDeNiveau>(NbrSections);
            foreach (SectionDeNiveau a in ListSections)
            {
                Copie.Add(a);
            }
            return Copie;
        }
    }
}