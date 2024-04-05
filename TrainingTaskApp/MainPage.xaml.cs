using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TrainingTaskApp;
using TrainingTaskApp.Models;
using TrainingTaskApp.ViewModels;

namespace TrainingTaskApp
{
    public partial class MainPage : Page
    {
        private PersonViewModel viewModel;
        public MainPage()
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

            selectedPerson.FirstName = FirstNameTextBox.Text;
            selectedPerson.LastName = LastNameTextBox.Text;

            viewModel.EditPerson(selectedPerson, selectedPerson.FirstName, selectedPerson.LastName);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPerson = ((Button)sender).DataContext as Person;

            viewModel.DeletePerson(selectedPerson);
        }
    }
}
