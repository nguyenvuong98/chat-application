using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SocketIO.ViewModel
{
    public class BaseModel : INotifyPropertyChanged
    {
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set {
                if(value != _isBusy)
                {
                    _isBusy = value;
                    RaisePropertyChanged("IsBusy");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
