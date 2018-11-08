using ChoixResto.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace ChoixResto.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void HomeControllerIndexTest()
        {
            HomeController controller = new HomeController();
            ViewResult result = (ViewResult)controller.Index();
            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
