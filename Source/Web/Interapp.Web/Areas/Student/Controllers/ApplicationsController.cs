﻿namespace Interapp.Web.Areas.Student.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using Models.ApplicationsViewModels;
    using Services.Contracts;
    using System.Web.Mvc;

    [Authorize(Roles = "Student")]
    public class ApplicationsController : Controller
    {
        private IApplicationsService applications;
        private IStudentInfosService studentInfos;
        private IMajorsService majors;

        public ApplicationsController(IApplicationsService applications, IStudentInfosService studentInfos, IMajorsService majors)
        {
            this.applications = applications;
            this.studentInfos = studentInfos;
            this.majors = majors;
        }

        [HttpGet]
        public ActionResult All()
        {
            var studentId = this.User.Identity.GetUserId();
            var model = this.applications
                .AllByStudent(studentId)
                .ProjectTo<ApplicationViewModel>();

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var application = this.applications
                .GetById(id);
            var studentId = this.User.Identity.GetUserId();

            if (application.StudentId != studentId)
            {
                return this.View();
            }

            var model = Mapper.Map<ApplicationDetailsViewModel>(application);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Submit(ApplicationInputViewModel model)
        {
            var studentId = this.User.Identity.GetUserId();
            var eligiblity = this.studentInfos.IsEligibleToApply(studentId, model.UniversityId);

            if (!eligiblity.IsEligible)
            {
                this.ModelState.AddModelError("Eligiblity", eligiblity.Message);
            }
            else
            {
                var major = this.majors.GetById(model.MajorId);
                
                if(major == null)
                {
                    this.ModelState.AddModelError("Major", "There is no such major.");
                }
            }

            if (this.ModelState.IsValid)
            {
                this.applications.Create(studentId, model.UniversityId, model.MajorId);
                return this.PartialView("_SuccessfullySubmitted");
            }

            return this.PartialView("_Error", model);
        }
    }
}