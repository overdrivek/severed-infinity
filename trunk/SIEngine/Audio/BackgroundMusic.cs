using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio;
using SIEngine.Other;
using SIEngine.Graphics;
using System.Windows.Forms;
using SIEngine.GUI;

namespace SIEngine.Audio
{
    /// <summary>
    /// A class to play background music.
    /// Make sure to link it to your window
    /// to see notifications when switching
    /// songs.
    /// </summary>
    public static class BackgroundMusic
    {
        private static Timer mainTimer;
        private static float currentVolume;
        private static Sound previousSound;
        public static Sound CurrentSound { get; set; }
        public static List<Sound> Playlist { get; set; }
        public static Window DisplayWindow { get; private set; }
        private static AudioNotificationControl notificationControl;
        
        static BackgroundMusic()
        {
            Playlist = new List<Sound>();
            
            mainTimer = new Timer();
            mainTimer.Interval = 10;
            mainTimer.Tick += FadeOutStep;

            notificationControl = new AudioNotificationControl();
        }

        /// <summary>
        /// Adds a song to the playlist.
        /// The song MUST be loaded first.
        /// </summary>
        /// <param name="?"></param>
        public static void AddSongs(params string[] songs)
        {
            foreach (var song in songs)
            {
                var sound = GeneralAudio.GetSound(song);
                if (sound == null)
                    continue;
                Playlist.Add(sound);
            }
        }

        private static void FadeOutStep(object sender, EventArgs evArgs)
        {
            if (currentVolume <= 0)
            {
                mainTimer.Stop();
                if (previousSound != null)
                    GeneralAudio.StopSound(previousSound.Name);

                notificationControl.Visible = false;
            }

            notificationControl.ShiftOpacity();

            currentVolume -= GameConstants.VolumeFadeOut;
            if(previousSound != null)
                previousSound.Volume = currentVolume;
            CurrentSound.Volume = GameConstants.MaxMusicVolume - currentVolume;
        }

        private static void FadeOut()
        {
            currentVolume = GameConstants.MaxMusicVolume;
            mainTimer.Start();

            GeneralAudio.PlaySound(CurrentSound.Name);
            CurrentSound.Volume = 0f;


            notificationControl.Visible = true;
            notificationControl.ResetShifting();
            notificationControl.MessageSound = CurrentSound;
            
        }

        /// <summary>
        /// Plays a random song from the playlist.
        /// </summary>
        public static void NextSong()
        {
            int index = Playlist.IndexOf(CurrentSound);
            int song = index;

            //we should not play the current song again
            while(song == index) song = GeneralMath.RandomInt() % Playlist.Count;

            if(index != -1) previousSound = Playlist[index];
            CurrentSound = Playlist[song];

            FadeOut();
        }

        public static void LinkToWindow(Window parent)
        {
            DisplayWindow = parent;
            DisplayWindow.AddChildren(notificationControl);
        }
    }
}
