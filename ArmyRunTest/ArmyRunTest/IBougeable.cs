using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
      interface IBougeable
    {
      

      
        
        float Intervalle_MAJ_Mouvement { get; set; }
        float TempsEcouleDepuisMajMouvement { get; set; }

         void CalculerVecteurResultant();
         void InitialiserListe();

        void ModifierIntervalle();


    }
}
