using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DBScout.Contracts;
using DBScout.DatabaseSettings.Properties;
using DBScout.StructurePreview.Views;
using FakeItEasy.ExtensionSyntax;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace DBScout.DatabaseSettings.ViewModels
{
    public class DbSettingsViewModel : BindableBase, IDbSettingsViewModel, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        #region Properties

        private string _dbSid;
        private string _exportRootPath;
        private string _exportSchema;
        private string _dbUser;
        private string _dbPassword;
        private ProviderEnum _selectedProvider = new ProviderEnum();

        public DelegateCommand ConnectCommand { get; private set; }
        public string Title { get; set; }

        public ProviderEnum SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }
        public List<ComboBoxItemProvider> ProviderListEnum { get; set; }

        public string DbSid
        {
            get { return _dbSid; }
            set
            {
                _dbSid = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        public string DbUser
        {
            get { return _dbUser; }
            set
            {
                _dbUser = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        public string DbPassword
        {
            get { return _dbPassword; }
            set
            {
                _dbPassword = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        public string ExportSchema
        {
            get { return _exportSchema; }
            set
            {
                _exportSchema = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        public string ExportRootPath
        {
            get { return _exportRootPath; }
            set
            {
                _exportRootPath = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion //Properties

        public DbSettingsViewModel(IRegionManager regionManager)
        {
            this.Title = Resources.StrDBScout_Settings;
            _regionManager = regionManager;
            ProviderListEnum = new List<ComboBoxItemProvider>()
                {
                    new ComboBoxItemProvider()
        {
            ValueProvidersEnum = ProviderEnum.Providers.Oracle,
                ValueProviderString = Resources.StrOracle},
                    new ComboBoxItemProvider()
        {
            ValueProvidersEnum = ProviderEnum.Providers.SqlServer,
                ValueProviderString = Resources.StrMS_SqlServer },
                    new ComboBoxItemProvider()
      
                };
            ConnectCommand = new DelegateCommand(ShowPreview, CanConnect);
        }

        private bool CanConnect()
        {
            var connectable = !string.IsNullOrEmpty(this.DbUser) && !string.IsNullOrEmpty(this.DbPassword);
                // &&!string.IsNullOrEmpty(this.DbProvider);
            return connectable;
        }

        private void ShowPreview()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegionName, typeof (PreviewView).ToString());
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public string GetConnectionString()
        {
            var connectionStringBuilder = new OleDbConnectionStringBuilder
            {
                DataSource = this.DbSid,
                // Provider = this.DbProvider,
                ["User ID"] = this.DbUser,
                ["Password"] = this.DbPassword
            };
            return connectionStringBuilder.ConnectionString;
        }

       
}
}