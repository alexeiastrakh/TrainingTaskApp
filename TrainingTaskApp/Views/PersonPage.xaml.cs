using System;
using TrainingTaskApp.Models;
using TrainingTaskApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrainingTaskApp
{
    public sealed partial class PersonPage : Page
    {
        private PersonViewModel viewModel;

        public PersonPage()
        {
            InitializeComponent();
            viewModel = new PersonViewModel();
            DataContext = viewModel;
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            viewModel.AddPerson(firstName, lastName);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedPerson = ((Button)sender).DataContext as Person;

            viewModel.EditPerson(selectedPerson, "NewFirstName", "NewLastName");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPerson = ((Button)sender).DataContext as Person;

            viewModel.DeletePerson(selectedPerson);
        }
    }
}
