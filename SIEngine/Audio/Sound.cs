using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio;

namespace SIEngine.Audio
{
    public class Sound
    {
        public int Source { get; set; }
        public int Buffer { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public int BitsPerSample { get; set; }
        public string Name { get; set; }

        private float volume;
        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                AL.Source(Source, ALSourcef.Gain, value);
            }
        }
        public byte[] Data { get; set; }

        public Sound(byte[] data, int channels, int sampleRate, int bitsPerSample,
            string name)
        {
            Channels = channels;
            SampleRate = sampleRate;
            BitsPerSample = bitsPerSample;
            Name = name;

            Buffer = AL.GenBuffer();
            Source = AL.GenSource();
            
            AL.BufferData(Buffer, GeneralAudio.GetSoundFormat(channels, bitsPerSample),
                data, data.Length, sampleRate);

            AL.Source(Source, ALSourcei.Buffer, Buffer);
        }
    }
}
