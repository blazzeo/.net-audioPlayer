using System;
using System.Linq;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public static IResourceDictionary LocaleDict { get; set; }
    public bool LocaleEn { get; private set; }

    private ViewModelBase _content = new();

    public ViewModelBase ContentVm
    {
        get => _content;
        private set
        {
            this.RaiseAndSetIfChanged(ref _content, value);
            SearchVm.SearchContent = this;
        }
    }
    
    public SearchViewModel SearchVm { get; set; }
    public PlayerViewModel PlayerVm { get; set; }
    public LibraryViewModel LibraryVm { get; set; }
    public PlayListViewModel PlaylistVm { get; set; }
    public QueueListViewModel QueueVm { get; set; }
    
    public MainWindowViewModel()
    {
        _Locale("ru-RU");
        LocaleEn = false;
        PlayList playlist = new("Orange", "/Users/blazzeo/MF Doom - 2004 - Mm..Food/", "Assets/default-audio.png");
        SearchVm = new SearchViewModel(this);
        PlaylistVm = new PlayListViewModel(this);
        LibraryVm = new LibraryViewModel(playlist, this);
        QueueVm = new QueueListViewModel(this);
        PlayerVm = new PlayerViewModel();
        PlayerVm.SetPlaylist(playlist);
        ContentVm = PlaylistVm;
    }

    public void ToPlaylists()
    {
        if(ContentVm != PlaylistVm)
            ContentVm = PlaylistVm;
    }

    public void OpenCreateDialog()
    {
        if(ContentVm.GetType() != typeof(CreatePlaylistViewModel))
            ContentVm = new CreatePlaylistViewModel(LibraryVm);
    }

    public void OpenEditDialog(PlayList playlistToEdit)
    {
        if(ContentVm.GetType() != typeof(CreatePlaylistViewModel))
            ContentVm = new CreatePlaylistViewModel(LibraryVm, playlistToEdit);
    }

    public void ToLibrary()
    {
        if (ContentVm != LibraryVm)
            ContentVm = LibraryVm;
    }

    public void SwitchLang(string lang)
    {
        LocaleEn = !LocaleEn;
        _Locale(lang);
    }
    
    static void _Locale(string locale) {
        var translations = App.Current?.Resources
            .MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString.Contains("/Lang/") ?? false);

        if (translations != null)
            App.Current!.Resources.MergedDictionaries.Remove(translations);

        var newDictionary = new ResourceInclude(new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml")) {
            Source = new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml")
        };
        App.Current?.Resources.MergedDictionaries.Add(newDictionary);

        LocaleDict = newDictionary.Loaded;
    }
}
