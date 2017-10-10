using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using StudentListASP;
using StudentListASP.Controllers;
using StudentListASP.Models;
using PagedList;


namespace StudentListASP.Tests.Controllers
{
    [TestFixture]
    public class StudentsListASPTests
    {
        List<Students> students;
        StudentsController studController;
        List<Groups> groups;
        GroupsController groupsController;


        Mock<ControllerContext> Context;

        [SetUp]
        public void SetUp()
        {
            Context = new Mock<ControllerContext>();
            studController = new StudentsController();
            groupsController = new GroupsController();
            studController.ControllerContext = Context.Object;
            groupsController.ControllerContext = Context.Object;


            students = new List<Students>();
            students.Add(new Students { IDStudent = 1, IDGroup = 1, IndexNo = "1", FirstName = "Andrzej", LastName = "Wajda", BirthDate = DateTime.Now, BirthPlace = "Aaaa"  });
            students.Add(new Students { IDStudent = 2, IDGroup = 2, IndexNo = "2",  FirstName = "Abc", LastName = "Def", BirthDate = DateTime.Now, BirthPlace = "Abb" });
            students.Add(new Students { IDStudent = 3, IDGroup = 3, IndexNo = "3", FirstName = "Jakub", LastName = "Jakubbo", BirthDate = DateTime.Now, BirthPlace = "Bbbb" });
            students.Add(new Students { IDStudent = 4, IDGroup = 4, IndexNo = "4", FirstName = "Marcin", LastName = "Marcinio", BirthDate = DateTime.Now, BirthPlace = "Abbbabbbb"  });
            students.Add(new Students { IDStudent = 5, IDGroup = 5, IndexNo = "5", FirstName = "Tymon", LastName = "Tymonio", BirthDate = DateTime.Now, BirthPlace = "Bbbabaaaa" });
            students.Add(new Students { IDStudent = 5,  IDGroup = 5, IndexNo = "5", FirstName = "Tytus", LastName = "Tytys", BirthDate = DateTime.Now, BirthPlace = "Ababaaaba" });


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
            Mock<Storage> storage = new Mock<Storage>();

            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            studController.Storage = storage.Object;
            ViewResult result = studController.Index(1, null, null, "", "") as ViewResult;
            Assert.AreEqual(5, (result.Model as StudentsList).PagedStudents.Count);
        }

        [Test]
        public void GroupsIndex()
        {
            Mock<Storage> storage = new Mock<Storage>();
            storage.Setup(o => o.GetGroups()).Returns(groups);
            groupsController.Storage = storage.Object;

            ViewResult result = groupsController.Index(1, "") as ViewResult;

            Assert.AreEqual(2, (result.Model as GroupsList).PagedGroups.Count);

        }

        [Test]
        public void GroupsDelete()
        {
            Mock<Storage> storage = new Mock<Storage>();
            storage.Setup(o => o.GetGroups()).Returns(groups);

            Groups gr = groups.First();
            storage.Setup(o => o.DeleteGroup(gr)).Callback(() => groups.Remove(gr));
            groupsController.Storage = storage.Object;

            ActionResult result = (groupsController.FilterGroups(new GroupsList { CurrentGroup = gr }, "Delete"));

            Assert.AreEqual(4, groups.Count);

        }

        [Test]
        public void GroupsAdd()
        {
            Mock<Storage> storage = new Mock<Storage>();
            storage.Setup(o => o.GetGroups()).Returns(groups);

            Groups gr = new Groups { Name = "FF", IDGroup = 6 };
            storage.Setup(o => o.CreateGroup("FF")).Callback(() => groups.Add(gr));
            groupsController.Storage = storage.Object;

            ViewResult result = (groupsController.FilterGroups(new GroupsList { CurrentGroup = gr }, "Create")) as ViewResult;

            Assert.AreEqual(6, groups.Count);
        }

