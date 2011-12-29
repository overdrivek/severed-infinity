using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Physics
{
    public class PhysicsObject
    {
        public float Mass { get; set; }
        public Vector Velocity { get; set; }
        public Object ParentObject { get; set; }

        public void ModulatePhysics ()
        {
            ParentObject.Location += Velocity;
        }
        public void ApplyNaturalForces()
        {
            Gravity.ApplyGravity(this);
        }
    }
}
