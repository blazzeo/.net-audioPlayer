using AudioPlayer.Models;
using Avalonia.Media.Imaging;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private PlayList _playList;
    private LibraryViewModel _library;
    public string Name { get; }
    public Bitmap CoverImage { get; }

    public CreatePlaylistViewModel(LibraryViewModel Library)
    {
        _library = Library;
        _playList = new PlayList();
    }

    public void OpenFolder()
    {
        
    }

    private void OpenDialog()
    {
        
    }

    public void Create()
    {
        
    }
}