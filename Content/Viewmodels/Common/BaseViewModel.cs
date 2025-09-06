using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wwrc_maui.Content.Viewmodels.Common
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region bindables properties
        #region beans
        bool _isBusy = false;
        bool _isEmpty = false;
        bool _isRefreshing = false;
        bool _isPullRefreshing = false;
        bool _enableRefreshing = true;
        bool _isFetching = false;
        string _title = string.Empty;
        #endregion
        #region props
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { IsPullRefreshing = value; SetProperty(ref _isRefreshing, value); }
        }
        public bool IsPullRefreshing
        {
            get { return _isPullRefreshing; }
            set { EnableRefreshing = value; SetProperty(ref _isPullRefreshing, value); }
        }
        public bool EnableRefreshing
        {
            get { return _enableRefreshing; }
            set { SetProperty(ref _enableRefreshing, value); }
        }
        public bool IsFetching
        {
            get { return _isFetching; }
            set { SetProperty(ref _isFetching, value); }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion
        #endregion

        public int page = 1;
        public int totalPageCount = 0;

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null) return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
