using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TrainingTaskApp;

namespace TrainingTaskApp
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoToPersonPage_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;

            if (frame != null)
            {
                frame.Navigate(typeof(PersonPage));
            }
        }
    }
}
