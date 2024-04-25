using System;
using System.Linq;
using AudioPlayer.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static IResourceDictionary LocaleDict { get; set; }
    public bool LocaleEn { get; private set; }

    private ViewModelBase _content;

    public ViewModelBase ContentVM
    {
        get => _content;
        private set
        {
            this.RaiseAndSetIfChanged(ref _content, value);
            SearchVM.SearchContent = this;
        }
    }
    
    public SearchViewModel SearchVM { get; set; } 
    public PlayerViewModel PlayerVM { get; set; }
    public LibraryViewModel LibraryVM { get; set; }
    public PlayListViewModel PlaylistVM { get; }
    public QueueListViewModel QueueVM { get; set; }
    
    public MainWindowViewModel()
    {
        _Locale("ru-RU");
        LocaleEn = false;
        PlayList playlist = new("Orange", "/Users/blazzeo/MF Doom - 2004 - Mm..Food/");
        SearchVM = new SearchViewModel(this);
        PlaylistVM = new PlayListViewModel(playlist);
        LibraryVM = new LibraryViewModel(playlist, this);
        QueueVM = new QueueListViewModel(playlist.TrackList);
        PlayerVM = new PlayerViewModel(playlist.TrackList[0]);
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

    public void SwitchLang(string lang)
    {
        LocaleEn = !LocaleEn;
        _Locale(lang);
    }
    
    static void _Locale(string locale) {
        var translations = App.Current?.Resources
            .MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString?.Contains("/Lang/") ?? false);

        if (translations != null)
            App.Current!.Resources.MergedDictionaries.Remove(translations);

        // var resource = AssetLoader.Open(new Uri($"avares://LocalizationSample/Assets/Lang/{targetLanguage}.axaml"));

        var newDictionary = new ResourceInclude(new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml")) {
            Source = new Uri($"avares://AudioPlayer/Assets/Lang/{locale}.axaml")
        };
        App.Current?.Resources.MergedDictionaries.Add(newDictionary);

        LocaleDict = newDictionary.Loaded;
    }
}
