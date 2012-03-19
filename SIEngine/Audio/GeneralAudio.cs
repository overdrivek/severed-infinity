using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio;
using System.IO;
using System.Diagnostics;
using SIEngine.Graphics.ParticleEngines;
using SIEngine.Other;

namespace SIEngine.Audio
{
    /// <summary>
    /// A basic class to manage wav file playback.
    /// </summary>
    public static class GeneralAudio
    {
        private static Dictionary<string, Sound> Sounds;
        
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public static bool LoadSound(string path, string name)
        {
            if (!File.Exists(path))
                return false;

            int channels, bitsPerSample, sampleRate;
            byte[] soundData = LoadWave(File.Open(path, FileMode.Open),
                out channels, out bitsPerSample, out sampleRate);

            var sound = new Sound(soundData, channels, sampleRate, bitsPerSample, name);
            sound.Volume = GameConstants.DefaultSoundVolume;
            Sounds.Add(name, sound);

            return true;
        }

        public static Sound GetSound(string name)
        {
            Sound sound;
            Sounds.TryGetValue(name, out sound);
            return sound;
        }

        public static void PlaySound(string name)
        {
            Sound sound;
            if (!Sounds.TryGetValue(name, out sound))
                return;
            
            AL.SourcePlay(sound.Source);
        }

        public static void StopSound(string name)
        {
            Sound sound;
            if (!Sounds.TryGetValue(name, out sound))
                return;
            
            AL.SourceStop(sound.Source);
        }

        private static AudioContext Context;
        static GeneralAudio()
        {
            Sounds = new Dictionary<string, Sound>();
            
            Context = new AudioContext();
        }
    }
}
