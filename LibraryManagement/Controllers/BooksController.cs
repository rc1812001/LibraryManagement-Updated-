using LibraryManagement.Models;
using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using NLog;
using System.Web.Mvc;
using Logger = NLog.Logger;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {

        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //using NLog for logging

        readonly ProjectDBEntities4 db = new ProjectDBEntities4();

        // To Search input string in Books based on id or Name/Category
        public ActionResult Search(string q)
        {

            var books = from book in db.BOOKs select book;

            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                int ID = Convert.ToInt32(Request["SearchType"]);
                var searchParameter = "Search result for ";

                if (!string.IsNullOrWhiteSpace(q))
                {
                    switch (ID)
                    {
                        case 0:
                            int iQ = int.Parse(q);
                            books = books.Where(b => b.BOOK_ID.Equals(iQ));
                            searchParameter += " Id = ' " + q + " '";
                            break;
                        case 1:
                            books = books.Where(b => b.BOOK_NAME.Contains(q) || b.CATEGORY.Contains(q));
                            searchParameter += " Title/Category contains ' " + q + " '";
                            break;

                    }
                }
                else
                {
                    searchParameter += "ALL";
                }
                ViewBag.SearchParameter = searchParameter;
                return View("Books", books);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("Books", books);
            }
        }
        // ------------------------------ BOOKS LIST -------------------------------------------//
        // Returns list of books to view if session is active is on else redirects to login 
        public ActionResult Books()
        {
            try
            {

                if (Session["USER_ID"] != null)
                {

                    return View(db.BOOKs.ToList());
                }
                else
                {
                    ViewBag.Message = "Username or password is incorrect.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //------------------------- CREATE ---------------------------------//
        //Returns create view
        public ActionResult Create()
        {
            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();

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
        public ActionResult Create(BookModel bookModel)
        {
            try
            {
                if (Session["USER_ID"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();

                if (ModelState.IsValid)
                {
                    BOOK book = new BOOK();

                    book.BOOK_NAME = bookModel.BOOK_NAME;
                    book.CATEGORY = bookModel.CATEGORY;
                    book.STATUS = bookModel.STATUS;
                    book.AUTHOR = bookModel.AUTHOR;
                    book.CREATED_BY = Session["USER_NAME"].ToString();
                    book.CREATE_TIMESTAMP = DateTime.Now;
                    db.BOOKs.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Books");
                }
                else
                {
                    return View("Create");
                }

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                Logger.Error(ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //----------------------------------- EDIT ---------------------------//
        public ActionResult Edit(int Id)
            {

                // To fill data in the form
                // to enable easy editing

                try
                {
                    if (Session["USER_ID"] == null)
                    {
                        return RedirectToAction("Login", "Login");
                    }


                    var books = db.BOOKs.Where(a => a.BOOK_ID == Id).FirstOrDefault();
                    ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();
                    BookModel bookModel = new BookModel();

                    bookModel.BOOK_ID = books.BOOK_ID;
                    bookModel.BOOK_NAME = books.BOOK_NAME;
                    bookModel.CATEGORY = books.CATEGORY;
                    bookModel.STATUS = books.STATUS;
                    bookModel.AUTHOR = books.AUTHOR;
                    return View(bookModel);

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    Logger.Error(ex);
                    return View("~/Views/Shared/Error.cshtml");
                }

            }


            [HttpPost]
            // To edit book data and save it to database
            public ActionResult Edit(BookModel bookModel)  //passing id , book tb obj as paaramters
            {
                // Use of lambda expression to access
                // particular record from a database

                try
                {
                    if (Session["USER_ID"] == null)
                    {
                        return RedirectToAction("Login", "Login");
                    }
                    ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();

                    if (ModelState.IsValid)
                    {
                        var data = db.BOOKs.Where(a => a.BOOK_ID == bookModel.BOOK_ID).FirstOrDefault();


                        data.BOOK_NAME = bookModel.BOOK_NAME;     //selected parameters are here
                        data.CATEGORY = bookModel.CATEGORY;
                        data.STATUS = bookModel.STATUS;
                        data.AUTHOR = bookModel.AUTHOR;
                        data.UPDATED_BY = Session["USER_NAME"].ToString();
                        data.UPDATE_TIMESTAMP = DateTime.Now;

                        db.SaveChanges();

                        // It will redirect to Books 
                        return RedirectToAction("Books");
                    }
                    return View(bookModel);
                }

                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    Logger.Error(ex);
                    return View("~/Views/Shared/Error.cshtml");
                }
            }


            //--------------------------- DELETE ------------------------------------- //
            //redirects to delete view with book data to be deleted
            public ActionResult Delete(int Id)
            {
                try
                {
                    if (Session["USER_ID"] == null)
                    {
                        return RedirectToAction("Login", "Login");
                    }
                var books = db.BOOKs.Where(a => a.BOOK_ID == Id).FirstOrDefault();
                BookModel bookModel = new BookModel();

                bookModel.BOOK_ID = books.BOOK_ID;
                bookModel.BOOK_NAME = books.BOOK_NAME;
                bookModel.CATEGORY = books.CATEGORY;
                bookModel.STATUS = books.STATUS;
                bookModel.AUTHOR = books.AUTHOR;
                return View(bookModel);
;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    Logger.Error(ex);
                    return View("~/Views/Shared/Error.cshtml");
                }
            }


        [HttpPost]
           

            // Deletes given book from using id
            public ActionResult Delete(BookModel bookModel)
            {
                try
                {
                    if (Session["USER_ID"] == null)
                    {
                        return RedirectToAction("Login", "Login");
                    }


                   var data = db.BOOKs.Where(a => a.BOOK_ID == bookModel.BOOK_ID).FirstOrDefault();
                    db.SaveChanges();
                    db.BOOKs.Remove(data);
                     db.SaveChanges();
                    return RedirectToAction("Books");
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

