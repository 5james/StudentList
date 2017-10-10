using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsList.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StudentsList;
using System.Windows;

namespace StudentList.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        ILogger logger = new Logger(typeof(MainWindowViewModel));

        #region Binded Commands
        public ICommand CreateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        #endregion

        public Storage storage { get; set; }

        #region private variables
        private String _firstName;
        private String _lastName;
        private String _city;
        private DateTime? _dateOfBirth;
        private string _index;

        private Group _selectedGroup;
        private Student _selectedStudent;

        private List<Student> _students = new List<Student>();

        private String _cityFilter;
        private Group _selectedGroupFilter;
        #endregion

        public MainWindowViewModel()
        {
            storage = new Storage();
            Students = storage.getStudents();

            #region ButtonCommands
            CreateCommand = new RelayCommand(
                    new Action<object>(delegate (object arg)
                    {
                        if (validateRequiredStudentsFields())
                        {
                            if (storage.createStudent(FirstName, LastName, IndexID, SelectedGroup.GroupId, City, DateOfBirth))
                                flushEntryData();
                        }
                        Students = storage.getStudents();
                    }), canAddStudent);

            DeleteCommand = new RelayCommand(
                new Action<object>(delegate (object arg)
                {
                    storage.deleteStudent(SelectedStudent);
                    flushEntryData();
                    Students = storage.getStudents();
                }), canDeleteStudent);
            UpdateCommand = new RelayCommand(
                new Action<object>(delegate (object arg)
                {
                    if (validateRequiredStudentsFields())
                    {
                        if (storage.updateStudent(new Student
                        {
                            Id = SelectedStudent.Id,
                            IndexID = IndexID,
                            FirstName = FirstName,
                            LastName = LastName,
                            City = City,
                            Group = SelectedGroup,
                            DateOfBirth = DateOfBirth,
                            Stamp = SelectedStudent.Stamp
                        }))
                        {
                            flushEntryData();
                        }
                    }
                    Students = storage.getStudents();
                }), canUpdateStudent);

            ClearCommand = new RelayCommand(
                new Action<object>(delegate (object arg)
                {
                    flushFilterData();
                    Students = storage.getStudents();
                }), CanClearFilters);

            FilterCommand = new RelayCommand(
                new Action<object>(delegate (object arg)
                {
                    filterStudents();
                }), () => { return true; });
            #endregion

        }
        #region database get groups/students
        public List<Group> Groups
        {
            get
            {
                List<Group> group = storage.getGroups();
                Group nullobj = new Group { GroupId = -1, Name = "" };
                group.Insert(0, nullobj);
                return group;
            }
        }

        public List<Student> Students
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region student properties get/set
        public String FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public String LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public String City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string IndexID
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region filter properties get/set
        public String CityFilter
        {
            get
            {
                return _cityFilter;
            }
            set
            {
                _cityFilter = value;
                OnPropertyChanged();
            }
        }

        public Group SelectedGroupFilter
        {
            get
            {
                return _selectedGroupFilter;
            }
            set
            {
                _selectedGroupFilter = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();

                //(UpdateCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();

                if (_selectedStudent != null)
                {
                    FirstName = _selectedStudent.FirstName;
                    LastName = _selectedStudent.LastName;
                    City = _selectedStudent.City;
                    DateOfBirth = _selectedStudent.DateOfBirth;
                    IndexID = _selectedStudent.IndexID;
                    Group selGroup = Groups.Find(i => i.GroupId.Equals(_selectedStudent.Group.GroupId));
                    SelectedGroup = selGroup;
                }
                else
                {
                    FirstName = "";
                    LastName = "";
                    City = "";
                    DateOfBirth = null;
                    IndexID = "";
                    SelectedGroup = null;
                }

                //(DeleteCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        #region interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Utils
        private void flushEntryData()
        {
            FirstName = "";
            LastName = "";
            City = "";
            DateOfBirth = null;
            SelectedGroup = null;
            IndexID = "";
        }

        private void flushFilterData()
        {
            SelectedGroupFilter = null;
            CityFilter = "";
        }

        private void filterStudents()
        {
            if ((SelectedGroupFilter == null || (SelectedGroupFilter != null && SelectedGroupFilter.GroupId == -1)) &&
                (String.IsNullOrEmpty(CityFilter) || String.IsNullOrWhiteSpace(CityFilter)))
            {
                Students = storage.getStudents();
            }
            else if (SelectedGroupFilter == null || (SelectedGroupFilter != null && SelectedGroupFilter.GroupId == -1))
            {
                Students = storage.getStudents(CityFilter);
            }
            else if ((String.IsNullOrEmpty(CityFilter) || String.IsNullOrWhiteSpace(CityFilter)))
            {
                Students = storage.getStudents(SelectedGroupFilter);
            }
            else
            {
                Students = storage.getStudents(SelectedGroupFilter, CityFilter);
            }
        }

        private bool validateRequiredStudentsFields()
        {
            bool toreturn = (!String.IsNullOrWhiteSpace(IndexID) &&
                            !String.IsNullOrWhiteSpace(FirstName) &&
                            !String.IsNullOrWhiteSpace(LastName) &&
                            (SelectedGroup != null) &&
                            SelectedGroup.GroupId > 0);
            if (!toreturn)
            {
                logger.LogError("Illegal attemp to create/update student with unsifficient datas!");
                MessageBox.Show("Za mało danych! Wymagania:\n- Imie\n- Nazwisko\n- Numer indeksu\n- Przypisana grupa", "Error", MessageBoxButton.OK);
                return false;
            }
            return toreturn;
        }

        //private bool canUpdate;
        //private bool isStudentSelected()
        //{
        //    return SelectedStudent != null;
        //}

        private bool isSelectedStudentAsPrevious()
        {
            if (City != null)
            {
                if (!City.Equals(SelectedStudent.City))
                    return false;
            } else if (SelectedStudent.City != null)
            {
                if (!SelectedStudent.City.Equals(City))
                    return false;
            }



            if (DateOfBirth != null)
            {
                if (!DateOfBirth.Equals(SelectedStudent.DateOfBirth))
                    return false;
            }else if (SelectedStudent.DateOfBirth != null)
            {
                if (!SelectedStudent.DateOfBirth.Equals(DateOfBirth))
                    return false;
            }



            if (FirstName != null)
            {
                if (!FirstName.Equals(SelectedStudent.FirstName))
                    return false;
            }else if (SelectedStudent.FirstName != null)
            {
                if (!SelectedStudent.FirstName.Equals(FirstName))
                    return false;
            }



            if (LastName != null)
            {
                if (!LastName.Equals(SelectedStudent.LastName))
                    return false;
            }
            else if (SelectedStudent.LastName != null)
            {
                if (!SelectedStudent.LastName.Equals(LastName))
                    return false;
            }


            if (SelectedGroup != null)
            {
                if (!SelectedGroup.Equals(SelectedStudent.Group))
                    return false;
            }
            else if (SelectedStudent.Group != null)
            {
                if (!SelectedStudent.Group.Equals(SelectedGroup))
                    return false;
            }



            if (IndexID != null)
            {
                if (!IndexID.Equals(SelectedStudent.IndexID))
                    return false;
            }
            else if (SelectedStudent.IndexID != null)
            {
                if (!SelectedStudent.IndexID.Equals(IndexID))
                    return false;
            }
            
            return true;
        }
        #endregion

        #region Ability of Buttons to click
        private bool canDeleteStudent()
        {
            return SelectedStudent != null;
        }

        private bool canUpdateStudent()
        {
            if (SelectedStudent != null)
            {
                return !isSelectedStudentAsPrevious();
            }
            else
                return false;
        }

        private bool canAddStudent()
        {
            if (SelectedStudent != null)
            {
                return !isSelectedStudentAsPrevious();
            }
            else
                return true;
        }

        private bool CanClearFilters()
        {
            if (SelectedGroupFilter == null && String.IsNullOrEmpty(CityFilter))
            {
                return false;
            }
            else if (SelectedGroupFilter != null && SelectedGroupFilter.GroupId == -1 && String.IsNullOrEmpty(CityFilter))
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}