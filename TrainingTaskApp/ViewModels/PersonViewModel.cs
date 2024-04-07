using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TrainingTaskApp.Services;
using TrainingTaskApp.Models;
using Windows.UI.Xaml.Controls;


namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People { get; set; }

        public Person NewPerson { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public PersonViewModel()
        {
            People = new ObservableCollection<Person>();
            NewPerson = new Person();
            LoadData();

            AddCommand = new RelayCommand(param => AddPerson(NewPerson.FirstName, NewPerson.LastName));
            EditCommand = new RelayCommand(param => EditPerson(param as Person));
            DeleteCommand = new RelayCommand(param => DeletePerson(param as Person));
        }

        public async void AddPerson(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                ContentDialog emptyFieldsDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "FirstName and LastName couldn't be empty",
                    CloseButtonText = "ОК"
                };

                ContentDialogResult result = await emptyFieldsDialog.ShowAsync();
            }
            else
            {
                People.Add(new Person { FirstName = firstName, LastName = lastName });
                await DataStorageService.SaveData(People);
                OnPropertyChanged(nameof(People));

                NewPerson.FirstName = string.Empty;
                NewPerson.LastName = string.Empty;

                OnPropertyChanged(nameof(NewPerson.FirstName));
                OnPropertyChanged(nameof(NewPerson.LastName));
            }
        }



        public async void EditPerson(Person person)
        {
            if (person != null)
            {
                await DataStorageService.SaveData(People);
                OnPropertyChanged(nameof(People));
            }
        }

        public async void DeletePerson(Person person)
        {
            if (person != null)
            {
                People.Remove(person);
                await DataStorageService.SaveData(People);
            }
        }

        public async void LoadData()
        {
            People = await DataStorageService.LoadData();
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
