using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SocketIO.ViewModel
{
    public abstract partial class BaseViewModel: FreshMvvm.FreshBasePageModel 
    {
        public bool IsBusy { get; set; }
        public int PAGE_COUNT { get; set; } = 20;

        public void RaisePropertyChanged(params string[] propertyNames)
        {
            for (int i = 0; i < propertyNames.Length; i++)
            {
                base.RaisePropertyChanged(propertyNames[i]);
            }
        }
    }
}
