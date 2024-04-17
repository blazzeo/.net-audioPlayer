using AudioPlayer.Models;
using ReactiveUI;
using ATL;
using CSCore.Codecs;

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
    public Interaction<CreatePlaylistViewModel, AlbumViewModel?> ShowDialog { get; }

    public MainWindowViewModel()
    {
        // ShowDialog = new Interaction<CreatePlaylistViewModel, AlbumViewModel>();
        
        PlayList playlist = new("Orange", "/Users/blazzeo/MF Doom - 2004 - Mm..Food/");
        PlaylistVM = new PlayListViewModel(playlist);
        LibraryVM = new LibraryViewModel(playlist);
        QueueVM = new QueueListViewModel(playlist.TrackList);
        PlayerVM = new PlayerViewModel(new Track("/Users/blazzeo/MF Doom - 2004 - Mm..Food/I Will - Central Cee (320).mp3"));
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

    // private async void LoadAlbums()
    // {
    // var playlist = (await Library.LoadCachedAsync()).Select(x => new LiViewModel(x));
    //
    // foreach (var playlist in playlists)
    // {
    //     Library.Add(playlist);
    // }
    //
    // foreach (var playlist in Library.Playlists)
    // {
    //     await playlist.LoadCover();
    // }
    // }
}
