using System;
using Un4seen.Bass;

namespace ConsoleClient.Audio
{
    class Sound : IDisposable
    {
        public int Handle { get; private set; }
        public string Path { get; private set; }
        public bool Stopped { get; private set; }

        /// <summary>
        /// Volume - range of 0.0f to 1.0f.
        /// </summary>
        public float Volume { get; set; }

        public bool Paused { get; set; }

        /// <summary>
        /// Whether or not the sound will loop continuously.
        /// </summary>
        public bool Loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                Bass.BASS_ChannelFlags(Handle, value ? BASSFlag.BASS_MUSIC_LOOP : BASSFlag.BASS_DEFAULT,
                    BASSFlag.BASS_MUSIC_LOOP);
            }
        }

        private bool _disposed;
        private bool _loop;
        /// <summary>
        /// Try not to call this externally, better design maybe internal class of AudioManager.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="path"></param>
        public Sound(int handle, string path)
        {
            Handle = handle;
            Path = path;
            Volume = 1.0f;
            Stopped = true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Bass.BASS_StreamFree(Handle);
                AudioManager.Unload(Path);
            }
        }

        /// <summary>
        /// Begin playing the audio.
        /// </summary>
        /// <param name="restart">Whether to start the song from the beginning when play is called.</param>
        /// <param name="loop">Whether to automatically loop the song.</param>
        public void Play(bool restart = false, bool loop = false)
        {
            Bass.BASS_ChannelPlay(Handle, restart);
            Paused = false;
            Loop = loop;
        }

        /// <summary>
        /// Stop playing a song. Varies from pause in that the current play position is lost.
        /// </summary>
        public void Stop()
        {
            Bass.BASS_ChannelStop(Handle);
            Stopped = true;
            Paused = false;
        }

        /// <summary>
        /// Pauses current song - maintains its position of playback for playing again.
        /// </summary>
        public void Pause()
        {
            Bass.BASS_ChannelPause(Handle);
            Paused = true;
        }
    }
}
