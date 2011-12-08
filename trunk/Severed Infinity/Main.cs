using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using SIEngine;
using SIEngine.GUI;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.Physics;
using System.Threading;
using Vector = SIEngine.BaseGeometry.Vector;
using Button = SIEngine.GUI.Button;
using TextBox = SIEngine.GUI.TextBox;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace HW
{
    class EntryPoint
    {
        static void Main (string[] Args)
        {
            Window window = new Window(800, 600, "Demo");
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -350.0f;
            
            Button button = new Button();
            button.Text = "Play";
            button.Image = "data/img/bck.bmp";
            button.ApplyStylishEffect();
            button.Location = new Vector(300, 150);

            Object obj = new Object();
            OBJModel model = new OBJModel();
            //model.ParseOBJFile("data/models/windmill/windmill.obj");
            model.ScaleFactor = 0.1f;
            obj.Body = model;

            Button button1 = new Button();
            button1.Text = "I'm a button";
            button1.Image = "data/img/bck.bmp";
            button1.ApplyStylishEffect();
            button1.Location = new Vector(300, 180);

            Button button2 = new Button();
            button2.Text = "Exit";
            button2.Image = "data/img/bck.bmp";
            button2.ApplyStylishEffect();
            button2.Location = new Vector(300, 210);

            Label label = new Label();
            label.Location = new Vector(300, 250);
            label.Text = "I'm a label.";

            TextBox textbox = new TextBox();
            textbox.Location = new Vector(300, 300);
            textbox.Size = new Vector(130, 20);
            textbox.Text = "I'm a textbox.";

            Skybox skybox = new Skybox();

            window.Children.Add(button);
            window.Children.Add(button1);
            window.Children.Add(button2);
            //window.Children3D.Add(obj);
            window.Children.Add(textbox);
            window.Children.Add(label);
            window.Children3D.Add(skybox);

            window.Run(30);
        }
    }
}