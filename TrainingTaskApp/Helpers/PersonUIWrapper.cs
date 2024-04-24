using System.ComponentModel;
using TrainingTaskApp.Models;

namespace TrainingTaskApp.Helpers
{
    public class PersonUIWrapper : ObservableObject, IEditableObject
    {
        private bool isEditing;
        private bool showEditButtons = true;
        private Person person;
        private Person backupPerson;

        public Person Person
        {
            get { return person; }
            set
            {
                if (person != value)
                {
                    person = value;
                    NotifyPropertyChanged();
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

        public void BeginEdit()
        {
            if (!isEditing)
            {
                backupPerson = new Person { FirstName = person.FirstName, LastName = person.LastName };
                IsEditing = true;
            }
        }

        public void EndEdit()
        {
            if (isEditing)
            {
                backupPerson = null;
                IsEditing = false;
                ShowEditButtons = true;
            }
        }

        public void CancelEdit()
        {
            if (isEditing && backupPerson != null)
            {
                person.FirstName = backupPerson.FirstName;
                person.LastName = backupPerson.LastName;
                EndEdit();
            }
        }
    }
}
