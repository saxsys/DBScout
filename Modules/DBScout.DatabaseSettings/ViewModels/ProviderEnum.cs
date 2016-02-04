using System.ComponentModel;

namespace DBScout.DatabaseSettings.ViewModels
{
    public class ProviderEnum : INotifyPropertyChanged
    {
        public enum Providers
        {
            Oracle = 1,
            SqlServer = 2
        }

        /// Need a void constructor in order to use as an object element 
        /// in the XAML.
        public ProviderEnum()
        {
        }

        private ProviderEnum.Providers _providersEnum = ProviderEnum.Providers.Oracle;


        public ProviderEnum.Providers ProvidersEnum
        {
            get { return _providersEnum; }
            set
            {
                if (_providersEnum != value)
                {
                    _providersEnum = value;
                    NotifyPropertyChanged("ProvidersesEnum");
                }
            }
        }

        #region INotifyPropertyChanged Members

        /// Need to implement this interface in order to get data binding
        /// to work properly.
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}