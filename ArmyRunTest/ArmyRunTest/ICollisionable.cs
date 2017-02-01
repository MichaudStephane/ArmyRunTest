using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelierXNA
{
    interface ICollisionable
    {
        bool EstCollisionable { get; set; }
        List<BoundingBox> ListeBoundingBoxes {get; set;}
        List<BoundingSphere> ListeBoundingSphere {get; set;}
        bool EstEnCollision(PrimitiveDeBaseAnimée a);
        Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a);
    }
}
