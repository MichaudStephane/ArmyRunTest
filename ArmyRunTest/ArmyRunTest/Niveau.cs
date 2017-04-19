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
        Random G�n�rateurAl�atoire { get; set; }
        int NbrSections { get; set; }
        Game Jeu { get; set; }
        public Vector3 Position { get; private set; }
        public virtual bool AD�truire { get; set; }
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
            AD�truire = false;
            Jeu = jeu;
            NbrSections = nbrSections;
            TabListObjetCollisionables = new List<PrimitiveDeBase>[NbrSections];
            for(int cpt=0; cpt< NbrSections;++cpt)
            {
                TabListObjetCollisionables[cpt] = new List<PrimitiveDeBase>();
            }
            ListSections = new List<SectionDeNiveau>();
            G�n�rateurAl�atoire = new Random();
            Position = positionInitiale;
            Cr�erNiveau();
            
        }
        public void D�truireNiveau()
        {
            AD�truire = true;
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

        void Cr�erNiveau()
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
                //int nombreAl�atoire = G�n�rateurAl�atoire.Next(0, NbrSectionsDisponibles + 1);
                int nombreAl�atoire = 0;
                if (nombreAl�atoire == 0)
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
                else if (nombreAl�atoire == 1)
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
                else if (nombreAl�atoire == 2)
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
                else if (nombreAl�atoire == 3)
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
                else if (nombreAl�atoire == 4)
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
                //else if (nombreAl�atoire == 5)
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