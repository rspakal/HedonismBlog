using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HedonismBlog.ViewModels.Validation
{
    public class LatinCharactersOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var input = value.ToString();
                if (!Regex.IsMatch(input, @"^[a-zA-Z]*$"))
                {
                    return new ValidationResult("The field must contain only Latin characters.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
