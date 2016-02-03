using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using DBScout.DatabaseSettings.Properties;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace DBScout.DatabaseSettings.ViewModels
{
    public class DbSettingsDesignViewModel : IDbSettingsViewModel
    {
        public string Title { get; set; }

        public DelegateCommand ConnectCommand
        {
            get { throw new NotImplementedException(); }
        }

        public string DbSid { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string ExportSchema { get; set; }
        public string ExportRootPath { get; set; }
        public ProviderEnum SelectedProvider { get; set; }
        public List<ComboBoxItemProvider> ProviderListEnum { get; set; }

        public DbSettingsDesignViewModel()
        {
            this.Title = Resources.StrDBScout_Settings;
        }
    }
}