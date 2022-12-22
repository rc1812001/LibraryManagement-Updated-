using LibraryManagement.Common;
using LibraryManagement.Web.Models;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;

namespace LibraryManagement.Web.Controllers
{
    public class LoginController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Login
        //Returns login view page
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        // Ends current session and redirects to login
        public ActionResult LogOut()
        {
            try
            {
                Session.Clear();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginModel objUser)
        {
            try
            {
                // Code for validating the Captcha  
                if (!this.IsCaptchaValid("Captcha is not valid"))
                {

                    ViewBag.Message = "Captcha is not Valid.Try again!";
                    return View(objUser);
                }

                if (ModelState.IsValid)
                {
                    Password decryptPassword = new Password();
                    //PasswordBase64 decryptPassword = new PasswordBase64();

                    using (ProjectDBEntities4 db = new ProjectDBEntities4())
                    {

                        var obj = db.USERs.ToList().Where(model => model.USER_NAME.Equals(objUser.USER_NAME) && decryptPassword.DecryptPassword(model.PASSWORD).Equals(objUser.PASSWORD)).FirstOrDefault();
                        if (obj != null && decryptPassword.DecryptPassword(obj.PASSWORD) == objUser.PASSWORD)
                        {
                            Session["USER_ID"] = obj.USER_ID.ToString();
                            Session["USER_NAME"] = obj.USER_NAME.ToString();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "UserName or Password is incorrect";
                        }
                    }
                }
                return View(objUser);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
