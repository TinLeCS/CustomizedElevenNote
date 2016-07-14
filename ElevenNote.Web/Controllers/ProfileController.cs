using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly Lazy<ProfileService> _svc;

        public ProfileController()
        {
            _svc =
                new Lazy<ProfileService>(
                    () =>
                    {
                        var email = User.Identity.GetUserName();
                        return new ProfileService(email);
                    });
        }

        // GET: Profile
        public ActionResult Index()
        {
            var profile = _svc.Value.GetProfile();

            return View(profile);
        }

        public ActionResult Edit(string email)
        {
            var detail = _svc.Value.GetProfile();
            var profile =
                new ProfileViewModel
                {
                    Email = detail.Email,
                    FirstName = detail.FirstName,
                    LastName = detail.LastName,
                    Address = detail.Address,
                    PhoneNumber = detail.PhoneNumber
                };
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!_svc.Value.UpdateProfile(vm))
            {
                ModelState.AddModelError("", "Unable to update profile");
                return View(vm);
            }

            return RedirectToAction("Index");
        }
    }
}