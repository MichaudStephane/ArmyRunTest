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
        float Masse { get; set; }
        List<Vector3> ListeVecteurs { get; set; }
        float IntervalleDeplacement { get; set; }
        Vector3 VecteurResultants { get; set; }
        Vector3 CalculerVecteurRésultant();
        void ModifierIntervalle();
        void Bouger();

    }
}
