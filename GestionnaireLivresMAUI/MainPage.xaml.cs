using GestionnaireLivresMAUI.ViewModels;

namespace GestionnaireLivresMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}