using Windows.UI.Xaml.Controls;
using TrainingTaskApp.ViewModels;

namespace TrainingTaskApp
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new PersonViewModel();
        }
    }
}
