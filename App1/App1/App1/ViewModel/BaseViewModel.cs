using App1.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App1.ViewModel
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        public const string IsBusyPropertyName = "IsBusy";

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value, IsBusyPropertyName); }
        }

        protected void SetProperty<U>(
            ref U something, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(something, value))
                return;

            something = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}