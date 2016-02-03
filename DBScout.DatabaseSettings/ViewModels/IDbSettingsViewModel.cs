using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace DBScout.DatabaseSettings.ViewModels
{
    public interface IDbSettingsViewModel
    {
        string Title { get; set; }
        DelegateCommand ConnectCommand { get;  }
        string DbSid { get; set; }
        string DbUser { get; set; }
        string DbPassword { get; set; }
        string ExportSchema { get; set; }
        string ExportRootPath { get; set; }
        StringDictionary DbProvider { get; set; }
        string GetConnectionString();
    }
}