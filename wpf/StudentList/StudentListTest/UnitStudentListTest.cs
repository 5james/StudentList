using System;
using StudentList.ViewModel;
using StudentsList.Model;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace StudentListTest
{
    [TestFixture]
    public class UnitStudentListTest
    {
        private MainWindowViewModel listVM;

        Storage storage;
        List<Student> studentList;

        [OneTimeSetUp]
        public void MyClassInitialize()
        {
            storage = new Storage();

            studentList = storage.getStudents();
            foreach (Student st in studentList)
            {
                storage.deleteStudent(st);
            }
        }

        //[TestFixtureSetUp]
        //public void reinitVM()
        //{
        //    listVM = new MainWindowViewModel();
        //}

        [SetUp]
        public void reinitVM2()
        {
            listVM = new MainWindowViewModel();
        }

        [OneTimeTearDown]
        public void MyClassCleanup()
        {
            foreach (Student st in studentList)
            {
                storage.createStudent(st.FirstName, st.LastName, st.IndexID, st.Group.GroupId, st.City, st.DateOfBirth);
            }
        }

        [TearDown]
        public void CleanUpAfterTest()
        {
            foreach (Student st in storage.getStudents())
            {
                storage.deleteStudent(st);
            }
        }


        [Test]
        public void createStudentWithVMFailNoData1()
        {
            Assert.AreEqual(0, listVM.Students.Count);

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(0, listVM.Students.Count);
        }

        [Test]
        public void createStudentWithVMFailNoData2()
        {
            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(0, listVM.Students.Count);
        }

        [Test]
        public void createStudentWithVMFailNoData3()
        {
            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(0, listVM.Students.Count);
        }

        [Test]
        public void createStudentWithVMSuccessful1()
        {
            Assert.AreEqual(0, listVM.Students.Count);
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);
        }

        //[Test]
        //public void mockGroups1()
        //{
        //    MockRepository mocks = new MockRepository();
        //    Storage calculator = (Storage)mocks.StrictMock(typeof(Storage));

        //    List<Group> groups = new List<Group>
        //    {
        //        new Group { GroupId = 1, Name = "1" },
        //        new Group { GroupId = 2, Name = "2" },
        //        new Group { GroupId = 3, Name = "3" }
        //    };
        //}

        [Test]
        public void StubGroups()
        {
            var stubs = MockRepository.GenerateMock<Storage>();

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };
            stubs.Stub(s => s.getGroups()).Return(groups);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };

            Assert.AreEqual(newvm.Groups.Count, 4);
        }

        [Test]
        public void StubStudents()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents()).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            newvm.Students = newvm.storage.getStudents();

            Assert.AreEqual(3, newvm.Students.Count);
        }

        [Test]
        public void StubGroupOfStudents()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="Test", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents(groups[0])).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            newvm.Students = newvm.storage.getStudents(groups[0]);

            Assert.AreEqual(3, newvm.Students.Count);
        }

        [Test]
        public void StubCityOfStudents()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            string city = "TestCity";

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents(city)).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            newvm.Students = newvm.storage.getStudents(city);

            Assert.AreEqual(3, newvm.Students.Count);
        }

        [Test]
        public void StubCityAndGroupOfStudents()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            string city = "TestCity";

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents(groups[0], city)).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            newvm.Students = newvm.storage.getStudents(groups[0], city);

            Assert.AreEqual(3, newvm.Students.Count);
        }


        [Test]
        public void StubFilters1()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            string city = "TestCity";

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="not", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents()).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            newvm.FilterCommand.Execute("");

            Assert.AreEqual(3, newvm.Students.Count);
        }

        [Test]
        public void StubFilters2()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            string city = "TestCity";

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents(city)).Return(students);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            //newvm.Students = newvm.storage.getStudents();

            newvm.CityFilter = "TestCity";
            newvm.FilterCommand.Execute("");

            Assert.AreEqual(3, newvm.Students.Count);
        }

        [Test]
        public void StubFilters3()
        {
            DateTime today = DateTime.Today;

            var stubs = MockRepository.GenerateMock<Storage>();

            string city = "TestCity";

            List<Group> groups = new List<Group>
            {
                new Group { GroupId = 1, Name = "1" },
                new Group { GroupId = 2, Name = "2" },
                new Group { GroupId = 3, Name = "3" }
            };

            List<Student> students = new List<Student>
            {
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=1, IndexID="1"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=2, IndexID="2"},
                new Student { FirstName = "Test", LastName = "Test", City="TestCity", DateOfBirth = today, Group = groups[0], Id=3, IndexID="3"}
            };
            stubs.Stub(s => s.getStudents(groups[0])).Return(students);
            //stubs.Stub(g => g.getGroups()).Return(groups);
            MainWindowViewModel newvm = new MainWindowViewModel { storage = stubs };
            //newvm.Students = newvm.storage.getStudents();

            //newvm.CityFilter = "TestCity";
            newvm.SelectedGroupFilter = groups[0];
            newvm.FilterCommand.Execute("");

            Assert.AreEqual(3, newvm.Students.Count);
        }


        [Test]
        public void createStudentWithVMSuccessful2()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
        }

        [Test]
        public void createStudentWithVMFailSameIndex()
        {
            Assert.AreEqual(0, listVM.Students.Count);
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test2";
            listVM.LastName = "Test2";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);
        }

        [Test]
        public void createStudentDBSuccessful1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);
            Assert.AreEqual(0, storage.getStudents().Count);

            storage.createStudent("Test", "test", "1", groups[0].GroupId, "Warszawa", DateTime.Today);

            Assert.AreEqual(1, storage.getStudents().Count);
        }

        //[Test]
        //public void createStudentMockSuccessful1()
        //{
        //    MockRepository mocks = new MockRepository();
        //    Storage _storage = (Storage)mocks.StrictMock(typeof(Storage));

        //    DateTime today = DateTime.Today;

        //    List<Group> groups = storage.getGroups();
        //    Assert.Greater(groups.Count, 0);
        //    Assert.AreEqual(0, storage.getStudents().Count);

        //    storage.createStudent("Test", "Test", "1", groups[0].GroupId, "Warszawa", today);
        //    Assert.AreEqual(1, storage.getStudents().Count);

        //    mocks.ReplayAll();
        //    listVM.storage = _storage;


        //    listVM.FirstName = "Test";
        //    listVM.LastName = "Test";
        //    listVM.IndexID = "1";
        //    listVM.SelectedGroup = groups[0];
        //    listVM.City = "Warszawa";
        //    listVM.DateOfBirth = today;

        //    listVM.CreateCommand.Execute("");

        //    Assert.AreEqual(1, storage.getStudents().Count);

        //    mocks.VerifyAll();
        //}

        [Test]
        public void deleteStudentWithVMSuccessful1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful1 if doesn't work)

            listVM.SelectedStudent = listVM.Students[0];
            listVM.DeleteCommand.Execute("");

            Assert.AreEqual(0, listVM.Students.Count);
        }


        [Test]
        public void deleteStudentWithVMSuccessful2()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful2 if doesn't work)

            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 1];
            listVM.DeleteCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);

            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 1];
            listVM.DeleteCommand.Execute("");

            Assert.AreEqual(0, listVM.Students.Count);
        }

        [Test]
        public void deleteStudentWithVMSuccessfulSequenceChoice()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful2 if doesn't work)

            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 1];
            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 2];
            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 1];
            listVM.DeleteCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);

            Assert.AreEqual(listVM.Students[0].IndexID, "1");
        }

        [Test]
        public void deleteStudentWithVMFailNoStudentSelected1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful1 if doesn't work)

            //listVM.SelectedStudent = listVM.Students[0];
            listVM.DeleteCommand.Execute("");

            Assert.AreEqual(1, listVM.Students.Count);
        }


        [Test]
        public void deleteStudentDBSuccessful1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);
            Assert.AreEqual(0, storage.getStudents().Count);

            storage.createStudent("Test", "test", "1", groups[0].GroupId, "Warszawa", DateTime.Today);
            Assert.AreEqual(1, storage.getStudents().Count);

            storage.deleteStudent(storage.getStudents()[0]);
            Assert.AreEqual(0, storage.getStudents().Count);
        }

        //[Test]
        //public void deleteStudentMockSuccessful1()
        //{


        //    MockRepository mocks = new MockRepository();
        //    Storage _storage = (Storage)mocks.StrictMock(typeof(Storage));

        //    DateTime today = DateTime.Today;

        //    List<Group> groups = storage.getGroups();
        //    Assert.Greater(groups.Count, 0);
        //    //Assert.AreEqual(0, storage.getStudents().Count);

        //    storage.createStudent("Test", "Test", "1", groups[0].GroupId, "Warszawa", today);
        //    //Assert.AreEqual(1, storage.getStudents().Count);

        //    storage.deleteStudent(storage.getStudents()[0]);
        //    //Assert.AreEqual(0, storage.getStudents().Count);

        //    mocks.ReplayAll();
        //    listVM.storage = _storage;


        //    listVM.FirstName = "Test";
        //    listVM.LastName = "Test";
        //    listVM.IndexID = "1";
        //    listVM.SelectedGroup = groups[0];
        //    listVM.City = "Warszawa";
        //    listVM.DateOfBirth = today;

        //    listVM.CreateCommand.Execute("");
        //    //Assert.AreEqual(1, storage.getStudents().Count);

        //    listVM.SelectedStudent = listVM.Students[0];
        //    listVM.DeleteCommand.Execute("");

        //    mocks.VerifyAll();
        //}

        [Test]
        public void updateStudentWithVMSuccessful1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful2 if doesn't work)

            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 2];
            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 1];
            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 2];

            listVM.FirstName = "Updated";
            listVM.LastName = "Updated";
            listVM.IndexID = "2016";

            listVM.UpdateCommand.Execute("");

            Assert.AreEqual(2, listVM.Students.Count);

            Assert.AreEqual("Updated", listVM.Students[0].FirstName);
            Assert.AreEqual("Updated", listVM.Students[0].LastName);
            Assert.AreEqual("2016", listVM.Students[0].IndexID);

        }



        [Test]
        public void updateStudentWithVMFailSameIndex1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);

            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful2 if doesn't work)

            listVM.SelectedStudent = listVM.Students[0];
            listVM.SelectedStudent = listVM.Students[1];
            listVM.SelectedStudent = listVM.Students[0];

            listVM.FirstName = "Updated";
            listVM.LastName = "Updated";
            listVM.IndexID = "2";

            listVM.UpdateCommand.Execute("");

            Assert.AreEqual(2, listVM.Students.Count);

            Assert.AreEqual("Test", listVM.Students[0].FirstName);
            Assert.AreEqual("Test", listVM.Students[0].LastName);
            Assert.AreEqual("1", listVM.Students[0].IndexID);

        }

        [Test]
        public void updateStudentWithVMFailNothingChange1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);
            Assert.AreEqual(0, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "1";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(1, listVM.Students.Count);

            listVM.FirstName = "Test";
            listVM.LastName = "Test";
            listVM.SelectedGroup = groups[0];
            listVM.IndexID = "2";
            listVM.DateOfBirth = DateTime.Today;

            listVM.CreateCommand.Execute("");
            Assert.AreEqual(2, listVM.Students.Count);
            // Two Students are added (AddStudentWithVMSuccessful2 if doesn't work)

            listVM.SelectedStudent = listVM.Students[listVM.Students.Count - 2];

            listVM.UpdateCommand.Execute("");

            Assert.AreEqual(2, listVM.Students.Count);

            Assert.AreEqual("Test", listVM.Students[0].FirstName);
            Assert.AreEqual("Test", listVM.Students[0].LastName);
            Assert.AreEqual("1", listVM.Students[0].IndexID);
        }

        [Test]
        public void updateStudentDBSuccessful1()
        {
            List<Group> groups = storage.getGroups();
            Assert.Greater(groups.Count, 0);
            Assert.AreEqual(0, storage.getStudents().Count);

            storage.createStudent("Test", "test", "1", groups[0].GroupId, "Warszawa", DateTime.Today);
            Assert.AreEqual(1, storage.getStudents().Count);
            storage.createStudent("Test", "test", "2", groups[0].GroupId, "Warszawa", DateTime.Today);
            Assert.AreEqual(2, storage.getStudents().Count);

            Student oldstudent = storage.getStudents()[0];

            Student newstudent = new Student
            {
                Id = oldstudent.Id,
                IndexID = "3",
                FirstName = "Updated",
                LastName = "Updated",
                City = oldstudent.City,
                Group = new Group { GroupId = oldstudent.Group.GroupId, Name = oldstudent.Group.Name, Stamp = oldstudent.Group.Stamp },
                DateOfBirth = oldstudent.DateOfBirth,
                Stamp = oldstudent.Stamp
            };

            storage.updateStudent(newstudent);
            Assert.AreEqual(2, storage.getStudents().Count);

            Student alteredstudent = storage.getStudents()[0];

            Assert.AreEqual("Updated", alteredstudent.LastName);
            Assert.AreEqual("Updated", alteredstudent.LastName);
            Assert.AreEqual("3", alteredstudent.IndexID);
        }

        //[Test]
        //public void MockSuccessful1()
        //{


        //    MockRepository mocks = new MockRepository();
        //    Storage _storage = (Storage)mocks.StrictMock(typeof(Storage));

        //    DateTime today = DateTime.Today;

        //    List<Group> groups = storage.getGroups();
        //    Assert.Greater(groups.Count, 0);
        //    Assert.AreEqual(0, storage.getStudents().Count);

        //    storage.createStudent("Test", "Test", "1", groups[0].GroupId, "Warszawa", today);
        //    Assert.AreEqual(1, storage.getStudents().Count);
        //    storage.createStudent("Test", "Test", "2", groups[0].GroupId, "Warszawa", today);
        //    Assert.AreEqual(2, storage.getStudents().Count);

        //    Student oldstudent = storage.getStudents()[0];

        //    Student newstudent = new Student
        //    {
        //        Id = oldstudent.Id,
        //        IndexID = "3",
        //        FirstName = "Updated",
        //        LastName = "Updated",
        //        City = oldstudent.City,
        //        Group = new Group { GroupId = oldstudent.Group.GroupId, Name = oldstudent.Group.Name, Stamp = oldstudent.Group.Stamp },
        //        DateOfBirth = oldstudent.DateOfBirth,
        //        Stamp = oldstudent.Stamp
        //    };

        //    storage.updateStudent(newstudent);
        //    Assert.AreEqual(2, storage.getStudents().Count);

        //    Student alteredstudent = storage.getStudents()[0];

        //    Assert.AreEqual("Updated", alteredstudent.LastName);
        //    Assert.AreEqual("Updated", alteredstudent.LastName);
        //    Assert.AreEqual("3", alteredstudent.IndexID);

        //    storage.deleteStudent()

        //    mocks.ReplayAll();
        //    listVM.storage = _storage;


        //    listVM.FirstName = "Test";
        //    listVM.LastName = "Test";
        //    listVM.IndexID = "1";
        //    listVM.SelectedGroup = groups[0];
        //    listVM.City = "Warszawa";
        //    listVM.DateOfBirth = today;

        //    listVM.CreateCommand.Execute("");

        //    listVM.FirstName = "Test";
        //    listVM.LastName = "Test";
        //    listVM.IndexID = "2";
        //    listVM.SelectedGroup = groups[0];
        //    listVM.City = "Warszawa";
        //    listVM.DateOfBirth = today;

        //    listVM.CreateCommand.Execute("");

        //    listVM.SelectedStudent = listVM.Students[0];

        //    listVM.FirstName = "Updated";
        //    listVM.LastName = "Updated";
        //    listVM.IndexID = "3";

        //    listVM.UpdateCommand.Execute("");

        //    mocks.VerifyAll();
        //}

    }
}