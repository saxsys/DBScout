using DBScout.Contracts;
using Prism.Mvvm;

namespace DBScout.ViewModels
{
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel
    {
        private string _title = "DBScout";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

   }

    public class MainWindowDesignViewModel:IMainWindowViewModel
    {
        public string Title { get; set; }

        public MainWindowDesignViewModel()
        {
            Title = "DBScout Home";
        }
    }
}
