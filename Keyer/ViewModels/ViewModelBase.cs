using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Keyer.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        protected ViewModelBase()
        {
            ErrorMessages = new ObservableCollection<string>();
        }

        [ObservableProperty]
        private ObservableCollection<string>? _errorMessages;
    }
}
