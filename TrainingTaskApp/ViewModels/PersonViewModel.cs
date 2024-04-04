using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainingTaskApp.Models;

namespace TrainingTaskApp.ViewModels
{
    public class PersonViewModel
    {
        public ObservableCollection<Person> People { get; set; }

        public PersonViewModel()
        {
            People = new ObservableCollection<Person>();
            LoadData();
        }

        public void AddPerson(string firstName, string lastName)
        {
            People.Add(new Person { FirstName = firstName, LastName = lastName });
            SaveData();
        }

        public void EditPerson(Person person, string newFirstName, string newLastName)
        {
            person.FirstName = newFirstName;
            person.LastName = newLastName;
            SaveData();
        }

        public void DeletePerson(Person person)
        {
            People.Remove(person);
            SaveData();
        }

        private async Task SaveData()
        {
            string data = JsonConvert.SerializeObject(People);
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync("people.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, data);
        }

        private async Task LoadData()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                var file = await localFolder.GetFileAsync("people.json");
                string data = await Windows.Storage.FileIO.ReadTextAsync(file);
                People = JsonConvert.DeserializeObject<ObservableCollection<Person>>(data);
            }
            catch (FileNotFoundException)
            {
                // If file not found, initialize People collection
                People = new ObservableCollection<Person>();
            }
        }
    }
}
