using AudioPlayer.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.ComponentModel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using NAudio.Wave;

namespace AudioPlayer.ViewModels;

public class PlayerViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly Player _player;
    private PlayList _playList;
    private List<TrackInfo> _trackList;
    private TrackInfo _activeTrack;
    private bool _looping;
    private bool _shuffled;
    private Bitmap _coverImage;
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public Bitmap CoverImage { 
        get => _coverImage;
        private set
        {
            if (Equals(_coverImage, value)) return;
            _coverImage = value;
            RaisePropertyChanged();
        }
    }
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        _player = new Player();
        _player.TrackIsEnd += OnTrackEnd;
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

    public void Play(TrackInfo track = null)
    {
        if (track != null)
            _activeTrack = track;
        CoverImage = GetImage(_activeTrack);
        _player.PlayFile();
    }

    public void Loop()
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
        RaisePropertyChanged();
    }

    public void PrevSong()
    {
        var prevSongId = (_trackList.IndexOf(_activeTrack) != 0) ?
          _trackList.IndexOf(_activeTrack) - 1 : _trackList.Count;
        _activeTrack = _trackList[prevSongId];
        Play(_activeTrack);
        RaisePropertyChanged();
    }

    public void Shuffle()
    {
        _shuffled = !_shuffled;
        if (_shuffled)
        {
            List<TrackInfo> shuffledList = [.._trackList];
            var n = shuffledList.Count;
            Random random = new Random();

            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (shuffledList[k], shuffledList[n]) = (shuffledList[n], shuffledList[k]);
            }
        
            _trackList = [..shuffledList];
        }
        else
        {
            _trackList = _playList.GetTracklist();
        }
    }

    public string TotalTimeStr
    {
        get => (_player.AudioFile == null) ? "00:00" : _player.AudioFile.TotalTime.ToString(@"mm\:ss");
    }

    public double TotalTime => _player.AudioFile.TotalTime.TotalSeconds;

    public double Position
    {
        get => _player.AudioFile.CurrentTime.TotalSeconds;
        set
        {
            _player.SetPositionFile(value);
            RaisePropertyChanged();
        }
    }

    public string PositionStr
    {
        get
        {
            if (_player.AudioFile == null)
            {
                return "0:00";
            }
            return _player.AudioFile.CurrentTime.ToString(@"mm\:ss");
        }
    }

    public int Volume
    {
        get => _player.Volume;
        set
        {
            _player.Volume = value;
            RaisePropertyChanged();
        }
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
        var images = TagLib.File.Create(track.Path).Tag.Pictures;
        return images[0].Data.Data;
    }
}
