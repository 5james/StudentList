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
    public class GroupsControllerTests
    {
        List<Groups> groups;
        GroupsController groupsController;


        //Mock<ControllerContext> Context;

        [SetUp]
        public void SetUp()
        {

            groupsController = new GroupsController();
            groups = new List<Groups>();
            groups.Add(new Groups { IDGroup = 1, Name = "A" });
            groups.Add(new Groups { IDGroup = 2, Name = "B" });
            groups.Add(new Groups { IDGroup = 3, Name = "C" });
            groups.Add(new Groups { IDGroup = 4, Name = "D" });
            groups.Add(new Groups { IDGroup = 5, Name = "E" });

        }
        

        [Test]
        public void GroupIndex()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            
            storage.Setup(o => o.GetGroups()).Returns(groups);
            groupsController.Storage = storage.Object;

            var result = groupsController.Get() as JsonResult<JsonGroupsGetResponse>;
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }
        

        [Test]
        public void CreateGroupCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Groups newgroup = new Groups
            {
                IDGroup = 1,
                Name = "Nowa"
            };
            
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);

            storage.Setup(o => o.CreateGroup(newgroup.Name)).Callback(() => groups.Add(newgroup));
            groupsController.Storage = storage.Object;

            var result = groupsController.Post(newgroup, "POST") as JsonResult<JsonGroupsGetResponse>;


            Assert.AreEqual(6, groups.Count);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }

        [Test]
        public void UpdateGroupCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Groups updatedGroup = groups.First();
            updatedGroup.Name = "Update";
            
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);
            storage.Setup(o => o.UpdateGroup(updatedGroup)).Callback(() => groups.First().Name = updatedGroup.Name);
            groupsController.Storage = storage.Object;

            var result = groupsController.Post(updatedGroup, "PUT") as JsonResult<JsonGroupsGetResponse>;

            Assert.AreEqual(5, groups.Count);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
            Assert.AreEqual(groups.First().Name, updatedGroup.Name);
        }

        [Test]
        public void DeteleGroupCtrl()
        {
            Mock<Storage> storage = new Mock<Storage>("name=StorageContext");
            Groups deletedGroup = groups.First();
            
            storage.Setup(o => o.GetGroups()).Returns(groups);
            List<Groups> tmpGroups = new List<Groups>();
            tmpGroups.Add(groups.First());
            storage.Setup(o => o.GetGroups("A")).Returns(tmpGroups);
            storage.Setup(o => o.DeleteGroup(deletedGroup)).Callback(() => groups.RemoveAt(0));
            groupsController.Storage = storage.Object;

            var result = groupsController.Post(deletedGroup, "DELETE") as JsonResult<JsonGroupsGetResponse>;

            Assert.AreEqual(4, groups.Count);
            Assert.AreEqual(groups.Count, result.Content.groupslist.Length);
        }

    }
}
