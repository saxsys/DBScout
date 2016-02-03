using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DBScout.StructurePreview.ViewModels
{
    public class PreviewViewModel : BindableBase, IPreviewViewModel
    {
        //private readonly IRegionManager _regionManager;
        public string Text { get; set; }

        public PreviewViewModel(IRegionManager regionManager)
        {
            this.Text = "Preview";
            //_regionManager = regionManager;
        }
    }
}
