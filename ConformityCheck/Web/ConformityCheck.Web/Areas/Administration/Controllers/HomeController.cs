namespace ConformityCheck.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdministrationController
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return this.View();
        }
    }
}
