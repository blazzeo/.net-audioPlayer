using AudioPlayer.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
using System.IO;
using NAudio.Wave;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class PlayerViewModel : ViewModelBase
{
    private readonly Player _player;
    private PlayList _playList;
    private List<TrackInfo> _trackList;
    private ObservableCollection<TrackInfo> _queueList = [];
    private TrackInfo _activeTrack;
    private bool _looping = false;
    private bool _shuffled = false;
    private Bitmap _coverImage;
    private readonly DispatcherTimer _timer;
    private double _position;

    public bool IsShuffled => _shuffled;
    public bool IsActive => !_player.IsActive;

    public TrackInfo ActiveTrack
    {
        get => _activeTrack;
        set => this.RaiseAndSetIfChanged(ref _activeTrack, value);
    }
    public PlayList CurrentPlaylist => _playList;
    public string Title => _playList.Name ?? "Empty";

    public ObservableCollection<TrackInfo>? QueueTrackList
    {
        get => _queueList;
        set => this.RaiseAndSetIfChanged(ref _queueList, value);
    }

    public Bitmap CoverImage
    {
        get => _coverImage;
        set => this.RaiseAndSetIfChanged(ref _coverImage, value);
    }

    public TimeSpan TotalTime => _player.AudioFile != null ? _player.AudioFile.TotalTime : TimeSpan.Zero;

    public TimeSpan CurrentTime => _player.AudioFile != null ? _player.AudioFile.CurrentTime : TimeSpan.Zero;

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
        _playList = new PlayList();
        _trackList = [];
        _player = new Player();
        _player.TrackIsEnd += OnTrackEnd;
        ActiveTrack = null;
        CoverImage = GetImage();
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_tick;
    }

    public PlayerViewModel(PlayList playlist)
    {
        _playList = playlist;
        _trackList = playlist.GetTrackList();
        ActiveTrack = _trackList[0];
        CoverImage = ActiveTrack.Image;
        _player = new Player(ActiveTrack);
        _player.TrackIsEnd += OnTrackEnd;
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(700.0)
        };
        _timer.Tick += Timer_tick;
    }

    private void Timer_tick(object sender, EventArgs e)
    {
        if (_player == null || _player.OutputDevice.PlaybackState != PlaybackState.Playing) return;
        _position = _player.AudioFile.CurrentTime.TotalSeconds;
        this.RaisePropertyChanged(nameof(Position));
        this.RaisePropertyChanged(nameof(CurrentTime));
    }

    private void OnTrackEnd(object sender, EventArgs e)
    {
        NextSong();
    }

    public void SetPlaylist(PlayList playlist, int id = -1)
    {
        _playList = playlist;
        _trackList = _playList.GetTrackList();
        QueueTrackList = playlist.TrackList;
        this.RaisePropertyChanged(nameof(Title));
        this.RaisePropertyChanged(nameof(QueueTrackList));
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
            this.RaisePropertyChanged(nameof(IsActive));
            _timer.Stop();
        }
        else
        {
            Play();   
        }
    }

    public void Play(TrackInfo track = null)
    {
        if (track != null)
        {
            ActiveTrack = track;
            _player.StopFile();
            _player.AudioFile = new AudioFileReader(ActiveTrack.Path);
            Position = 0.0;
        }
        _timer.Start();
        this.RaisePropertyChanged(nameof(IsActive));

        CoverImage = GetImage(ActiveTrack);
        _player.PlayFile();
        this.RaisePropertyChanged(nameof(IsActive));
        this.RaisePropertyChanged(nameof(CurrentTime));
        this.RaisePropertyChanged(nameof(TotalTime));
        this.RaisePropertyChanged(nameof(Position));
        this.RaisePropertyChanged(nameof(CoverImage));
    }

    public void LoopAudio()
    {
        _looping = !_looping;
    }

    public void NextSong()
    {
        if (ActiveTrack is null) return;
        var nextTrack = ActiveTrack;
        if (!_looping)
        {
            var curSongId = _trackList.IndexOf(ActiveTrack);
            var nextSongId = (curSongId != _trackList.Count - 1) ?
              curSongId + 1 : 0;
            nextTrack = _trackList[nextSongId];
        }
        Play(nextTrack);
    }

    public void PrevSong()
    {
        if (ActiveTrack is null) return;
        var curSongId = _trackList.IndexOf(ActiveTrack);
        var prevSongId = (curSongId != 0) ?
          curSongId - 1 : _trackList.Count - 1;
        var nextTrack = _trackList[prevSongId];
        Play(nextTrack);
    }

    public void Shuffle()
    {
        if (_shuffled)
        {
            _trackList = _playList.GetTrackList();
            _shuffled = false;
            this.RaisePropertyChanged(nameof(IsShuffled));
            return;
        }

        _shuffled = true;
        this.RaisePropertyChanged(nameof(IsShuffled));
        List<TrackInfo> tempList = [.._trackList];
        var n = tempList.Count;
        var random = new Random();

        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (tempList[k], tempList[n]) = (tempList[n], tempList[k]);
        }

        List<TrackInfo> shuffledList = [..tempList];
        _trackList = shuffledList;
    }

    public int Volume
    {
        get => _player.Volume;
        set => _player.Volume = value;
    }

    private static Bitmap GetImage(TrackInfo track = null)
    {
        if (track != null && Path.GetExtension(track.Path) != ".wav")
        {
            return track.Image;
        }
        var memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        return new Bitmap(memory);
    }
}
