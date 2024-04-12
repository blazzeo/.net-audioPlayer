using AudioPlayer.Models;
using ATL;

namespace AudioPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase ContentVM;
    public PlayerViewModel PlayerVM { get; set; }
    public LibraryViewModel LibraryVM { get; set; }
    public PlayListViewModel PlaylistVM { get; set; }

    public MainWindowViewModel()
    {
        PlayList playlist = new("Orange", "/Users/blazzeo/Orange/");
        PlaylistVM = new(playlist);
        LibraryVM = new(playlist);
        PlayerVM = new(new Track("/Users/blazzeo/Orange/Pink Matters - Frank Ocean.wav"));
        ContentVM = PlaylistVM;
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
