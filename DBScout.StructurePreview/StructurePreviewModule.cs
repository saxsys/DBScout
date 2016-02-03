using DBScout.Contracts;
using Prism.Modularity;
using Prism.Regions;
using DBScout.StructurePreview.Views;
using Microsoft.Practices.Unity;

namespace DBScout.StructurePreview
{
    [Module(ModuleName = "StructurePreviewModule")]
    public class StructurePreviewModule : IModule
    {
        readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public StructurePreviewModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, PreviewView>(typeof (PreviewView).ToString());
            //_regionManager.RegisterViewWithRegion(RegionNames.ContentRegionName, typeof(PreviewView));
        }
    }
}