using LibraryManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using LibraryManagement.Web.DAL;
using LibraryManagement;
using LibraryManagement.Common;

namespace ProductDemo.Web.Controllers
{
    public class UsersController : Controller
    {

        LibraryManagement.ProjectDBEntities4 db = new ProjectDBEntities4();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.USERs.Where(u => u.USER_NAME == model.USER_NAME).Any())
                {
                    ViewBag.Message = "UserName is Already Taken, try with another UserName";
                    return View("~/Views/Users/Index.cshtml");
                }
                else
                {
                    Password encryptPassword = new Password();
                    //RegisterDataLayer dal = new RegisterDataLayer();
                    //string message = dal.SignUpUser(model);
                    USER user = new USER();
                    user.USER_NAME = model.USER_NAME;
                    user.FIRST_NAME = model.FIRST_NAME;
                    user.LAST_NAME = model.LAST_NAME;
                    user.PASSWORD = encryptPassword.EncryptPassword(model.PASSWORD);
                    user.GENDER = model.GENDER;
                    user.MOBILE = model.MOBILE;
                    user.EMAIL_ID = model.EMAIL_ID;
                    user.IS_ACTIVE = true;
                    db.USERs.Add(user);
                    db.SaveChanges();
                    return View("Create");
                }


            }
            else
            {
                return View("~/Views/Users/Index.cshtml");
            }
        }
    }
}