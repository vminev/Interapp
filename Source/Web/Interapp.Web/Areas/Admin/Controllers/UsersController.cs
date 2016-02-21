﻿namespace Interapp.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Data.Models;
    using Services.Contracts;
    using AutoMapper.QueryableExtensions;
    using Models.UsersViewModels;

    public class UsersController : Controller
    {
        private IUsersService users;

        public UsersController(IUsersService users)
        {
            this.users = users;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsersRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = this.users.All().ProjectTo<UserViewModel>();
            DataSourceResult result = model.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UsersUpdate([DataSourceRequest]DataSourceRequest request, UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var entity = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth
                };

                this.users.Update(entity);
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UsersDestroy([DataSourceRequest]DataSourceRequest request, UserViewModel user)
        {
            this.users.Delete(user.Id);

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }
    }
}
