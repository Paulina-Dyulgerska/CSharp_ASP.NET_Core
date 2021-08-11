namespace ConformityCheck.Data
{
    using System;

    using Microsoft.AspNetCore.Identity;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 30, 0);
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = true;

            //// TODO - for production:
            // options.Password.RequireDigit = true;
            // options.Password.RequireLowercase = true;
            // options.Password.RequireUppercase = true;
            // options.Password.RequireNonAlphanumeric = true;
            // options.Password.RequiredLength = 8;
            // options.Lockout.MaxFailedAccessAttempts = 3;
            // options.Password.RequiredUniqueChars = 6;
            // options.SignIn.RequireConfirmedEmail = true;
            // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        }
    }
}