        [Test]
        public void GroupsEdit()
        {
            Mock<Storage> storage = new Mock<Storage>();
            storage.Setup(o => o.GetGroups()).Returns(groups);

            Groups gr = groups.First();
            gr.Name = "1111";
            storage.Setup(o => o.UpdateGroup(gr)).Callback(() => groups.First().Name = gr.Name);
            groupsController.Storage = storage.Object;

            ViewResult result = (groupsController.FilterGroups(new GroupsList { CurrentGroup = gr }, "Edit")) as ViewResult;

            Assert.AreEqual(5, groups.Count);
            Assert.AreEqual("1111", groups.First().Name);

        }
        [Test]
        public void StudentsRemove()
        {
            Mock<Storage> storage = new Mock<Storage>();

            string town = "Aaaa";
            string group = "1";
            var g = new Groups() { IDGroup = 1, Name = "1" };


            List<Groups> gl = new List<Groups>();
            gl.Add(new Groups() { IDGroup = 1, Name = "1" });


            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            storage.Setup(o => o.GetGroups(1)).Returns(gl);
            storage.Setup(o => o.GetGroups("1")).Returns(gl);
            storage.Setup(o => o.GetStudents(g, town)).Returns(students.FindAll(s => s.BirthPlace.Equals(town) && s.IDGroup.Equals(1)));
            studController.Storage = storage.Object;

            ViewResult result = studController.Index(1, town, group, "", "Filter") as ViewResult;


            Assert.AreEqual(1, (result.Model as StudentsList).PagedStudents.Count);
        }
    }
    [Test]
        public void StudentsAdd()
        {
            Mock<Storage> storage = new Mock<Storage>();
            storage.Setup(o => o.GetGroups()).Returns(groups);

            Groups gr = new Groups { Name = "FF", IDGroup = 6 };
            storage.Setup(o => o.CreateGroup("FF")).Callback(() => groups.Add(gr));
            groupsController.Storage = storage.Object;

            ViewResult result = (groupsController.FilterGroups(new GroupsList { CurrentGroup = gr }, "Create")) as ViewResult;

            Assert.AreEqual(6, groups.Count);
        }

        [Test]
        public void StudentsFilterGroup()
        {
            Mock<Storage> storage = new Mock<Storage>();

            var g = new Groups() { IDGroup = 1, Name = "1" };

            List<Groups> gl = new List<Groups>();
            gl.Add(new Groups() { IDGroup = 1, Name = "1" });
            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            storage.Setup(o => o.GetGroups(1)).Returns(gl);
            storage.Setup(o => o.GetGroups("1")).Returns(gl);
            storage.Setup(o => o.GetStudents(g)).Returns(students.GetRange(0, 1));
            studController.Storage = storage.Object;

            ViewResult result = studController.Index(1, null, "1", "", "Filter") as ViewResult;


            Assert.AreEqual(1, (result.Model as StudentsList).PagedStudents.Count);
            Assert.AreEqual(1, (result.Model as StudentsList).PagedStudents.First().IDGroup);


        }

        [Test]
        public void StudentsFilterTown()
        {
            Mock<Storage> storage = new Mock<Storage>();

            string town = "Aaaa";
            var g = new Groups() { IDGroup = 1, Name = "1" };


            List<Groups> gl = new List<Groups>();
            gl.Add(new Groups() { IDGroup = 1, Name = "1" });


            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            storage.Setup(o => o.GetGroups(1)).Returns(gl);
            storage.Setup(o => o.GetGroups("1")).Returns(gl);
            storage.Setup(o => o.GetStudents(town)).Returns(students.FindAll(s => s.BirthPlace.Equals(town)));
            studController.Storage = storage.Object;

            ViewResult result = studController.Index(1, town, null, "", "Filter") as ViewResult;


            Assert.AreEqual(1, (result.Model as StudentsList).PagedStudents.Count);
        }


        [Test]
        public void StudentsFilterTownGroup()
        {
            Mock<Storage> storage = new Mock<Storage>();

            string town = "Aaaa";
            string group = "1";
            var g = new Groups() { IDGroup = 1, Name = "1" };


            List<Groups> gl = new List<Groups>();
            gl.Add(new Groups() { IDGroup = 1, Name = "1" });


            storage.Setup(o => o.GetStudents()).Returns(students);
            storage.Setup(o => o.GetGroups()).Returns(groups);
            storage.Setup(o => o.GetGroups(1)).Returns(gl);
            storage.Setup(o => o.GetGroups("1")).Returns(gl);
            storage.Setup(o => o.GetStudents(g, town)).Returns(students.FindAll(s => s.BirthPlace.Equals(town) && s.IDGroup.Equals(1)));
            studController.Storage = storage.Object;

            ViewResult result = studController.Index(1, town, group, "", "Filter") as ViewResult;


            Assert.AreEqual(1, (result.Model as StudentsList).PagedStudents.Count);
        }



}
