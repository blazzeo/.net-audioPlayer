using System;
using NAudio.Wave;

namespace AudioPlayer.Models
{
    [Serializable]
    public class Player
    {
        [NonSerialized] private AudioFileReader _audioFile;
        public bool IsActive;

        [NonSerialized] public WaveOutEvent _outputDevice = new WaveOutEvent();
       

        private int _volume = 10; // min 0, max 100

        public Player()
        {
            OutputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        
        public Player(TrackInfo track)
        {
            AudioFile = new AudioFileReader(track.Path);
            OutputDevice.PlaybackStopped += OnPlaybackStopped;
            OutputDevice.Init(AudioFile);
        }

        public AudioFileReader AudioFile
        {
            get => _audioFile;
            set
            {
                _audioFile = value;
                if (_audioFile != null)
                {
                    _audioFile.Volume = Volume / 100f;
                    if (OutputDevice == null)
                    {
                        OutputDevice = new WaveOutEvent();
                        OutputDevice.PlaybackStopped += OnPlaybackStopped;
                    }
                }
            }
        }

        public WaveOutEvent OutputDevice
        {
            get => _outputDevice;
            set => _outputDevice = value;
        }

        public int Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                if (AudioFile != null)
                    AudioFile.Volume = value / 100f;
            }
        }
        
        public event EventHandler<EventArgs> TrackIsEnd;

        public void PlayFile()
        {
            if (AudioFile != null)
            {
                if (OutputDevice.PlaybackState == PlaybackState.Stopped)
                    OutputDevice.Init(AudioFile);
                OutputDevice.Play();
                IsActive = true;
            }
        }

        public void StopFile()
        {
            OutputDevice?.Stop();
            OnPlaybackStopped(this, new StoppedEventArgs());
            IsActive = false;
        }

        public void PauseFile()
        {
            OutputDevice?.Pause();
            IsActive = false;
        }

        public void SetPositionFile(double pos)
        {
            TimeSpan secs = new(0, 0, (int)pos);
            if (AudioFile != null)
                AudioFile.CurrentTime = secs;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (AudioFile != null && AudioFile.FileName != string.Empty)
                if (sender is Player)
                {
                    OutputDevice?.Dispose();
                    OutputDevice = null;
                    AudioFile?.Dispose();
                    AudioFile = null;
                }
                else if ((int) (AudioFile.CurrentTime.TotalSeconds + 0.026) == (int) AudioFile.TotalTime.TotalSeconds)
                {
                    OutputDevice?.Dispose();
                    OutputDevice = null;
                    AudioFile?.Dispose();
                    AudioFile = null;
                    TrackIsEnd.Invoke(sender, args);
                }
        }
    }
}
