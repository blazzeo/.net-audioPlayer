using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Controls.Templates;

namespace AudioPlayer.Templates;

public class SongImageTemplate : IDataTemplate
{
    public Control Build(object? param)
    {
        return new Image() { Source = (Bitmap?)param, Height = 50, Width = 50, Margin = new Avalonia.Thickness(5) };
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
        return new Image() { Source = (Bitmap?)param, Height = 150, Width = 150, Margin = new Avalonia.Thickness(20) };
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
        return new Button() { Content = param, };
    }

    public bool Match(object? data)
    {
        return data is Image;
    }
}
