using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChoixResto.Tests.Extensions
{
    public static class ControllerExtension
    {
        public static void ValideLeModele<T>(this Controller controller, T modele)
        {
            controller.ModelState.Clear();

            ValidationContext context = new ValidationContext(modele, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(modele, context, results);

            foreach(ValidationResult result in results)
            {
                foreach(var memberName in result.MemberNames)
                {
                    controller.ModelState.AddModelError(memberName, result.ErrorMessage);
                }
            }
        }
    }
}
