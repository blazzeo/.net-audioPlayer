using ATL;
using System;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AudioPlayer.Models;

[ObservableObject]
public partial class Player
{
    private Track? activeTrack;
    private ISoundOut? outputDevice;
    private IWaveSource? audioFile;

    public event EventHandler<PlaybackStoppedEventArgs>? PlaybackStopped;

    public int Volume
    {
        get
        {
            if (outputDevice != null)
                return Math.Min(100, Math.Max((int)(outputDevice.Volume * 100), 0));
            return 100;
        }
        set
        {
            if (outputDevice != null)
            {
                outputDevice.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
            }
        }
    }

    public void Open(Track track, MMDevice device)
    {
        CleanupPlayback();

        activeTrack = track;

        audioFile =
            CodecFactory.Instance.GetCodec(track.Path)
                .ToSampleSource()
                .ToMono()
                .ToWaveSource();
        outputDevice = new WasapiOut() { Latency = 100, Device = device };
        outputDevice.Initialize(audioFile);
        if (PlaybackStopped != null) outputDevice.Stopped += PlaybackStopped;
    }

    public TimeSpan TotalTime
    {
        get => audioFile.GetLength();
    }

    public TimeSpan Position
    {
        get
        {
            if (audioFile != null)
                return audioFile.GetPosition();
            return TimeSpan.Zero;
        }
        set
        {
            if (audioFile != null)
                audioFile.SetPosition(value);
        }
    }

    public PlaybackState PlaybackState
    {
        get
        {
            if (outputDevice != null)
                return outputDevice.PlaybackState;
            return PlaybackState.Stopped;
        }
    }

    private bool IsPaused()
    {
        if (outputDevice != null)
        {
            return outputDevice.PlaybackState == PlaybackState.Paused;
        }
        return false;
    }

    private bool IsPlaying()
    {
        if (outputDevice != null)
        {
            return outputDevice.PlaybackState == PlaybackState.Playing;
        }
        return false;
    }

    public void Play()
    {
        if (outputDevice != null)
            outputDevice.Play();
    }

    public void TogglePlay()
    {
        if (outputDevice != null && IsPaused())
        {
            outputDevice.Play();
        }
        else if (outputDevice != null && IsPlaying())
        {
            outputDevice.Pause();
        }
    }

    public void Pause()
    {
        if (outputDevice != null)
            outputDevice.Pause();
    }

    public void Stop()
    {
        if (outputDevice != null)
            outputDevice.Stop();
    }

    private void CleanupPlayback()
    {
        if (outputDevice != null)
        {
            outputDevice.Dispose();
            outputDevice = null;
        }
        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }
    }

    // protected override void Dispose(bool disposing)
    // {
    //     base.Dispose(disposing);
    //     CleanupPlayback();
    // }
}
