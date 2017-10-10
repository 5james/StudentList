using log4net.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StudentListJS.Models
{
    public class Storage
    {
        ILogger logger = new Logger(typeof(Storage));
        string dbName;
        public Storage(string name = "name=StorageContext")
        {
            dbName = name;
            //Database.SetInitializer<StorageContext>(null);
        }

        #region GET from DB
        public virtual List<Students> GetStudents()
        {
            var db = new StorageContext(dbName);
            return new List<Students>(db.Students.Include(s => s.Groups).ToList());
        }

        public virtual List<Students> GetStudents(Groups groupArg)
        {
            var db = new StorageContext(dbName);
            return new List<Students>(db.Students.Where(s => s.Groups.IDGroup.Equals(groupArg.IDGroup)).ToList());
        }

        public virtual List<Students> GetStudents(String cityArg)
        {
            var db = new StorageContext(dbName);
            return new List<Students>(db.Students.Where(s => s.BirthPlace.ToLower().Contains(cityArg.ToLower())).ToList());
        }

        public virtual List<Students> GetStudents(Groups groupArg, String cityArg)
        {
            var db = new StorageContext(dbName);
            return new List<Students>(db.Students.Where(s => s.Groups.IDGroup.Equals(groupArg.IDGroup) && s.BirthPlace.ToLower().Contains(cityArg.ToLower())).ToList());
        }

        //public virtual List<PagedGroups> getGroups()
        //{
        //    var db = new StorageContext(dbName);
        //    return new List<PagedGroups>(db.PagedGroups.ToList());
        //}

        public virtual List<Groups> GetGroups()
        {
            var db = new StorageContext(dbName);
            return new List<Groups>(db.Groups.ToList());
        }

        public virtual List<Groups> GetGroups(string name)
        {
            var db = new StorageContext(dbName);
            var a = new List<Groups>(db.Groups.Where(g => g.Name.Equals(name)));
            return new List<Groups>(db.Groups.Where(g => g.Name.Equals(name)));
        }

        public virtual List<Groups> GetGroups(int id)
        {
            var db = new StorageContext(dbName);
            return new List<Groups>(db.Groups.Where(g => g.IDGroup.Equals(id)));
        }

        #endregion

        #region POST to DB
        public virtual void CreateStudent(string firstName, string lastName, string indexid, int groupId, string city, DateTime? dateOfBirth)
        {
            try
            {
                var db = new StorageContext(dbName);
                if (db.Students.Where(st => st.IndexNo.Equals(indexid)).ToList().Count > 0)
                {
                    logger.LogError("Illegal attemp to add student with same index!");
                    throw new Exception("Niedozwolona próba utworzenia studenta! Istnieje już student z takim samym numerem indeksu!");
                }
                var group = db.Groups.Find(groupId);
                if (group == null)
                {
                    throw new Exception("Niedozwolona próba utworzenia studenta! Nie istnieje wybrana grupa!");
                }
                var student = new Students
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IndexNo = indexid,
                    IDGroup = group.IDGroup,
                    BirthPlace = city,
                    BirthDate = dateOfBirth
                };
                db.Students.Add(student);
                db.SaveChanges();
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
                throw new Exception("Niedozwolona próba utworzenia studenta!");
            }
        }

        public virtual void UpdateStudent(Students st)
        {
            var db = new StorageContext(dbName);
            var original = db.Students.Find(st.IDStudent);
            if (original != null)
            {
                if (!StructuralComparisons.StructuralEqualityComparer.Equals(st.Stamp, original.Stamp))
                {
                    logger.LogError("Illegal attemp to update student! This record (student) was changed.");
                    throw new Exception("Niedozwolona próba aktualizacji studenta! Rekord studenta został zmieniony poza programem, który nie zdążył się zaktualizwać.");
                }
                if (db.Students.Where(s => s.IndexNo.Equals(st.IndexNo) && !(s.IDStudent.Equals(st.IDStudent))).ToList().Count != 0)
                {
                    logger.LogError("Illegal attemp to update student's index to existing one!");
                    throw new Exception("Niedozwolona próba zmiany numeru indexu studenta na taki, który już istnieje!");
                }

                original.FirstName = st.FirstName;
                original.LastName = st.LastName;
                original.IndexNo = st.IndexNo;
                original.BirthDate = st.BirthDate;
                original.BirthPlace = st.BirthPlace;
                original.IDGroup = st.IDGroup;

                try
                {
                    db.SaveChanges();
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
                    throw new Exception("Niedozwolona próba zmiany studenta!");
                }
            }
            else
            {
                throw new Exception("Niedozwolona próba zmiany studenta, który już nie istnieje!");
            }
        }

        public virtual void DeleteStudent(Students st)
        {
            if (st == null)
            {
                logger.LogWarningMessage("No Student was picked to be removed!");
                throw new Exception("Żaden student nie został wybrany!");
            }
            logger.LogInfoMessage("Student with id = " + st.IDStudent.ToString() + " is going to be removed.");
            try
            {
                var db = new StorageContext(dbName);
                var original = db.Students.Find(st.IDStudent);
                if (original != null)
                {
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(st.Stamp, original.Stamp))
                    {
                        logger.LogError("Illegal attemp to delete student! This record (student) was changed.");
                        throw new Exception("Niedozwolona próba usunięcia studenta! Rekord studenta został zmieniony poza programem, który nie zdążył się zaktualizwać.");
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
                    throw new Exception("Student nie został znaleziony w bazie danych.");
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
        public virtual void CreateGroup(string name)
        {
            try
            {
                var db = new StorageContext(dbName);
                if (db.Groups.Where(g => g.Name.Equals(name)).ToList().Count > 0)
                {
                    logger.LogError("Illegal attemp to add group with same name!");
                    throw new Exception("Niedozwolona próba utworzenia grupy! Istnieje już grupa z taką nazwą!");
                }
                var group = new Groups
                {
                    Name = name
                };
                db.Groups.Add(group);
                db.SaveChanges();
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
                throw new Exception("Niedozwolona próba utworzenia grupy!");
            }
        }

        public virtual void DeleteGroup(Groups gr)
        {
            if (gr == null)
            {
                logger.LogWarningMessage("No Group was picked to be removed!");
                throw new Exception("Żadna grupa nie została wybrana!");
            }
            logger.LogInfoMessage("Group with id = " + gr.IDGroup.ToString() + " is going to be removed.");
            try
            {
                var db = new StorageContext(dbName);
                var original = db.Groups.Find(gr.IDGroup);
                if (original != null)
                {
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(gr.Stamp, original.Stamp))
                    {
                        logger.LogError("Illegal attemp to delete group! This record (group) was changed.");
                        throw new Exception("Niedozwolona próba usunięcia grupy! Rekord grupy został zmieniony poza programem, który nie zdążył się zaktualizwać.");
                    }
                    if (original.Equals(gr))
                    {
                        db.Groups.Remove(original);
                        db.SaveChanges();
                    }
                }
                else
                {
                    logger.LogWarningMessage("Group (to be removed) was not found in database!");
                    throw new Exception("Grupa nie została znaleziona w bazie danych.");
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

        public virtual void UpdateGroup(Groups gr)
        {
            var db = new StorageContext(dbName);
            var original = db.Groups.Find(gr.IDGroup);
            if (original != null)
            {
                if (!StructuralComparisons.StructuralEqualityComparer.Equals(gr.Stamp, original.Stamp))
                {
                    logger.LogError("Illegal attemp to update group! This record (group) was changed.");
                    throw new Exception("Niedozwolona próba aktualizacji grupy! Rekord grupy został zmieniony poza programem, który nie zdążył się zaktualizwać.");
                }
                if (db.Groups.Where(s => s.IDGroup.Equals(gr.IDGroup) && !(s.IDGroup.Equals(gr.IDGroup))).ToList().Count != 0)
                {
                    logger.LogError("Illegal attemp to update group's name to existing one!");
                    throw new Exception("Niedozwolona próba zmiany nazwy grupy na taką, która już istnieje!");
                }

                original.Name = gr.Name;

                try
                {
                    db.SaveChanges();
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
                    throw new Exception("Niedozwolona próba zmiany groupy!");
                }
            }
            else
            {
                throw new Exception("Niedozwolona próba zmiany groupy, który już nie istnieje!");
            }
        }
        #endregion
    }
}