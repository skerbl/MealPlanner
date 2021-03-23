using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace MPData
{
    [AddINotifyPropertyChangedInterface]
    public class BaseNotificationClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
