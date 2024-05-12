using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AudioPlayer.ViewModels;
using Avalonia.VisualTree;
using ReactiveUI;
using Microsoft.Xaml.Behaviors;

namespace AudioPlayer.Views;

public partial class QueueView : UserControl
{
    public QueueView()
    {
        InitializeComponent();
        // var tmp = new QueueListViewModel(this.FindAncestorOfType<MainWindowViewModel>());
        // DataContext = tmp;
        // this.ListOfTracks.ItemsPanelRoot.DoubleTapped += tmp.PlayNow;
    }
}