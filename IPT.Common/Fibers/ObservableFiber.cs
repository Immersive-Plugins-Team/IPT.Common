using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IPT.Common.Fibers;

namespace IPT.Common.Models
{
    /// <summary>
    /// A base class for models that implements the INotifyPropertyChanged interface.
    /// </summary>
    public abstract class ObservableFiber : GenericFiber, INotifyPropertyChanged
    {
        protected ObservableFiber(string name, int interval) : base(name, interval) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
