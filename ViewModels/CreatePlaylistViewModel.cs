using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using AudioPlayer.DataTemplate;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private readonly LibraryViewModel _library;
    private string _name = "";
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    private ObservableCollection<TrackInfo> _trackList;
    private readonly int _idInLibrary = -1;

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
        var playList = new PlayList();
        _audioSource = new FlatTreeDataGridSource<TrackInfo>(playList.TrackList);
    }
    
    public CreatePlaylistViewModel(LibraryViewModel library, PlayList playList)
    {
        _idInLibrary = library.Libs.IndexOf(playList);
        Name = playList.Name!;
        _trackList = playList.TrackList;
        _coverImage = playList.Image;
        _library = library;
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(playList.TrackList)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title, GridLength.Parse("210")),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist, GridLength.Parse("170")),
                new TextColumn<TrackInfo, string>("Album", x => x.Album, GridLength.Parse("170")),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
                new TemplateColumn<TrackInfo>("", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(a))),
            }
        };
    }
    
    public async void OpenFolder()
    {
        _trackList = new ObservableCollection<TrackInfo>();
        var pathFolder = await OpenFolder(CancellationToken.None);

        if (string.IsNullOrWhiteSpace(pathFolder)) return;
        
        var files = Directory.GetFiles(pathFolder);
        
        foreach (var file in files)
        {
            if (!PlayList.IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            _trackList.Add(track);
        }
        
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title, GridLength.Parse("210")),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist, GridLength.Parse("170")),
                new TextColumn<TrackInfo, string>("Album", x => x.Album, GridLength.Parse("170")),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
                new TemplateColumn<TrackInfo>("", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(a))),
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
        _trackList ??= [];
        var paths = await OpenMultipleFiles(CancellationToken.None);

        foreach (var path in paths.Where(path => !string.IsNullOrWhiteSpace(path)).Where(PlayList.IsAudioFile))
        {
            _trackList?.Add(new TrackInfo(path));
        }

        if (_trackList != null)
            AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList)
            {
                Columns =
                {
                    new TextColumn<TrackInfo, string>("Title", x => x.Title),
                    new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
                    new TextColumn<TrackInfo, string>("Album", x => x.Album),
                    new TextColumn<TrackInfo, string>("Duration",
                        x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
                    new TemplateColumn<TrackInfo>("", new FuncDataTemplate<TrackInfo>((a, e) => GetButton(a))),
                }
            };
    }

    public void Create()
    {
        var newPlaylist = new PlayList(Name, _trackList, CoverImage);
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
    
    private Control GetButton(TrackInfo track)
    {
        var it = new CommandButton();
        var tuple = new Tuple<TrackInfo, ObservableCollection<TrackInfo>>(track, _trackList);
        return it.Build(tuple);
    }
}