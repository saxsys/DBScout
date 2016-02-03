using Prism.Modularity;
using Prism.Regions;
using System;
using System.ComponentModel;
using DBScout.Contracts;
using DBScout.DatabaseSettings.Views;
using Microsoft.Practices.Unity;

namespace DBScout.DatabaseSettings
{
    [Module(ModuleName = "DatabaseSettingsModule")]
    public class DatabaseSettingsModule : IModule
    {
        readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public DatabaseSettingsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, DbSettingsView>(typeof (DbSettingsView).Name);
            //_regionManager.RequestNavigate(RegionNames.ContentRegionName, typeof(DbSettingsView).Name);
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegionName, typeof(DbSettingsView));
        }
    }
}