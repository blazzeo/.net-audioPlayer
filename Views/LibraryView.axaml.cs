using System.Threading.Tasks;
using AudioPlayer.Models;
using AudioPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AudioPlayer.Views;

public partial class LibraryView : ReactiveUserControl<LibraryViewModel>
{
    public LibraryView()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    
    private async Task DoShowDialogAsync(InteractionContext<CreatePlaylistViewModel, PlayList?> interaction)
    {
        var dialog = new CreatePlaylist
        {
            DataContext = interaction.Input
        };

        var window = Window.GetTopLevel(this) as Window;
        
        var result = await dialog.ShowDialog<PlayList?>(window);
        interaction.SetOutput(result);
    }
}