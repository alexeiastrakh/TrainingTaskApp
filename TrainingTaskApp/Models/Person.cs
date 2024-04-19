using System.ComponentModel;
using TrainingTaskApp.Helpers;

namespace TrainingTaskApp.Models
{
    public class Person : ObservableObject
    {
        private string firstName;
        private string lastName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
