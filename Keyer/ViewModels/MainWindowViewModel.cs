using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Keyer.Helpers;
using Keyer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Bitmap = Avalonia.Media.Imaging.Bitmap;


namespace Keyer.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Bitmap? ImageFromBinding { get; } = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-error.png"));

        [ObservableProperty] private Bitmap? _loadedFile = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-in.png"));
        [ObservableProperty] private string _loadedFilePath;
        [ObservableProperty] private Bitmap? _modifiedFile = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-out.png"));

        [RelayCommand]
        private async Task OpenFile()
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
                    LoadedFilePath = file.Path.LocalPath;
                    LoadedFile = new Bitmap(readStream);
                }
                else
                {
                    LoadedFile = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-error.png"));
                    throw new Exception("File exceeded 1MB limit.");
                }
            }
            catch (Exception e)
            {
                LoadedFile = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-error.png"));
                ErrorMessages?.Add(e.Message);
            }
        }

        [RelayCommand]
        private async Task EditFile()
        {
            try
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(LoadedFilePath);

                System.Drawing.Bitmap resultBitmap = new System.Drawing.Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

                for (var x = 0; x < bitmap.Width; x++)
                {
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        System.Drawing.Color camColor = bitmap.GetPixel(x, y);

                        byte max = Math.Max(Math.Max(camColor.R, camColor.G), camColor.B);
                        byte min = Math.Min(Math.Min(camColor.R, camColor.G), camColor.B);

                        bool replace = camColor.G != min && (camColor.G == max || max - camColor.G < 8) && (max - min) > 96;

                        if (replace)
                        {
                            camColor = System.Drawing.Color.FromArgb(0, 255, 255, 255);
                        }

                        resultBitmap.SetPixel(x, y, camColor);
                    }
                }

                /// <summary>
                /// create Avalonia.Media.Imaging.Bitmap from System.Drawing.Bitmap.
                /// </summary>
                await using (MemoryStream memory = new MemoryStream())
                {
                    resultBitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    ModifiedFile = new Avalonia.Media.Imaging.Bitmap(memory);
                }
            }
            catch (Exception e)
            {
                LoadedFile = ImageHelper.LoadFromResource(new("avares://Keyer/Assets/auto-insert-error.png"));
                ErrorMessages?.Add(e.Message);
            }
        }
    }
}
