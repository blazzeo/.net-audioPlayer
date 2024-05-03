using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private PlayList _playList;
    private LibraryViewModel _library;
    private string pathFolder;
    private string pathImage;
    public string Name { get; }
    public Bitmap CoverImage { get; private set; }
    public FlatTreeDataGridSource<TrackInfo> AudioSource { get; }
    
    public new event PropertyChangedEventHandler? PropertyChanged;

    public CreatePlaylistViewModel(LibraryViewModel Library)
    {
        CoverImage = DefaultCover();
        _library = Library;
        _playList = new PlayList();
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_playList.TrackList)
        {
            Columns = {
                new TextColumn<TrackInfo, string>("Title", x => x.Title),
                new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
                new TextColumn<TrackInfo, string>("Album", x => x.Album),
                new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
            }
        };
        // this.CloseWindowCommand = new RelayCommand<ICloseable>(this.CloseWindow);
    }

    public async void OpenFolder()
    {
        pathFolder = OpenFolder(CancellationToken.None).Result;
        Console.WriteLine(11);
    }

    private async void OpenImage()
    {
        pathImage = OpenFile(CancellationToken.None).Result;
        Console.WriteLine(pathImage);
        CoverImage = new Bitmap(new MemoryStream(File.ReadAllBytes(pathImage)));
    }

    public void Create()
    {
        var newPlaylist = new PlayList(Name, pathFolder, pathImage);
        _library.AddNewPlaylist(newPlaylist);
    }
    
    public void Cancel()
    {
        _library.Cancel();
    }

    private Bitmap DefaultCover()
    {
        
            MemoryStream memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
            return new Bitmap(memory);
        
    }
}