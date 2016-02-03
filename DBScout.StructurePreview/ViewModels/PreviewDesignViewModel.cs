namespace DBScout.StructurePreview.ViewModels
{
    public class PreviewDesignViewModel:IPreviewViewModel
    {
        public string Text { get; set; }

        public PreviewDesignViewModel()
        {
            Text = "PreviewDesign";
        }
    }
}