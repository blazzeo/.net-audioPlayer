using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Collections.Generic;
using AudioPlayer.Models;
using System;
using System.IO;
using AudioPlayer.DataTemplate;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class PlayListViewModel : ViewModelBase
{
    private MainWindowViewModel _window;
    public PlayList _playList;
    private Bitmap _image;
    private readonly List<TrackInfo> _trackList;
    public string Title { get; }
    private FlatTreeDataGridSource<TrackInfo> _audioSource;
    public Bitmap Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    public FlatTreeDataGridSource<TrackInfo> AudioSource
    {
        get => _audioSource;
        set => this.RaiseAndSetIfChanged(ref _audioSource, value);
    }

    public PlayListViewModel(MainWindowViewModel window)
    {
        _window = window;
        _playList = new();
        _trackList = new List<TrackInfo>();
        Image = new Bitmap(new MemoryStream(System.IO.File.ReadAllBytes("Assets/default-audio.png")));
        Title = "Empty";
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList);
    }

    public PlayListViewModel(PlayList playList, MainWindowViewModel window)
    {
        _window = window;
        _playList = playList;
        _trackList = new(_playList.TrackList);

        Image = playList.Image;
        Title = playList.Name!;

        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList)
        {
            Columns = {
                new TemplateColumn<TrackInfo>("Cover", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(a))),
                new TextColumn<TrackInfo, string>("Title", x => x.Title, GridLength.Parse("220")),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist, GridLength.Parse("150")),
                new TextColumn<TrackInfo, string>("Album", x => x.Album, GridLength.Parse("150")),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
          }
        };
    }

    public void ChangeGrid(List<TrackInfo> tracklist)
    {
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(tracklist)
        {
            Columns = {
                new TemplateColumn<TrackInfo>("Cover", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(a))),
                new TextColumn<TrackInfo, string>("Title", x => x.Title, GridLength.Parse("220")),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist, GridLength.Parse("150")),
                new TextColumn<TrackInfo, string>("Album", x => x.Album, GridLength.Parse("150")),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
            }
        };
    }

    private Control GetButton(TrackInfo track)
    {
        var it = new ImageButtonCommand();
        var img = GetImage(track);
        Tuple<PlayerViewModel, PlayList, TrackInfo, Control> tuple = new(_window.PlayerVm, _playList, track, img);

        // Console.WriteLine(track.Title);
        return it.Build(tuple);
    }

    private Control GetImage(TrackInfo track)
    {
        var it = new SongImageTemplate();
        if (track == null)
        {
            return it.Build(new Bitmap(new MemoryStream(System.IO.File.ReadAllBytes("Assets/default-audio.png"))));
        }
        else
            return it.Build(track.Image);
    }
}
