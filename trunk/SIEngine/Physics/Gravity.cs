using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;

namespace SIEngine.Physics
{
    public static class Gravity
    {
        public static Vector Force;
        public static void ApplyGravity (PhysicsObject target)
        {
            target.Velocity.X += Force.X;
            target.Velocity.Y += Force.Y;
            target.Velocity.Z += Force.Z;
        }

        static Gravity()
        {
            //note 1 pixel = 1 cm
            Force = new Vector(0.0f, -0.09780327f, 0.0f);
        }
    }
}
