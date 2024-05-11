using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;

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

public class ImageButtonTemplate : IDataTemplate
{
    public Control Build(object? param)
    {
        return new Button() { Content = param, Padding = Avalonia.Thickness.Parse("0") };
    }

    public bool Match(object? data)
    {
        return data is Image;
    }
}

public class CommandButton : IDataTemplate
{
    public Control Build(object? param)
    {
        return new Button()
        {
            Command = param, CommandParameter = this, Content = "Delete", Padding = Avalonia.Thickness.Parse("1")
        };
    }

    public bool Match(object? data)
    {
        return data is ();
    }
}
