using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace DBScout.DatabaseSettings.ViewModels
{
    public class IDbSettingsDesignViewModel : IDbSettingsViewModel
    {
        public string Title { get; set; }
 
        public DelegateCommand ConnectCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DbSid { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string ExportSchema { get; set; }
        public string ExportRootPath { get; set; }
        public StringDictionary DbProvider { get; set; }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public IDbSettingsDesignViewModel()
        {
            this.Title = "DBScout Settings";
        }
    }
}