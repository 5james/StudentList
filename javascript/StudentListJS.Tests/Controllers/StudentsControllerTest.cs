using NUnit.Framework;
using StudentListJS.Controllers;
using System;
using System.Collections.Generic;
using Moq;
using System.Web.Mvc;
using StudentListJS.Models;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Linq;

namespace StudentListJS.Tests
{
    [TestFixture]
    public class StudentsControllerTest
    {
        List<Students> students;
        StudentsController studController;
        List<Groups> groups;
        GroupsController groupsController;


        //Mock<ControllerContext> Context;

        [SetUp]
        public void SetUp()
        {
            studController = new StudentsController();
            students = new List<Students>();
            students.Add(new Students {
                IDStudent = 1,
                IDGroup = 1,
                IndexNo = "1",
                FirstName = "Andrzej",
                LastName = "W",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Aaaa"
            });
            students.Add(new Students {
                IDStudent = 2,
                IDGroup = 2,
                IndexNo = "2",
                FirstName = "Abc",
                LastName = "Def",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Abb"
            });
            students.Add(new Students {
                IDStudent = 3,
                IDGroup = 3,
                IndexNo = "3",
                FirstName = "Jakub",
                LastName = "Jakubbo",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Bbbb"
            });
            students.Add(new Students {
                IDStudent = 4,
                IDGroup = 4,
                IndexNo = "4",
                FirstName = "Marcin",
                LastName = "Marcinio",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Abbbabbbb"
            });
            students.Add(new Students {
                IDStudent = 5,
                IDGroup = 5,
                IndexNo = "5",
                FirstName = "Tymon",
                LastName = "Tymonio",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Bbbabaaaa"
            });
            students.Add(new Students {
                IDStudent = 6,
                IDGroup = 5,
                IndexNo = "6",
                FirstName = "Tytus",
                LastName = "Tytusso",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Ababaaaba"
            });

            groupsController = new GroupsController();
            groups = new List<Groups>();
            groups.Add(new Groups { IDGroup = 1, Name = "A" });
            groups.Add(new Groups { IDGroup = 2, Name = "B" });
            groups.Add(new Groups { IDGroup = 3, Name = "C" });
            groups.Add(new Groups { IDGroup = 4, Name = "D" });
            groups.Add(new Groups { IDGroup = 5, Name = "E" });

        }

        [Test]
        public void StudentIndex()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            studController.Storage = storage.Object;

            var result = studController.Get() as JsonResult<JsonStudentsGetResponse>;
            Assert.AreEqual(students.Count, result.Content.studentslist.Length);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }


        [Test]
        public void CreateStudentCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Students newStudent = new Students
            {
                IDGroup = 1,
                IndexNo = "101",
                IDStudent = 101,
                FirstName = "Nowy",
                LastName = "Rekord",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Warszawa"
            };

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);

            storage.Setup(o => o.CreateStudent(newStudent.FirstName, newStudent.LastName, newStudent.IndexNo, newStudent.IDGroup, newStudent.BirthPlace, newStudent.BirthDate)).Callback(() => students.Add(newStudent));
            studController.Storage = storage.Object;

            var result = studController.Post(newStudent, "POST") as JsonResult<JsonStudentsGetResponse>;


            Assert.AreEqual(7, students.Count);
            Assert.AreEqual(students.Count, result.Content.studentslist.Length);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }


        [Test]
        public void UpdateStudentCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Students updatedStudent = students.First();
            updatedStudent.LastName = "Update";

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);
            storage.Setup(o => o.UpdateStudent(updatedStudent)).Callback(() => students.First().LastName = updatedStudent.LastName);
            studController.Storage = storage.Object;

            var result = studController.Post(updatedStudent, "PUT") as JsonResult<JsonStudentsGetResponse>;

            Assert.AreEqual(6, students.Count);
            Assert.AreEqual(students.Count, result.Content.studentslist.Length);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
            Assert.AreEqual(students.First().LastName, updatedStudent.LastName);
        }

        [Test]
        public void DeteleStudentCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Students deletedStudent = students.First();

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);
            storage.Setup(o => o.DeleteStudent(deletedStudent)).Callback(() => students.RemoveAt(0));
            studController.Storage = storage.Object;

            var result = studController.Post(deletedStudent, "DELETE") as JsonResult<JsonStudentsGetResponse>;

            Assert.AreEqual(5, students.Count);
            Assert.AreEqual(students.Count, result.Content.studentslist.Length);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }


        [Test]
        public void CreateAndDeleteStudentCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Students newStudent = new Students
            {
                IDGroup = 1,
                IndexNo = "101",
                IDStudent = 101,
                FirstName = "Nowy",
                LastName = "Rekord",
                BirthDate = new DateTime(2017, 1, 1),
                BirthPlace = "Warszawa"
            };

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);

            storage.Setup(o => o.CreateStudent(newStudent.FirstName, newStudent.LastName, newStudent.IndexNo, newStudent.IDGroup, newStudent.BirthPlace, newStudent.BirthDate)).Callback(() => students.Add(newStudent));

            storage.Setup(o => o.DeleteStudent(newStudent)).Callback(() => students.Remove(newStudent));
            studController.Storage = storage.Object;

            var result = studController.Post(newStudent, "POST") as JsonResult<JsonStudentsGetResponse>;
            result = studController.Post(newStudent, "DELETE") as JsonResult<JsonStudentsGetResponse>;



            Assert.AreEqual(6, students.Count);
            Assert.AreEqual(students.Count, result.Content.studentslist.Length);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }
    }
}
