using DocumentFormat.OpenXml.Office2010.Excel;
using LibraryManagement.Common;
using LibraryManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagement.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //Returns login view page
        public ActionResult Login()
        {
            return View();
        }

        // Ends current session and redirects to login
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginModel objUser)
        {
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
                        return RedirectToAction("Books","Books");
                    }
                    else
                    {
                        ViewBag.Message = "UserName or Password is incorrect";
                    }
                }
            }
            return View(objUser);
        }
    }
}
