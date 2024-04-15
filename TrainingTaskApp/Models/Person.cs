using System.ComponentModel;
using TrainingTaskApp.Helpers;

namespace TrainingTaskApp.Models
{
    public class Person : ObservableObject
    {
        private bool isEditing;
        private bool showEditButtons = true;
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
                    NotifyPropertyChanged(nameof(FirstName));
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
                    NotifyPropertyChanged(nameof(LastName));
                }
            }
        }

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                if (isEditing != value)
                {
                    isEditing = value;
                    NotifyPropertyChanged(nameof(IsEditing));
                    NotifyPropertyChanged(nameof(ShowEditButtons));
                }
            }
        }

        public bool ShowEditButtons
        {
            get { return !isEditing; }
            set
            {
                if (showEditButtons != value)
                {
                    showEditButtons = value;
                    NotifyPropertyChanged(nameof(ShowEditButtons));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
