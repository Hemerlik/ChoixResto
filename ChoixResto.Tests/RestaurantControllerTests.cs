using ChoixResto.Controllers;
using ChoixResto.Models;
using ChoixResto.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChoixResto.Tests
{
    [TestClass]
    public class RestaurantControllerTests
    {
        [TestMethod]
        public void RestaurantController_IndexOk()
        {
            using(IDal dal = new DalEnDur())
            {
                RestaurantController controller = new RestaurantController(dal);
                ViewResult result = (ViewResult)controller.Index();
                List<Resto> restos = result.Model as List<Resto>;
                Assert.AreEqual("Resto Pinambour", restos[0].Nom);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifierRestaurant_Test()
        {
            using (IDal dal = new DalEnDur())
            {
                RestaurantController controller = new RestaurantController(dal);
                Resto resto = new Resto { Id = 1, Nom = null, Telephone = "0102030405" };
                controller.ValideLeModele(resto);

                ViewResult result = (ViewResult)controller.ModifierRestaurant(resto);

                Assert.AreEqual(string.Empty, result.ViewName);
                Assert.IsFalse(result.ViewData.ModelState.IsValid);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifierRestaurantValid_Test()
        {
            using (IDal dal = new DalEnDur())
            {
                RestaurantController controller = new RestaurantController(dal);
                Resto resto = new Resto { Id = 1, Nom = "Resto tralala", Telephone = "0102030405" };
                controller.ValideLeModele(resto);

                RedirectToRouteResult result = (RedirectToRouteResult)controller.ModifierRestaurant(resto);

                Assert.AreEqual("Index", result.RouteValues["action"]);
                Resto restotrouve = dal.ObtientTousLesRestaurants().First();
                Assert.AreEqual("Resto tralala", restotrouve.Nom);
            }
        }
    }
}
