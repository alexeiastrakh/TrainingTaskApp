using System.ComponentModel;
using TrainingTaskApp.Models;

namespace TrainingTaskApp.Helpers
{
    public class PersonUIWrapper : ObservableObject
    {
        private bool isEditing;
        private bool showEditButtons = true;
        private Person person;

        public Person Person
        {
            get { return person; }
            set
            {
                if (person != value)
                {
                    person = value;
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
