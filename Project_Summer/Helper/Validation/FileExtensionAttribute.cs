using System.ComponentModel.DataAnnotations;

namespace Project_Summer.Helper.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                String[] extensions = { "jpg", "png", "jpeg"};
                bool reultt = extensions.Any(x =>extension.EndsWith(x));
                if (!reultt)
                {
                    return new ValidationResult("chỉ cho phép file jpg or png");
                }
            }
            return ValidationResult.Success;
        }
    }
}
