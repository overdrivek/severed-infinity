using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Physics;
using SIEngine.Graphics.ParticleEngines;
using System.Windows.Forms;
using Button = SIEngine.GUI.Button;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    /// <summary>
    /// Represents an object that flies through the scene.
    /// </summary>
    public class FlyingObject : Object
    {
        /// <summary>
        /// True for paused, false otherwise.
        /// </summary>
        public bool Paused { get; set; }
        /*{
            set
            {
                if (!value)
                    MainTimer.Start();
                else MainTimer.Stop();
            }
        }*/
        /// <summary>
        /// A reference to the managed model for this object.
        /// </summary>
        public ModelManager.ManagedModel ModelReference { get; set; }

        /// <summary>
        /// Indicates that the flying object is in the current
        /// window and has not yet been exploded or completed
        /// its life cycle.
        /// </summary>
        public bool Alive { get; set; }

        /// <summary>
        /// The physical body of the object. Used to calculate
        /// parabolic movement.
        /// </summary>
        public PhysicsObject PhysicalBody { get; set; }

        /// <summary>
        /// Main animation timer.
        /// </summary>
        public Timer MainTimer { get; set; }

        //blur

        /// <summary>
        /// Toggles the motion blur the model leaves behind itself.
        /// </summary>
        public bool EnableMotionBlur { get; set; }

        /// <summary>
        /// Sets the length of the blur trail.
        /// </summary>
        public int BlurStackSize { get; set; }

        /// <summary>
        /// The time, in milliseconds, which the object lives for.
        /// </summary>
        public int Lifespan { get; set; }

        private LinkedList<Vector> BlurStack { get; set; }
        private float alphaStep;
        private int time;

        /// <summary>
        /// Initializes an instance of this class, along with the values for
        /// using it.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="model"></param>
        /// <param name="blurStackSize"></param>
        public FlyingObject(GameWindow parent, OBJModel model, int blurStackSize)
        {
            Parent = parent;
            Lifespan = 100;
            
            //blur
            BlurStackSize = blurStackSize;
            BlurStack = new LinkedList<Vector>();
            EnableMotionBlur = true;
            alphaStep = .5f / BlurStackSize;

            PhysicalBody = new PhysicsObject();
            PhysicalBody.ParentObject = this;
            PhysicalBody.Velocity = new Vector(GeneralMath.RandomFloat(-0.7f, 0.7f),
                GeneralMath.RandomFloat(1.5f, 2.5f), GeneralMath.RandomFloat(-0.5f, 0.5f));

            Body = model;

            Location = new Vector(0.0f, 0.0f, 0.0f);
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Tick += AnimationStep;
        }

        public void Start()
        {
            Alive = true;
            MainTimer.Start();
            BlurStack.Clear();

            for (int i = 0; i < BlurStackSize; ++i)
                BlurStack.AddFirst((Vector)null);

            Parent.Add3DChildren(this);
        }

        public void Kill()
        {
            MainTimer.Stop();
            Alive = false;
            Parent.Children3D.Remove(this);
        }

        private void AnimationStep(object sedner, EventArgs evArgs)
        {
            if (EnableMotionBlur)
            {
                Vector temp = BlurStack.Last.Value;
                BlurStack.RemoveLast();
                BlurStack.AddFirst(temp);

                if (BlurStack.First.Value == null)
                    BlurStack.First.Value = new Vector(0f, 0f, 0f);

                BlurStack.First().X = Location.X;
                BlurStack.First().Y = Location.Y;
                BlurStack.First().Z = Location.Z;
            }

            if (Paused)
                return;
            PhysicalBody.ApplyNaturalForces();
            PhysicalBody.ModulatePhysics();

            if (time >= Lifespan && !Paused)
                Kill();
            time++;
        }

        public override void Draw()
        {
            GeneralGraphics.EnableAlphaBlending();
            GeneralGraphics.UseDefaultShaderProgram();
            if(ShaderProgram != null)
                ShaderProgram.UseProgram();
            bool rotate = Body.Rotate;

            GL.PushMatrix();
            {
                GL.Color4(Color.White);
                GL.Translate(Location.X, Location.Y, Location.Z);
                Body.Draw();
            }
            GL.PopMatrix();
            GeneralGraphics.UseDefaultShaderProgram();

            float alpha = 0.6f;
            
            Body.Rotate = false;
                foreach (var trail in BlurStack)
                {
                    if (trail == null)
                        continue;
                    
                    GL.PushMatrix();
                    {
                        GL.Translate(trail.X, trail.Y, trail.Z);
                        Body.Draw(Color.FromArgb((byte)(255 * alpha), Color.LightGray));
                    }
                    GL.PopMatrix();
                    //Console.WriteLine(alpha);

                    alpha -= alphaStep;
                }

                Body.Rotate = rotate;

            GeneralGraphics.DisableBlending();
        }
    }
}
