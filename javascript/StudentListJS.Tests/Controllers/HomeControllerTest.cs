using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentListJS;
using StudentListJS.Controllers;

namespace StudentListJS.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Student List", result.ViewBag.Title);
        }
    }
}
