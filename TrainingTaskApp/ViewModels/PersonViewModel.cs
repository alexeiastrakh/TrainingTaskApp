using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainingTaskApp.Helpers;
using TrainingTaskApp.Models;
using TrainingTaskApp.Services;
using Windows.UI.Xaml.Controls;

namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People { get; set; }

        public Person NewPerson { get; set; }

        private Person originalPerson;
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public PersonViewModel()
        {
            People = new ObservableCollection<Person>();
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
                    CloseButtonText = "ОК"
                };
            }
            else
            {
                if (People == null)
                    People = new ObservableCollection<Person>();
                People.Add(new Person { FirstName = firstName, LastName = lastName });
                await DataStorageService.SaveData(People);
                NotifyPropertyChanged(nameof(People));

                NewPerson.FirstName = string.Empty;
                NewPerson.LastName = string.Empty;
            }

            if (dialog != null)
                await dialog.ShowAsync();
        }



        public void EditPerson(object parameter)
        {
            Person person = (Person)parameter;
            if (person != null)
            {
                originalPerson = new Person { FirstName = person.FirstName, LastName = person.LastName };
                foreach (var p in People)
                {
                    if (p == person)
                    {
                        p.IsEditing = true;
                        p.ShowEditButtons = false;
                    }
                    else
                    {
                        p.IsEditing = false;
                        p.ShowEditButtons = true;
                    }
                }
            }
        }


        public async Task DeletePerson(object parameter)
        {
            Person person = (Person)parameter;
            if (person != null)
            {
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
                    People.Remove(person);
                    await DataStorageService.SaveData(People);
                }
            }
        }

        public async Task SaveChanges(object parameter)
        {
            Person person = (Person)parameter;
            if (person != null)
            {
                await DataStorageService.SaveData(People);
                NotifyPropertyChanged(nameof(People));
                person.IsEditing = false;
                person.ShowEditButtons = true;
            }
        }

        public async Task CancelChanges(object parameter)
        {
            Person person = (Person)parameter;
            if (person != null && originalPerson != null)
            {
                person.FirstName = originalPerson.FirstName;
                person.LastName = originalPerson.LastName;
                person.IsEditing = false;

                NotifyPropertyChanged(nameof(person.FirstName));
                NotifyPropertyChanged(nameof(person.LastName));
                NotifyPropertyChanged(nameof(person.IsEditing));

                person.ShowEditButtons = true;

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
                    person.IsEditing = false;
                    person.ShowEditButtons = true;
                    People.Add(person);
                }
            }
        }

    }

}
