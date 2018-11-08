using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ChoixResto.Attributes
{
    public class AtLeastOneAttribute : ValidationAttribute, IClientValidatable
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }

        public AtLeastOneAttribute():base("You must at least give one way to contact the restaurant")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo[] properties = validationContext.ObjectType.GetProperties();
            PropertyInfo property1 = properties.FirstOrDefault(p => p.Name == Param1);
            PropertyInfo property2 = properties.FirstOrDefault(p => p.Name == Param2);

            var param1Value = property1.GetValue(validationContext.ObjectInstance) as string;
            var param2Value = property2.GetValue(validationContext.ObjectInstance) as string;

            if (string.IsNullOrWhiteSpace(param1Value) && string.IsNullOrWhiteSpace(param2Value))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ValidationType = "verifcontact";
            rule.ErrorMessage = ErrorMessage;
            rule.ValidationParameters.Add("parametre1", Param1);
            rule.ValidationParameters.Add("parametre2", Param2);

            return new List<ModelClientValidationRule> { rule };
        }
    }
}