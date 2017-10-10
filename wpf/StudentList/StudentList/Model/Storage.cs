using StudentList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentsList.Model
{
    public class Storage
    {
        ILogger logger = new Logger(typeof(Storage));

        #region get from database
        public virtual List<Student> getStudents()
        {
            var db = new StorageContext();
            return db.Students.ToList();
        }

        public virtual List<Student> getStudents(Group groupArg)
        {
            var db = new StorageContext();
            return new List<Student>(db.Students.Where(s => s.Group.GroupId.Equals(groupArg.GroupId)).ToList());
        }

        public virtual List<Student> getStudents(String cityArg)
        {
            var db = new StorageContext();
            return new List<Student>(db.Students.Where(s => s.City.ToLower().Contains(cityArg.ToLower())).ToList());
        }

        public virtual List<Student> getStudents(Group groupArg, String cityArg)
        {
            var db = new StorageContext();
            return new List<Student>(db.Students.Where(s => s.Group.GroupId.Equals(groupArg.GroupId) && s.City.ToLower().Contains(cityArg.ToLower())).ToList());
        }

        public virtual List<Group> getGroups()
        {
            var db = new StorageContext();
            return db.Groups.ToList();
        }
        #endregion

        #region send to database
        public virtual bool createStudent(string firstName, string lastName, string indexid, int groupId, string city, DateTime? dateOfBirth)
        {
            try
            {
                var db = new StorageContext();
                if (db.Students.Where(st => st.IndexID.Equals(indexid)).ToList().Count > 0)
                {
                    logger.LogError("Illegal attemp to add student with same index!");
                    MessageBox.Show("Niedozwolona próba utworzenia studenta!\nIstnieje już student z takim samym numerem indeksu!", "Error", MessageBoxButton.OK);
                    return false;
                }
                var group = db.Groups.Find(groupId);
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IndexID = indexid,
                    Group = group,
                    City = city,
                    DateOfBirth = dateOfBirth
                };
                db.Students.Add(student);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
                return false;
            }
        }


        public virtual bool updateStudent(Student st)
        {
            var db = new StorageContext();
            var original = db.Students.Find(st.Id);
            if (original != null)
            {
                if (!StructuralComparisons.StructuralEqualityComparer.Equals(st.Stamp, original.Stamp))
                {
                    logger.LogError("Illegal attemp to update student! This record (student) was changed.");
                    MessageBox.Show("Niedozwolona próba aktualizacji studenta! Rekord studenta został zmieniony poza programem, który nie zdążył się zaktualizwać.", "Error", MessageBoxButton.OK);
                    return false;
                }

                if (db.Students.Where(s => s.IndexID.Equals(st.IndexID)).ToList().Count != 0)
                {
                    logger.LogError("Illegal attemp to update student's index to existing one!");
                    MessageBox.Show("Niedozwolona próba zmiany numeru indexu studenta na taki, który już istnieje!", "Error", MessageBoxButton.OK);
                    return false;
                }

                original.FirstName = st.FirstName;
                original.LastName = st.LastName;
               
                original.IndexID = st.IndexID;

                original.DateOfBirth = st.DateOfBirth;
                original.City = st.City;

                Group stGroup = db.Groups.Find(st.Group.GroupId);
                original.Group = stGroup;
                try
                {
                    db.SaveChanges();
                    return true;

                }
                catch (Exception ex)
                {
                    List<string> errorMessages = new List<string>();
                    foreach (DbEntityValidationResult validationResult in (ex as DbEntityValidationException).EntityValidationErrors)
                    {
                        string entityName = validationResult.Entry.Entity.GetType().Name;
                        foreach (DbValidationError error in validationResult.ValidationErrors)
                        {
                            logger.LogException(ex);
                            errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                        }
                    }

                    return false;
                }

            }

            return false;
        }

        public virtual void deleteStudent(Student st)
        {
            if (st == null)
            {
                logger.LogWarningMessage("No Student was picked to be removed!");
                MessageBox.Show("Żaden student nie został wybrany!", "Error", MessageBoxButton.OK);
                return;
            }
            logger.LogInfoMessage("Student with id = " + st.Id.ToString() + " is going to be removed.");
            try
            {
                var db = new StorageContext();
                var original = db.Students.Find(st.Id);
                if (original != null)
                {
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(st.Stamp, original.Stamp))
                    {
                        logger.LogError("Illegal attemp to delete student! This record (student) was changed.");
                        MessageBox.Show("Niedozwolona próba usunięcia studenta! Rekord studenta został zmieniony poza programem, który nie zdążył się zaktualizwać.", "Error", MessageBoxButton.OK);
                        return;
                    }
                    if (original.Equals(st))
                    {
                        db.Students.Remove(original);
                        db.SaveChanges();
                    }
                }
                else
                {
                    logger.LogWarningMessage("Student (to be removed) was not found in database!");
                    MessageBox.Show("Student nie został znaleziony w bazie danych.", "Error", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in (ex as DbEntityValidationException).EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
            }
        }
        #endregion
    }
}
