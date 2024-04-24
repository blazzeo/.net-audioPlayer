using Avalonia.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Platform.Storage;

namespace AudioPlayer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GetPath();
    }
    
    public async Task<string> GetPath()
    {
        var storage = this.StorageProvider;
        Task<IReadOnlyList<IStorageFile>> files = storage.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "New file",
            AllowMultiple = false
        });
        
        return files.Result[0].Path.ToString();
    }
}
