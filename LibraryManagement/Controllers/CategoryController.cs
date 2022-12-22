using DocumentFormat.OpenXml.Spreadsheet;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{

    public class CategoryController : Controller
    {


        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        readonly ProjectDBEntities4 db = new ProjectDBEntities4();


        // To Search input string in Categories based on id or Name
        public ActionResult Search(string q)
        {
            var Cats = from c in db.CATEGORies select c;
            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                int id = Convert.ToInt32(Request["SearchType"]);
                var searchParameter = "Search result for ";

                if (!string.IsNullOrWhiteSpace(q))
                {
                    switch (id)
                    {
                        case 0:
                            int iQ = int.Parse(q);
                            Cats = Cats.Where(c => c.CATEGORY_ID.Equals(iQ));
                            searchParameter += " Id = ' " + q + " '";
                            break;
                        case 1:
                            Cats = Cats.Where(b => b.CATEGORY_NAME.Contains(q));
                            searchParameter += "Category Name contains ' " + q + " '";
                            break;

                    }
                }
                else
                {
                    searchParameter += "ALL";
                }
                ViewBag.SearchParameter = searchParameter;
                return View("CategoryList", Cats);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("CategoryList", Cats);
            }


        }

        // Returns list of Categories to view if session is active else redirects to login 
        public ActionResult CategoryList()
        {
            try
            {
                if (Session["USER_ID"] != null)
                {

                    return View(db.CATEGORies.ToList());
                }
                else
                {
                    ViewBag.Message = "Username or password is incorrect.";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }


        // create view
        public ActionResult Create()
        {
            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                return View();
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

        //Adds a new book to database based on data from create view
        public ActionResult Create(CATEGORY category)
        {
            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                if (ModelState.IsValid)
                {
                    if (db.CATEGORies.Where(u => u.CATEGORY_NAME == category.CATEGORY_NAME).Any())
                    {
                        ViewBag.Message = "This category already exist";
                        return View();
                    }
                    category.CREATED_BY = Session["UserName"].ToString();
                    category.CREATE_TIMESTAMP = DateTime.Now;

                    db.CATEGORies.Add(category);

                    db.SaveChanges();
                    return RedirectToAction("CategoryList");
                }

                return View();
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