using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DBScout.Contracts;
using DBScout.StructurePreview.Views;
using FakeItEasy.ExtensionSyntax;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace DBScout.DatabaseSettings.ViewModels
{
    public class DbSettingsViewModel : BindableBase //IDbSettingsViewModel, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private string _dbSid;
        private string _exportRootPath;
        private string _exportSchema;
        private string _dbUser;
        private string _dbPassword;
        private string _selectedProvider;

        public DelegateCommand ConnectCommand { get; private set; }
        public string Title { get; set; }

        public string SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }

    


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

        public DbSettingsViewModel(IRegionManager regionManager)
        {
            this.Title = "DBScout Settings";
            _regionManager = regionManager;

            ConnectCommand = new DelegateCommand(ShowPreview, CanConnect);
        }

        private bool CanConnect()
        {
            var connectable = !string.IsNullOrEmpty(this.DbUser) && !string.IsNullOrEmpty(this.DbPassword);// &&!string.IsNullOrEmpty(this.DbProvider);
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