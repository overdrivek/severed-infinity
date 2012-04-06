using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SI.Other;
using SI.Properties;
using SIEngine.Audio;

namespace SI.GUI
{
    class BetaVerification
    {
        private TextBox nameBox, keyBox;
        private Label nameLabel, keyLabel, mainLabel, wrongKeyMessage;
        private Button verifyButton;
        public GameWindow Parent { get; private set; }

        public BetaVerification(GameWindow parent)
        {
            mainLabel = new Label();
            mainLabel.Location = new Vector(350, 180);
            mainLabel.Text = "Please enter your open beta key.";

            nameBox = new TextBox();
            nameBox.Location = new Vector(325, 220);
            
            nameLabel = new Label();
            nameLabel.Location = new Vector(315, 200);
            nameLabel.Text = "Name:";

            keyLabel = new Label();
            keyLabel.Location = new Vector(325, 240);
            keyLabel.Text = "Beta key:";

            keyBox = new TextBox();
            keyBox.Location = new Vector(300, 260);
            keyBox.Size.X = 200;

            verifyButton = new Button();
            verifyButton.Location = new Vector(335, 290);
            verifyButton.ApplyStylishEffect();
            verifyButton.Image = "data/img/bck.bmp";
            verifyButton.Text = "Verify";

            verifyButton.MouseClick += (pos) =>
                {
                    if (OpenBetaFunctions.VerifyBetaKey(nameBox.Text, keyBox.Text))
                    {
                        Parent.Children.Remove(nameBox);
                        Parent.Children.Remove(nameLabel);
                        Parent.Children.Remove(keyBox);
                        Parent.Children.Remove(keyLabel);
                        Parent.Children.Remove(verifyButton);
                        Parent.Children.Remove(wrongKeyMessage);
                        Parent.Children.Remove(mainLabel);

                        Settings.Default.CredentialsVerified = true;
                        Settings.Default.Save();

                        if (Settings.Default.MusicStatus)
                            BackgroundMusic.StartPlayback();

                        Parent.Menu = new MainMenu(Parent);
                    }
                    else wrongKeyMessage.Visible = true;
                };

            wrongKeyMessage = new Label();
            wrongKeyMessage.Text = "The key and username do not match.";
            wrongKeyMessage.Location = new Vector(340, 310);
            wrongKeyMessage.Visible = false;

            Parent = parent;

            Parent.AddChildren(nameLabel, nameBox, mainLabel, keyLabel, keyBox, verifyButton, wrongKeyMessage);

            Console.WriteLine(nameBox.Size.X);
        }
    }
}
