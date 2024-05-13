using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AudioPlayer.Models;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls.Shapes;
using DynamicData;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public partial class ViewModelBase : ReactiveObject
{
    [RelayCommand]
    public async Task<string?> OpenFile(CancellationToken token)
    {
        try
        {
            var file = await DoOpenFilePickerAsync();
            return file?.TryGetLocalPath();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
    
    [RelayCommand]
    public async Task<List<string?>> OpenMultipleFiles(CancellationToken token)
    {
        try
        {
            var files = await DoOpenFilesPickerAsync();
            return files.Select(file => file?.TryGetLocalPath()).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    [RelayCommand]
    public async Task<string?> OpenFolder(CancellationToken token)
    {
        try
        {
            var folder = await DoOpenFolderPickerAsync();
            return folder?.TryGetLocalPath();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
            Title = "Open File",
            AllowMultiple = false
        });

        return files?.Count >= 1 ? files[0] : null;
    }
    
    private async Task<IStorageFile[]> DoOpenFilesPickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = true
        });

        return files?.Count >= 1 ? files.ToArray() : null;
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

    public bool IsImageFile(string path)
    {
        HashSet<string> imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpeg",
            ".png",
            ".jpg"
        };

        var extension = System.IO.Path.GetExtension(path);

        return imageExtensions.Contains(extension);
    }
}
