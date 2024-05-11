using AudioPlayer.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using AudioPlayer.Models;
using System.ComponentModel;
using Avalonia.Threading;
using System;
using System.IO;
using NAudio.Wave;
using ReactiveUI;
using NAudio.Utils;

namespace AudioPlayer.ViewModels;

public class PlayerViewModel : ViewModelBase
{
    private readonly Player _player;
    private PlayList _playList;
    private List<TrackInfo> _trackList;
    private TrackInfo _activeTrack;
    private bool _looping;
    private Bitmap _coverImage;
    private DispatcherTimer _timer;
    private double _position;
    public PlayList CurrentPlaylist { get => _playList; } 
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public Bitmap CoverImage { 
        get => _coverImage;
        set => this.RaiseAndSetIfChanged(ref _coverImage, value);
    }

    public TimeSpan TotalTime
    {
        get => _player.AudioFile.TotalTime;
    }

    public TimeSpan CurrentTime
    {
        get => _player.AudioFile.CurrentTime;
    }

    public double Position
    {
        get => _position;
        set
        {
            _player.SetPositionFile(value);
            _position = value;
            this.RaisePropertyChanged();
        }
    }

    public PlayerViewModel()
    {
        _trackList = new();
        _player = new();
        _player.TrackIsEnd += OnTrackEnd;
        _playList = new();
        _activeTrack = new();
        CoverImage = GetImage();
    }

    public PlayerViewModel(PlayList playlist)
    {
        _playList = playlist;
        _trackList = playlist.GetTracklist();
        _activeTrack = _trackList[0];
        CoverImage = _activeTrack.Image;
        _player = new Player(_activeTrack);
        _player.TrackIsEnd += OnTrackEnd;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_tick;
    }

    private void Timer_tick(object sender, EventArgs e)
    {
        if (_player != null && _player._outputDevice.PlaybackState == PlaybackState.Playing)
        {
            _position = _player.AudioFile.CurrentTime.TotalSeconds;
            this.RaisePropertyChanged(nameof(Position));
            this.RaisePropertyChanged(nameof(CurrentTime));
        }
    }
    
    private void OnTrackEnd(object sender, EventArgs e)
    {
        NextSong(_looping);
    }

    public void SetPlaylist(PlayList playlist)
    {
        _playList = playlist;
    }

    public void SetVolume(int value)
    {
        _player.Volume = value / 100;
    }

    public void TogglePlay()
    {
        if (_player.IsActive)
        {
            _player.PauseFile();
            _timer.Stop();
        }
        else
        {
            Play();
            _timer.Start();
        }
    }

    public void Play(TrackInfo track = null)
    {
        if (track != null)
            _activeTrack = track;
        CoverImage = GetImage(_activeTrack);
        Position = 0.0;
        _player.PlayFile();
        this.RaisePropertyChanged(nameof(CurrentTime));
        this.RaisePropertyChanged(nameof(TotalTime));
        this.RaisePropertyChanged(nameof(Position));
        this.RaisePropertyChanged(nameof(CoverImage));
    }

    public void LoopAudio()
    {
        _looping = !_looping;
    }

    public void NextSong(bool looping = false)
    {
        if (!looping)
        {
            var nextSongId = (_trackList.IndexOf(_activeTrack) != _trackList.Count) ?
              _trackList.IndexOf(_activeTrack) + 1 : 0;
            _activeTrack = _trackList[nextSongId];
            _player.AudioFile = new AudioFileReader(_activeTrack.Path);
        }
        Play(_activeTrack);
    }

    public void PrevSong()
    {
        var prevSongId = (_trackList.IndexOf(_activeTrack) != 0) ?
          _trackList.IndexOf(_activeTrack) - 1 : _trackList.Count;
        _activeTrack = _trackList[prevSongId];
        Play(_activeTrack);
    }

    public void Shuffle()
    {
        List<TrackInfo> tempList = new(_trackList);
        int n = tempList.Count;
        Random random = new Random();

        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            TrackInfo temp = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = temp;
        }

        List<TrackInfo> shuffledList = new(tempList);
        _trackList = shuffledList;
    }

   /* public string TotalTimeStr
    {
        get
        {
            // Console.WriteLine(_player.AudioFile.TotalTime.ToString(@"mm\:ss"));
            if (_player.AudioFile == null)
            {
                return "0:00";
            }
            return _player.AudioFile.TotalTime.ToString(@"mm\:ss");
        }
    }*/

   /* public string PositionStr
    {
        get
        {
            if (_player.AudioFile == null)
            {
                return "0:00";
            }
            return _player.AudioFile.CurrentTime.ToString(@"mm\:ss");
        }
    }*/

    public int Volume
    {
        get => _player.Volume;
        set => _player.Volume = value;
    }

    private Bitmap GetImage(TrackInfo track = null)
    {
        MemoryStream memory;
        if (track != null && Path.GetExtension(track.Path) != ".wav")
        {
            memory = new MemoryStream(TrackTag(track));
            return new Bitmap(memory);
        }
        memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        return new Bitmap(memory);
    }

    private byte[] TrackTag(TrackInfo track)
    {
        // hi there <3
        var images = TagLib.File.Create(track.Path).Tag.Pictures;
        return images[0].Data.Data;
    }
}
