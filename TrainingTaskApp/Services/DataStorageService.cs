using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTaskApp.Models;

namespace TrainingTaskApp.Services
{
    public class DataStorageService
    {
        public static async Task SaveData(ObservableCollection<Person> people)
        {
            string data = JsonConvert.SerializeObject(people);
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("people.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, data);
        }

        public static async Task<ObservableCollection<Person>> LoadData()
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                var file = await folder.GetFileAsync("people.json");
                string data = await Windows.Storage.FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<ObservableCollection<Person>>(data);
            }
            catch (FileNotFoundException)
            {
                return new ObservableCollection<Person>();
            }
        }
    }
}
