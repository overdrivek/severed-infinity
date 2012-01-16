using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics.ParticleEngines
{
    public class Explosion : Object
    {
        private ExplosionParticleEmitter explosionEmitter;
        private ShockwaveParticleEmitter shockEmitter;
        private DebrisParticleEmitter debrisEmitter;
        public float Scale { get; set; }
        public bool HasDebris { get; set; }

        public Explosion(int numParticles, bool hasDebris = false)
        {
            HasDebris = hasDebris;
            explosionEmitter = new ExplosionParticleEmitter(numParticles);
            shockEmitter = new ShockwaveParticleEmitter();
            debrisEmitter = new DebrisParticleEmitter(numParticles);
            Scale = 1f;
        }

        public void Explode()
        {
            explosionEmitter.Start();
            shockEmitter.Start();
            if(HasDebris)
                debrisEmitter.Start();
        }

        public override void Draw()
        {
            GL.PushMatrix();
            {
                GL.Translate(Location.X, Location.Y, Location.Z);
                GL.Scale(Scale, Scale, Scale);

                explosionEmitter.Draw();
                shockEmitter.Draw();
                if (HasDebris)
                    debrisEmitter.Draw();
            }
            GL.PopMatrix();
        }
    }
}
