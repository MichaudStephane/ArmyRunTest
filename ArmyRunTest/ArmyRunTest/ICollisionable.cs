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
        bool EstCollisionable { get; }
        BoundingBox HitBoxGénérale { get; set; }
        Vector3 DonnerVectorCollision(PrimitiveDeBaseAnimée a);
    }
}
