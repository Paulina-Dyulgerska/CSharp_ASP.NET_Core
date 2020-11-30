namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class FileExtensionAttribute : ValidationAttribute
    {
        public FileExtensionAttribute(string extension)
        {
            this.Extension = extension;
            this.ErrorMessage = $"File should be of type: .{extension}";
        }

        public string Extension { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var hasRightExtension = ((IFormFile)value).FileName.EndsWith($".{this.Extension}");

            if (hasRightExtension)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
