using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.Graphics;
using SIEngine.Other;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public static class ModelManager
    {
        public class ManagedModel
        {
            public ManagedModel(float rarity, OBJModel model, bool unlocked, string name,
                string unlockMessage)
            {
                this.rarity = rarity;
                this.model = model;
                this.unlocked = unlocked;
                this.name = name;
                this.unlockMessage = unlockMessage;
            }

            public string unlockMessage;
            public float rarity;
            public OBJModel model;
            public bool unlocked;
            public string name;
        }

        public static int UnlockedModels { get; set; }
        public static List<ManagedModel> modelBank;

        private static void UnlockAnimation(GameWindow parent)
        {
            var infoBox = new InfoBox(parent, new Vector(200, 200),
                modelBank[UnlockedModels].unlockMessage);
            infoBox.Size.X = 250;
            infoBox.CanBeMoved = false;
            infoBox.Show();

            var obj = new Object();
            obj.Body = modelBank[UnlockedModels].model;

            //i know it's bad practice but since our camera is
            //always located at one point, it doesn't matter
            obj.Location = new Vector(0, 0, 0);
            obj.Body.Rotate = true;

            infoBox.OKClicked += (pos) =>
            {
                parent.Children3D.Remove(obj);
                obj.Body.Rotate = false;
            };
            infoBox.ExclamationClicked += (pos) =>
            {
                parent.Add3DChildren(obj);
            };
        }

        public static void UnlockModel(GameWindow parent)
        {
            modelBank[0].rarity -= modelBank[UnlockedModels].rarity;
            modelBank[UnlockedModels].unlocked = true;

            UnlockAnimation(parent);
            //UnlockedModels++;
        }

        public static OBJModel GetRandomModel()
        {
            float sum = 1f;
            float random = GeneralMath.RandomFloat(0f, 1f);
            for (int i = 0; i < modelBank.Count; ++ i)
            {
                float min = sum - modelBank[i].rarity;
                float max = sum;
                sum -= modelBank[i].rarity;

                if (random >= min && random <= max)
                    return modelBank[i].model;
            }
            return null;
        }

        static ModelManager()
        {
            int i = 0;

            modelBank = new List<ManagedModel>();
            OBJModel model;
            //first model
            
            model = new OBJModel("data/models/apple/apple.obj");
            model.Stroke = false;
            model.ScaleFactor = 0.03f;
            model.CalculateReach();
            modelBank.Add(new ModelManager.ManagedModel(1f, model,
                Properties.Settings.Default.unlockStatus[i++], "Apple", ""));
            

            //second model
            i++;
            model = new OBJModel("data/models/cake/pie.obj");
            
            model.ScaleFactor = 0.003f;
            model.Stroke = false;
            model.ApplyOriginalObjectColor = true;
            model.CalculateReach();

            modelBank.Add(new ModelManager.ManagedModel(.3f, model,
                Properties.Settings.Default.unlockStatus[i++], "IPhone",
                "You have unlocked a pie!.\n"
                + "Congrats. Now you can\n"
                + "explode more than apples!\n"
                + "Go on, explode it.\n"
                + "Now, mate!"));
            if (Properties.Settings.Default.unlockStatus[i - 1] == true)
                modelBank[0].rarity -= modelBank[i - 1].rarity;
            
            UnlockedModels = 1;
        }
    }
}
