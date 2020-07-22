using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SocketIO.BaseModel
{
    public abstract partial class ViewModelBase : FreshMvvm.FreshBasePageModel
    {
        public bool IsBusy { get; set; }
        public int PAGE_COUNT { get; set; } = 20;

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            for (int i = 0; i < propertyNames.Length; i++)
            {
                base.RaisePropertyChanged(propertyNames[i]);
            }
        }
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
