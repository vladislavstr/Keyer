using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Keyer.Services
{
    public interface IFilesService
    {
        public Task<IStorageFile?> OpenFileAsync();
        public Task<IStorageFile?> SaveFileAsync();
    }
}
