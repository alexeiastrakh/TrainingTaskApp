using Windows.UI.Xaml.Controls;
using TrainingTaskApp.ViewModels;
using Windows.UI.Xaml;

namespace TrainingTaskApp
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = Application.Current.Resources["PersonViewModel"];
        }
    }
}
