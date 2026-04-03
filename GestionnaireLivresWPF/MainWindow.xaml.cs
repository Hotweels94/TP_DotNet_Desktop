using System.Windows;
using GestionnaireLivresWPF.ViewModels;

namespace GestionnaireLivresWPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}