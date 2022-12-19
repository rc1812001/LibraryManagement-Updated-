using LibraryManagement.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {


        ProjectDBEntities4 db = new ProjectDBEntities4();
        // To Search input string in Books based on id or Name/Category
        public ActionResult Search(string q)
        {
            var books = from b in db.BOOKs select b;
            int id = Convert.ToInt32(Request["SearchType"]);
            var searchParameter = "Search result for ";
            try
            {
                if (!string.IsNullOrWhiteSpace(q))
                {
                    switch (id)
                    {
                        case 0:
                            try
                            {
                                int.TryParse(q, out int iQ);
                                books = books.Where(b => b.BOOK_ID.Equals(iQ));
                                searchParameter += " Id = ' " + q + " '";
                            }
                            catch (Exception)
                            {
                                ViewBag.Message = "Enter int .";

                            }
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
                var finalbooks = db.BOOKs.Any() ? books.ToList() : new List<BOOK>();
                return View("Books", finalbooks);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Books", books);
            }
        }
        // ------------------------------ BOOKS LIST -------------------------------------------//
        // Returns list of books to view if session is on else redirects to login 
        public ActionResult Books()
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

        //------------------------- CREATE ---------------------------------//
        //Returns create view
        public ActionResult Create()
        {
            ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //Adds a new book to database based on data from create view
        public ActionResult Create(BookModel Book)
        {
            ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();
            if (ModelState.IsValid)
            {
                BOOK book = new BOOK();

              

                book.BOOK_NAME = Book.BOOK_NAME;
                book.CATEGORY = Book.CATEGORY;
                book.STATUS = book.STATUS;  
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
        //----------------------------------- EDIT ---------------------------//
        public ActionResult Edit(int Id)
        {

            // To fill data in the form
            // to enable easy editing

            var books = db.BOOKs.Where(a => a.BOOK_ID == Id).FirstOrDefault();
            ViewBag.CategoryList = db.CATEGORies.Select(c => c.CATEGORY_NAME).ToList();

            return View(books);

        }

        [HttpPost]

        public ActionResult Edit(int Id, BOOK book)  //passing id , book tb obj as paaramters
        {
            // Use of lambda expression to access
            // particular record from a database

            var data = db.BOOKs.Where(a => a.BOOK_ID == Id).FirstOrDefault();

          
                data.BOOK_NAME = book.BOOK_NAME;     //selected parameters are here
                data.CATEGORY = book.CATEGORY;
                data.STATUS = book.STATUS;
                data.AUTHOR = book.AUTHOR;
                data.UPDATED_BY = Session["USER_NAME"].ToString();
                data.UPDATE_TIMESTAMP = DateTime.Now;
                db.SaveChanges();

                // It will redirect to Books 
                return RedirectToAction("Books");
            
            
        }
         //--------------------------- DELETE ------------------------------------- //
        //redirects to delete view with book data to be deleted
        public ActionResult Delete(int id)
        {

            var book = db.BOOKs.Where(b => b.BOOK_ID == id).FirstOrDefault();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        // Deletes given book from using id
        public ActionResult Delete(BOOK book, int id)
        {
            var books = db.BOOKs.Where(b => b.BOOK_ID == id).FirstOrDefault();
            db.BOOKs.Remove(books);
            db.SaveChanges();
            return RedirectToAction("Books");
        }
    }

}

