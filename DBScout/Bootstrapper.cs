using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using DBScout.DatabaseSettings;
using DBScout.Views;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using DBScout.Contracts;
using DBScout.StructurePreview;
using DBScout.StructurePreview.Views;

namespace DBScout
{
    class Bootstrapper : UnityBootstrapper
    {
        private IRegionManager _regionManager;

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            var window = (MainWindowView) this.Shell;
            Application.Current.MainWindow = window;

            _regionManager = this.Container.Resolve<IRegionManager>();
           
            window.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new DirectoryModuleCatalog();
            moduleCatalog.ModulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Modules\";
            return moduleCatalog;
        }


        private object CreateViewModel(Type arg)
        {
            return Container.Resolve(arg);
        }


        protected override ILoggerFacade CreateLogger()
        {
            return new Logger();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof (UniformGrid), Container.Resolve<UniformGridRegionAdapter>());
            return mappings;
        }

        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
            //Container.RegisterType<IViewModel, MainWindowViewModel>();
        }
    }
}
