using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Keyer.Helpers;

namespace Keyer.Services
{
    public class FilesService : IFilesService
    {
        private readonly Window _target;

        public FilesService(Window target)
        {
            _target = target;
        }

        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open PNG File",
                AllowMultiple = false,
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IStorageFile?> SaveFileAsync()
        {
            return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save PNG File",
                DefaultExtension = "png",
                ShowOverwritePrompt = true
            });
        }
    }
}
