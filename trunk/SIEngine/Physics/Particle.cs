using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;

namespace SIEngine.Physics
{
    public class Particle
    {
        PhysicsObject ActualObject { get; set; }
        Polygon VisualObject { get; set; }
        

        public Particle ()
        {
            ActualObject = new PhysicsObject();
            VisualObject = new Polygon();
        }

        public static implicit operator GUIObject(Particle vec)
        {
            return vec.VisualObject;
        }
    }
}
