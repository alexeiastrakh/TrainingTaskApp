using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainingTaskApp.Models;

namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People { get; set; }

        public Person NewPerson { get; set; }
        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }

        public PersonViewModel()
        {
            People = new ObservableCollection<Person>();
            NewPerson = new Person();
            LoadData();
        }

        public void AddPerson(string firstName, string lastName)
        {
            People.Add(new Person { FirstName = firstName, LastName = lastName });
            SaveData();
            NewPerson.FirstName = "";
            NewPerson.LastName = "";
            OnPropertyChanged(nameof(People));
        }

        public void EditPerson(Person person, string newFirstName, string newLastName)
        {
            person.FirstName = newFirstName;
            person.LastName = newLastName;
            SaveData();
            OnPropertyChanged(nameof(People)); 
        }


        public void DeletePerson(Person person)
        {
            People.Remove(person);
            SaveData();
        }

        private async Task SaveData()
        {
            string data = JsonConvert.SerializeObject(People);
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Resources");
            var file = await folder.CreateFileAsync("people.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, data);
        }


        private async Task LoadData()
        {
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Resources");
            try
            {
                var file = await folder.GetFileAsync("people.json");
                string data = await Windows.Storage.FileIO.ReadTextAsync(file);
                People = JsonConvert.DeserializeObject<ObservableCollection<Person>>(data);
            }
            catch (FileNotFoundException)
            {
                People = new ObservableCollection<Person>();
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
