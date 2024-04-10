using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainingTaskApp.Models;
using TrainingTaskApp.Services;
using Windows.UI.Xaml.Controls;

namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
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
            EditCommand = new RelayCommand(param => EditPerson(param as Person));
            DeleteCommand = new RelayCommand(async param => await DeletePerson(param as Person));
            SaveCommand = new RelayCommand(async param => await SaveChanges(param as Person));
            CancelCommand = new RelayCommand(param => CancelChanges(param as Person));
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
                OnPropertyChanged(nameof(People));

                NewPerson.FirstName = string.Empty;
                NewPerson.LastName = string.Empty;
            }

            if (dialog != null)
                await dialog.ShowAsync();
        }

        public void EditPerson(Person person)
        {
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

        public async Task DeletePerson(Person person)
        {
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

        public async Task SaveChanges(Person person)
        {
            if (person != null)
            {
                await DataStorageService.SaveData(People);
                OnPropertyChanged(nameof(People));
                person.IsEditing = false;
                person.ShowEditButtons = true;
            }
        }

        public async Task CancelChanges(Person person)
        {
            if (person != null)
            {
                person.FirstName = originalPerson.FirstName;
                person.LastName = originalPerson.LastName;
                person.IsEditing = false;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
