using AudioPlayer.Models;
using ReactiveUI;

using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using Avalonia.Controls;

namespace AudioPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _content;

    public ViewModelBase ContentVM
    {
        get => _content;
        private set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    
    public SearchViewModel SearchVM { get; set; } 
    public PlayerViewModel PlayerVM { get; set; }
    public LibraryViewModel LibraryVM { get; set; }
    public PlayListViewModel PlaylistVM { get; }
    public QueueListViewModel QueueVM { get; set; }
    
    public MainWindowViewModel()
    {
        PlayList playlist = new("Orange", "/Users/blazzeo/MF Doom - 2004 - Mm..Food/");
        PlaylistVM = new PlayListViewModel(playlist);
        LibraryVM = new LibraryViewModel(playlist);
        QueueVM = new QueueListViewModel(playlist.TrackList);
        PlayerVM = new PlayerViewModel();
        ContentVM = PlaylistVM;
    }

    public void ToPlaylists()
    {
        if(ContentVM != PlaylistVM)
            ContentVM = PlaylistVM;
    }

    public void ToLibrary()
    {
        if (ContentVM != LibraryVM)
            ContentVM = LibraryVM;
    }
    
}
