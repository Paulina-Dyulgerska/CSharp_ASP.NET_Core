namespace ConformityCheck.Web.Areas.Administration.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ConformityCheck.Common;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IUsersService usersService,
            ILogger<UsersController> logger,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.logger = logger;
            this.userManager = userManager;
        }

        public IActionResult Index(PagingViewModel input)
        {
            if (input.PageNumber <= 0)
            {
                // loads StatusCodeError404.cshtm
                return this.NotFound();
            }

            var model = new UsersIndexModel
            {
                ItemsPerPage = input.ItemsPerPage,
                PageNumber = input.PageNumber,
                PagingControllerActionCallName = nameof(this.Index),
                CurrentSortOrder = input.CurrentSortOrder,
                CurrentSearchInput = input.CurrentSearchInput,
                CurrentSortDirection = input.CurrentSortDirection == GlobalConstants.CurrentSortDirectionDesc ?
                                    GlobalConstants.CurrentSortDirectionAsc : GlobalConstants.CurrentSortDirectionDesc,
            };

            try
            {
                if (string.IsNullOrWhiteSpace(input.CurrentSearchInput))
                {
                    model.ItemsCount = this.usersService.GetCount();
                    model.Users = this.usersService.GetAllOrderedAsPages(
                                                    input.CurrentSortOrder,
                                                    input.PageNumber,
                                                    input.ItemsPerPage);
                }
                else
                {
                    input.CurrentSearchInput = input.CurrentSearchInput.Trim();
                    model.ItemsCount = this.usersService.GetCountBySearchInput(input.CurrentSearchInput);
                    model.Users = this.usersService.GetAllBySearchInputOrderedAsPages(
                                                input.CurrentSearchInput,
                                                input.CurrentSortOrder,
                                                input.PageNumber,
                                                input.ItemsPerPage);
                }

                this.logger.LogInformation($"Users loaded successfully");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Users loading failed: {ex}");

                return this.Redirect("/");
            }

            return this.View(model);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                await this.usersService.CreateAsync(input);

                this.TempData[GlobalConstants.TempDataMessagePropertyName] = GlobalConstants.ArticleCreatedSuccessfullyMessage;
                this.logger.LogInformation($"User: {input.UserName} was created by user: {user.Id}");
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.OperationFailed;
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Article creation failed: {ex}");

                // return user to the Create view instead of Home/Error page
                return this.View();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(string id)
        {
            var user = this.usersService.GetById(id);

            return this.View(user);
        }

        public IActionResult Details(UserIdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDataErrorMessagePropertyName] = GlobalConstants.InvalidEntityId;

                return this.RedirectToAction(nameof(this.Index));
            }

            var model = this.usersService.GetById(input.Id);

            return this.View(model);
        }
    }
}
