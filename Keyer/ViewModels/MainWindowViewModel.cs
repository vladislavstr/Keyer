using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyer.Helpers;
using Keyer.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Keyer.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Bitmap? ImageFromBinding { get; } = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-error.png"));

        [ObservableProperty] private Bitmap? _file;

        [RelayCommand]
        private async Task OpenFile(CancellationToken token)
        {
            ErrorMessages?.Clear();
            try
            {
                var filesService = App.Current?.Services?.GetService<IFilesService>();
                if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

                var file = await filesService.OpenFileAsync();
                if (file is null) return;

                /// <summary>
                /// Limited the file size to 1 MB to avoid unnecessary complications.
                /// </summary>
                if ((await file.GetBasicPropertiesAsync()).Size <= 1024 * 1024 * 1)
                {
                    await using var readStream = await file.OpenReadAsync();
                    File = new Bitmap(readStream);
                }
                else
                {
                    throw new Exception("File exceeded 1MB limit.");
                }
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }
    }
}
