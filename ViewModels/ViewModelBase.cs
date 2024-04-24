using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public partial class ViewModelBase : ReactiveObject
{
    [RelayCommand]
    protected async Task<string> OpenFile(CancellationToken token)
    {
        try
        {
            var file = await DoOpenFilePickerAsync();
            return file is null ? null : file.Path.ToString();
        }
        catch (Exception e)
        {
            //Console.Error(e.Message);
        }

        return null;
    }

    [RelayCommand]
    protected async Task<string> OpenFolder(CancellationToken token)
    {
        try
        {
            var folder = await DoOpenFolderPickerAsync();
            return folder is null ? null : folder.Path.ToString();
        }
        catch (Exception e)
        {
            
        }

        return null;
    }
    
    private async Task<IStorageFile?> DoOpenFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        return files?.Count >= 1 ? files[0] : null;
    }
    
    private async Task<IStorageFolder?> DoOpenFolderPickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false
        });

        return files?.Count >= 1 ? files[0] : null;
    }
}
