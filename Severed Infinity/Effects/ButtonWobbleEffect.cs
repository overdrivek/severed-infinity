using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using SIEngine.Graphics.Shaders;

namespace SI.Effects
{
    /// <summary>
    /// Async class to animate a wobble effect on buttons
    /// </summary>
    public class ButtonWobbleEffect
    {
        public Button Target { get; set; }
        private ShaderProgram Program { get; set; }

        public ButtonWobbleEffect (Button target)
        {
            Target = target;
            Program = new ShaderProgram("data/effects/WobbleVertex.vert", "data/effects/WobbleFragment.frag");
            Program.UseProgram();
        }

        public void StartEffect()
        {

        }

        private void AnimationStep()
        {

        }

        public void EndEffect()
        {

        }
    }
}
