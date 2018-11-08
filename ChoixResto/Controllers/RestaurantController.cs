using ChoixResto.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal _dal;

        public RestaurantController() : this(new Dal()) { }
        public RestaurantController(IDal dal) => _dal = dal;

        // GET: Restaurant
        public ActionResult Index()
        {
            List<Resto> restos = _dal.ObtientTousLesRestaurants();
            return View(restos);
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                var resto = _dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (resto != null)
                {
                    return View(resto);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
            {
                return View(resto);
            }

            _dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            if (_dal.RestaurantExiste(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Un autre restaurant portant le même nom existe déjà");
                return View(resto);
            }

            if (!ModelState.IsValid)
            {
                return View(resto);
            }

            _dal.CreerRestaurant(resto.Nom, resto.Telephone);
            return RedirectToAction("Index");

        }
    }
}