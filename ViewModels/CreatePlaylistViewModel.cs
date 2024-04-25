using System.IO;
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
        CoverImage = DefaultCover();
        _library = Library;
        _playList = new PlayList();
    }

    public void OpenFolder()
    {
        
    }

    private void OpenDialog()
    {
        // string str = OpenFile();
    }

    public void Create()
    {
        
    }

    private Bitmap DefaultCover()
    {
        
            MemoryStream memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
            return new Bitmap(memory);
        
    }
}