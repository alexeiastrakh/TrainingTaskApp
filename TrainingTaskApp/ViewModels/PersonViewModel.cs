using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainingTaskApp.Helpers;
using TrainingTaskApp.Infrastructure.Command;
using TrainingTaskApp.Models;
using TrainingTaskApp.Services;
using Windows.UI.Xaml.Controls;

namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel : ObservableObject
    {
        public ObservableCollection<PersonUIWrapper> People { get; set; }

        public Person NewPerson { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public PersonViewModel()
        {
            People = new ObservableCollection<PersonUIWrapper>();
            NewPerson = new Person();
            LoadData();

            AddCommand = new RelayCommand(param => AddPerson(NewPerson.FirstName, NewPerson.LastName));
            EditCommand = new RelayCommand(EditPerson);
            DeleteCommand = new RelayCommand(async param => await DeletePerson(param));
            SaveCommand = new RelayCommand(async param => await SaveChanges(param));
            CancelCommand = new RelayCommand(param => CancelChanges(param));
        }

        public async void AddPerson(string firstName, string lastName)
        {
            ContentDialog dialog = null;
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "FirstName and LastName couldn't be empty",
                    CloseButtonText = "OK"
                };
            }
            else
            {
                if (People == null)
                    People = new ObservableCollection<PersonUIWrapper>();
                var newPerson = new Person { FirstName = firstName, LastName = lastName };
                People.Add(new PersonUIWrapper { Person = newPerson });
                var people = new ObservableCollection<Person>(People.Select(p => p.Person));
                await DataStorageService.SaveData(people);

                NewPerson.FirstName = string.Empty;
                NewPerson.LastName = string.Empty;
            }

            if (dialog != null)
                await dialog.ShowAsync();
        }

        public void EditPerson(object parameter)
        {
            PersonUIWrapper personWrapper = (PersonUIWrapper)parameter;
            if (personWrapper != null)
            {
                personWrapper.BeginEdit();
            }
        }

        public async Task DeletePerson(object parameter)
        {
            PersonUIWrapper personWrapper = (PersonUIWrapper)parameter;
            if (personWrapper != null)
            {
                if (personWrapper.IsEditing)
                    personWrapper.EndEdit();
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Confirm Delete",
                    Content = "Are you sure you want to delete this person?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No"
                };

                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    People.Remove(personWrapper);
                    var people = new ObservableCollection<Person>(People.Select(p => p.Person));
                    await DataStorageService.SaveData(people);
                }
            }
        }


        public async Task SaveChanges(object parameter)
        {
            PersonUIWrapper personWrapper = (PersonUIWrapper)parameter;
            if (personWrapper != null)
            {
                personWrapper.EndEdit();

                var people = new ObservableCollection<Person>(People.Select(p => p.Person));
                await DataStorageService.SaveData(people);

            }
        }

        public async Task CancelChanges(object parameter)
        {
            PersonUIWrapper personWrapper = (PersonUIWrapper)parameter;
            if (personWrapper != null)
            {
                personWrapper.CancelEdit();

                ContentDialog cancelDialog = new ContentDialog
                {
                    Title = "Changes Cancelled",
                    Content = "Your changes have been cancelled.",
                    CloseButtonText = "OK"
                };

                await cancelDialog.ShowAsync();
            }
        }


        public async Task LoadData()
        {
            var loadedPeople = await DataStorageService.LoadData();
            if (loadedPeople != null)
            {
                foreach (var person in loadedPeople)
                {
                    PersonUIWrapper personWrapper = new PersonUIWrapper
                    {
                        Person = person,
                        IsEditing = false,
                        ShowEditButtons = true
                    };
                    People.Add(personWrapper);
                }
            }
        }
    }
}
