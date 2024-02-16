using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Keyer.Services
{
    public class FilesService : IFilesService
    {
        private readonly Window _target;
        //private readonly FilePickerFileType _fileType;

        public FilesService(Window target)/*, FilePickerFileType fileType)*/
        {
            _target = target;
            //_fileType = fileType;
        }

        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open PNG File",
                AllowMultiple = false,
                //FileTypeFilter = new[] { _fileType, FilePickerFileTypes.TextPlain }
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