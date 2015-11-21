using Logging;
using System;
using System.Collections.Generic;
using System.IO;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace ConsoleClient.Audio
{
    static class AudioManager
    {
        private static readonly Dictionary<String, Sound> _audios = new Dictionary<string, Sound>();
        private static bool _initialised;

        /// <summary>
        /// Initialise the audio library Bass. Only needs to be called once, gets called automatically
        /// if you try to play audio.
        /// </summary>
        public static void Initialise()
        {
            if (_initialised) return;
            Frequency = 44100;
            Bass.BASS_Init(-1, Frequency, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            BassFx.LoadMe();
            _initialised = true;
        }

        /// <summary>
        /// Load a music file from a filename. Adds to a dict to keep loaded sounds cached.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sound LoadFromFile(string path)
        {
            Initialise();
            if (_audios.ContainsKey(path))
            {
                return _audios[path];
            }

            if (!File.Exists(path)) return null;
            int handle = Bass.BASS_StreamCreateFile(path, 0, 0,
                BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            handle = BassFx.BASS_FX_TempoCreate(handle, BASSFlag.BASS_MUSIC_PRESCAN);

            if (handle == 0)
            {
                return null;
            }

            try
            {
                var audio = new Sound(handle, path);
                _audios.Add(path, audio);
                return audio;
            }
            catch (Exception ex)
            {
                Out.Red(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Unload a sound. Useful for manually clearing memory.
        /// </summary>
        /// <param name="path"></param>
        public static void Unload(string path)
        {
            if (!_audios.ContainsKey(path)) return;
            _audios[path].Dispose();
            _audios.Remove(path);
        }

        /// <summary>
        /// Audio frequency, keep at 44100 mainly.
        /// </summary>
        public static int Frequency { get; set; }
    }
}
