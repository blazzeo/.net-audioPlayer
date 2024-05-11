using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using AudioPlayer.DataTemplate;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using DynamicData;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private PlayList _playList;
    
    private readonly LibraryViewModel _library;
    private string _name = "";
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    private ObservableCollection<TrackInfo> _tracklist;
    private int _idInLibrary = -1;

    private Bitmap _coverImage;
    public Bitmap CoverImage
    {
        get => _coverImage;
        private set => this.RaiseAndSetIfChanged(ref _coverImage, value);
    }

    private FlatTreeDataGridSource<TrackInfo> _audioSorce;
    public FlatTreeDataGridSource<TrackInfo> AudioSource
    {
        get => _audioSorce;
        private set => this.RaiseAndSetIfChanged(ref _audioSorce, value);
    }

    public CreatePlaylistViewModel(LibraryViewModel library)
    {
        _coverImage = DefaultCover();
        _library = library;
        _playList = new PlayList();
        _audioSorce = new FlatTreeDataGridSource<TrackInfo>(_playList.TrackList);
    }

    public CreatePlaylistViewModel(LibraryViewModel library, PlayList playList)
    {
        _idInLibrary = library.Libs.IndexOf(playList);
        Name = playList.Name;
        _tracklist = playList.TrackList;
        _coverImage = playList.Image;
        _library = library;
        _playList = playList;
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(playList.TrackList)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
                new TextColumn<TrackInfo, string>("Album", x => x.Album),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
                new TemplateColumn<TrackInfo>("", new FuncDataTemplate<TrackInfo>((a,e) => GetButton())),
            }
        };
    }
    
    private static Control GetButton()
    {
        var it = new ImageButtonTemplate();
        
        return it.Build(img);
    }

    public async void OpenFolder()
    {
        _tracklist = new ObservableCollection<TrackInfo>();
        var pathFolder = await OpenFolder(CancellationToken.None);

        if (string.IsNullOrWhiteSpace(pathFolder)) return;
        
        var files = Directory.GetFiles(pathFolder);
        
        foreach (var file in files)
        {
            if (!PlayList.IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            _tracklist.Add(track);
        }
        
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_tracklist)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
                new TextColumn<TrackInfo, string>("Album", x => x.Album),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
            }
        };

    }

    public async void OpenImage()
    {
        var pathImage = await OpenFile(CancellationToken.None);
        if (string.IsNullOrWhiteSpace(pathImage)) return;
        if (base.IsImageFile(pathImage))
            CoverImage = new Bitmap(new MemoryStream(await File.ReadAllBytesAsync(pathImage)));
    }

    public async void OpenTrack()
    {
        _tracklist ??= [];
        var pathes = await OpenMultipleFiles(CancellationToken.None);
        if (pathes is null) return;
        
        foreach (var path in pathes)
        {
            if (string.IsNullOrWhiteSpace(path)) continue;
            if (PlayList.IsAudioFile(path))
                _tracklist?.Add(new TrackInfo(path));
        }
        // if (string.IsNullOrWhiteSpace(pathTrack)) return;
        // if (PlayList.IsAudioFile(pathTrack))
        //     _tracklist?.Add(new TrackInfo(pathTrack));
        
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_tracklist)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
                new TextColumn<TrackInfo, string>("Album", x => x.Album),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
            }
        };
    }

    public void Create()
    {
        var newPlaylist = new PlayList(Name, _tracklist, CoverImage);
        if (_idInLibrary != -1)
        {
            _library.UpdatePlaylist(newPlaylist, _idInLibrary);
        }
        else
        {
            _library.AddNewPlaylist(newPlaylist);    
        }
        
    }
    
    public void Cancel()
    {
        _library.Cancel();
    }

    private static Bitmap DefaultCover()
    {
        var memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        return new Bitmap(memory);
    }
}