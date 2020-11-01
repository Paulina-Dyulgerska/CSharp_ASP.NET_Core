namespace ConformityCheck.Web.Areas.Administration.Controllers
{
    using ConformityCheck.Common;
    using ConformityCheck.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
