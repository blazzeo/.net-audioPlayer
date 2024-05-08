using System;
using System.IO;
using System.Threading;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private PlayList _playList;
    private readonly LibraryViewModel _library;
    private string _pathFolder;
    private string _pathImage;
    private string _name;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

    private Bitmap _coverImage;
    public Bitmap CoverImage
    {
        get => _coverImage;
        private set => this.RaiseAndSetIfChanged(ref _coverImage, value);
    }

    private FlatTreeDataGridSource<TrackInfo> _audioSource;
    public FlatTreeDataGridSource<TrackInfo> AudioSource
    {
        get => _audioSource;
        private set => this.RaiseAndSetIfChanged(ref _audioSource, value);
    }

    public CreatePlaylistViewModel(LibraryViewModel library)
    {
        _coverImage = DefaultCover();
        _library = library;
        _playList = new PlayList();
        _audioSource = new FlatTreeDataGridSource<TrackInfo>(_playList.TrackList);
    }

    public CreatePlaylistViewModel(LibraryViewModel library, PlayList playList)
    {
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
            }
        };
    }

    public async void OpenFolder()
    {
        _pathFolder = await OpenFolder(CancellationToken.None);
        _playList = new PlayList(Name, _pathFolder, CoverImage);
        Console.WriteLine(_playList.TrackList.Count);
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_playList.TrackList)
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
        _pathImage = await OpenFile(CancellationToken.None);
        CoverImage = new Bitmap(new MemoryStream(await File.ReadAllBytesAsync(_pathImage)));
    }

    public void Create()
    {
        var newPlaylist = new PlayList(Name, _pathFolder, CoverImage);
        _library.AddNewPlaylist(newPlaylist);
    }

    public void Save()
    {
        
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