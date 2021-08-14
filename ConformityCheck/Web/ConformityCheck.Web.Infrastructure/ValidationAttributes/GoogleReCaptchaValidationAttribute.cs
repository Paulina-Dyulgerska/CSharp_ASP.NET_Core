namespace ConformityCheck.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;

    using ConformityCheck.Web.Infrastructure.ReCaptcha;
    using ConformityCheck.Web.Infrastructure.Settings;
    using Microsoft.Extensions.Options;

    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var recaptchaSettings = (IOptions<ReCaptchaSettings>)validationContext.GetService(typeof(IOptions<ReCaptchaSettings>));

            if (recaptchaSettings == null || string.IsNullOrWhiteSpace(recaptchaSettings.Value.Secret))
            {
                return new ValidationResult(
                    "Google reCAPTCHA validation failed. Secret key not found.",
                    new[] { validationContext.MemberName });
            }

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("secret", recaptchaSettings.Value.Secret),
                        new KeyValuePair<string, string>("response", value.ToString()),
                    //// new KeyValuePair<string, string>("remoteip", remoteIp),
                    });

            var httpResponse = httpClient.PostAsync(recaptchaSettings.Value.ApiUrl, content)
                .GetAwaiter().GetResult();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new ValidationResult(
                    $"Google reCAPTCHA validation failed. Status code: {httpResponse.StatusCode}.",
                    new[] { validationContext.MemberName });
            }

            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var siteVerifyResponse = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);
            return siteVerifyResponse.Success
                       ? ValidationResult.Success
                       : new ValidationResult(
                           "Google reCAPTCHA validation failed.",
                           new[] { validationContext.MemberName });
        }
    }
}
