using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;

namespace SIEngine.Physics
{
    public class PhysicsObject
    {
        public float Mass { get; set; }
        public Vector Velocity { get; set; }
        public GUIObject ParentObject { get; set; }

        public void ModulatePhysics ()
        {
            ParentObject.Location.X *= Velocity.X;
            ParentObject.Location.Y *= Velocity.Y;
            ParentObject.Location.Z *= Velocity.Z;
            ParentObject.Location.W *= Velocity.W;
        }
        public void ApplyNaturalForces()
        {
            Gravity.ApplyGravity(this);
        }
    }
}
