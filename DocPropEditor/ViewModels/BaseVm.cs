using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocPropEditor.ViewModels
{
    internal class BaseVm:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
