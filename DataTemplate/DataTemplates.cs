using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using AudioPlayer.Models;
using AudioPlayer.ViewModels;
using ReactiveUI;

namespace AudioPlayer.DataTemplate;

public class SongImageTemplate : IDataTemplate
{
    public Control Build(object? param)
    {
        return param != null ? 
            new Image() { Source = (Bitmap?)param, Height = 50, Width = 50, Margin = new Avalonia.Thickness(0) } :
            new Image() { Source = new Bitmap(new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"))), Height = 50, Width = 50, Margin = new Avalonia.Thickness(0) };
    }

    public bool Match(object? data)
    {
        return data is Bitmap;
    }
}

public class AlbumImageTemplate : IDataTemplate
{
    public Control Build(object? param)
    {
        return param != null ? 
            new Image() { Source = (Bitmap?)param, Height = 150, Width = 150, Margin = new Avalonia.Thickness(0) } :
            new Image() { Source = new Bitmap(new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"))), Height = 150, Width = 150, Margin = new Avalonia.Thickness(0) };
    }

    public bool Match(object? data)
    {
        return data is Bitmap;
    }
}

// public class ImageButtonTemplate : IDataTemplate
// {
//     public Control Build(object? param)
//     {
//         return new Button() { Content = param, Padding = Avalonia.Thickness.Parse("0") };
//     }
//
//     public bool Match(object? data)
//     {
//         return data is Image;
//     }
// }

public class CommandButton : IDataTemplate
{
    private readonly ReactiveCommand<Tuple<TrackInfo, ObservableCollection<TrackInfo>>, Unit> _myCommand;

    public CommandButton()
    {
        _myCommand = ReactiveCommand.Create<Tuple<TrackInfo, ObservableCollection<TrackInfo>>, Unit>((obj) =>
        {
            var (track, playlist) = obj;    
            playlist.Remove(track);
            return Unit.Default;
        });
    }

    public Control Build(object? param)
    {
        return new Button()
        {
            Command = _myCommand, CommandParameter = param, Content = "Delete", Padding = Avalonia.Thickness.Parse("1")
        };
    }

    public bool Match(object? data)
    {
        return data is Tuple<TrackInfo, ObservableCollection<TrackInfo>>;
    }
}

public class ImageButtonCommand : IDataTemplate
{
    private readonly ReactiveCommand<Tuple<PlayerViewModel, PlayList, TrackInfo, Control>, Unit> _myCommand;

    public ImageButtonCommand()
    {
        _myCommand = ReactiveCommand.Create<Tuple<PlayerViewModel, PlayList, TrackInfo, Control>, Unit>((obj) =>
        {
            var (player, playlist, track, _) = obj;
            if (player.CurrentPlaylist != playlist)
                player.SetPlaylist(playlist);
            player.Play(track);
            return Unit.Default;
        });
    }

    public Control Build(object? param)
    {
        var (_, _, _, image) = (Tuple<PlayerViewModel, PlayList, TrackInfo, Control>)param;

        // var it = new SongImageTemplate();
        // img = it.Build(bitmap);
        
        return new Button()
        {
            Command = _myCommand, CommandParameter = param, Content = image, Padding = Avalonia.Thickness.Parse("1")
        };
    }

    public bool Match(object? data)
    {
        return data is Tuple<PlayerViewModel, PlayList, TrackInfo, Control>;
    }
}