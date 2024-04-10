using System.ComponentModel;

namespace TrainingTaskApp.Models
{
    public class Person : INotifyPropertyChanged
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
                    OnPropertyChanged(nameof(FirstName));
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
                    OnPropertyChanged(nameof(LastName));
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
                    OnPropertyChanged(nameof(IsEditing));
                    OnPropertyChanged(nameof(ShowEditButtons));
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
                    OnPropertyChanged(nameof(ShowEditButtons));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
