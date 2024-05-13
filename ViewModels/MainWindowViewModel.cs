using System;
using System.Linq;
using AudioPlayer.Models;
using Avalonia.Markup.Xaml.Styling;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public bool LocaleEn { get; private set; }

    private ViewModelBase _content = new();

    public ViewModelBase ContentVm
    {
        get => _content;
        private set
        {
            this.RaiseAndSetIfChanged(ref _content, value);
            SearchVm.SearchContent = ContentVm;
        }
    }

    public SearchViewModel SearchVm { get; set; }
    public PlayerViewModel PlayerVm { get; set; }
    public LibraryViewModel LibraryVm { get; }
    public PlayListViewModel PlaylistVm { get; set; }

    public MainWindowViewModel()
    {
        _Locale("ru-RU");
        LocaleEn = false;
        PlayList playlist = new("Orange", "/Users/blazzeo/Orange/", "/Users/blazzeo/Downloads/soprano.jpeg");
        SearchVm = new SearchViewModel(this);
        PlaylistVm = new PlayListViewModel(this);
        LibraryVm = new LibraryViewModel(playlist, this);
        PlayerVm = new PlayerViewModel();
        ContentVm = LibraryVm;
    }
    
    public void OpenCreateDialog()
    {
        if (ContentVm.GetType() != typeof(CreatePlaylistViewModel))
            ContentVm = new CreatePlaylistViewModel(LibraryVm);
    }

    public void OpenEditDialog(PlayList playlistToEdit)
    {
        if (ContentVm.GetType() != typeof(CreatePlaylistViewModel))
            ContentVm = new CreatePlaylistViewModel(LibraryVm, playlistToEdit);
    }

    public void ToPlaylists()
    {
            ContentVm = PlaylistVm;
    }
    
    public void ToLibrary()
    {
            ContentVm = LibraryVm;
    }

    public void SwitchLang(string lang)
    {
        LocaleEn = !LocaleEn;
        _Locale(lang);
    }

    static void _Locale(string locale)
    {
        var translations = App.Current?.Resources
            .MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString.Contains("/Lang/") ?? false);

        if (translations != null)
            App.Current!.Resources.MergedDictionaries.Remove(translations);

        var newDictionary = new ResourceInclude(new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml"))
        {
            Source = new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml")
        };
        App.Current?.Resources.MergedDictionaries.Add(newDictionary);
    }
}
