using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AudioPlayer.Views;

public partial class CreatePlaylist : Window, ICloseable
{
    public CreatePlaylist()
    {
        InitializeComponent();
    }
}