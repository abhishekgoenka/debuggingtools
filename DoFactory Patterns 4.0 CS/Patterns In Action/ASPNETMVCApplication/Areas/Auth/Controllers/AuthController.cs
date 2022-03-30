using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Security;
using System.Diagnostics.CodeAnalysis;

using ASPNETMVCApplication.Areas.Auth.Models;
using ASPNETMVCApplication.Code.Filters;
using ASPNETMVCApplication.Code.HtmlHelpers;
using ASPNETMVCApplication.Controllers;
using ASPNETMVCApplication.Repositories;
using ASPNETMVCApplication.Repositories.Core;
using ASPNETWebApplication;

namespace ASPNETMVCApplication.Areas.Auth.Controllers
{
    /// <summary>
    /// Controller class for the Authentication area.
    /// </summary>
    public class AuthController : BaseController
    {
        private IAuthRepository _authRepository;

        /// <summary>
        /// Default Constructor for AuthController.
        /// </summary>
        public AuthController() :
            this(new AuthRepository())
        {
        }

        /// <summary>
        /// Overloaded 'injectable' Constructor for AuthController.
        ///
        /// Pattern: Constructor Dependency Injection (DI).
        /// </summary>
        /// <param name="authRepository">The authentication repository</param>
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Action method. 
        /// Prepares login page.
        /// </summary>
        /// <returns></returns>
        [Menu(MenuItem.Login)]
        public ActionResult Login()
        {
            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Title = "login" } };

            return View(new LoginModel());
        }

        /// <summary>
        /// Action method. HTTP POST only.
        /// Processes login credentials.
        /// </summary>
        /// <param name="model">The login credentials entered by user.</param>
        /// <param name="returnUrl">Return url following login.</param>
        /// <returns></returns>
        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_authRepository.Login(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
            }

            ModelState.AddModelError("", "The username or password are incorrect.");

            return View(model);
        }

        /// <summary>
        /// Action method. Performs a logout for current user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            _authRepository.Logout();
            FormsAuthentication.SignOut();

            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Title = "logout" } };
            return View();
        }
    }
}
