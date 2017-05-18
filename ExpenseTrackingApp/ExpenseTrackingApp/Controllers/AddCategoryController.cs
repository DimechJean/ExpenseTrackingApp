using ExpenseTrackingApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class AddCategoryController : Controller
    {
        // GET: AddCategory
        public ActionResult Index()
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccount");
            }
            return View();
        }

        public ActionResult Add()
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("../Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(TransactionCategory category)
        {
            HttpCookie auth = Request.Cookies["auth"];
            string email = auth.Values.Get("Email");
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                    decimal maxId;
                    if (db.TransactionCategory.Count() == 0)
                        maxId = 0;
                    else
                    {
                        maxId = db.TransactionCategory.Max(x => x.ID);
                        maxId++;
                    }
                    category.ID = maxId;
                    category.UserAccount = user.ID;
                    if (CategoryExists(category.NameCat,email))
                    {
                        TempData["notice"] = "Category Already Exists";
                        return RedirectToAction("Add", "AddCategory");
                    }

                    db.TransactionCategory.Add(category);
                    db.SaveChanges();
                }

                ModelState.Clear();
                TempData["notice"] = "Category Added Successfully";
                return RedirectToAction("TransactionCategories", "View");
            }

            return Content("Invalid Category");
        }

        public static bool CategoryExists(string catname,string email)
        {
            using (Model1 db = new Model1())
            {

                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                List<TransactionCategory> transactions = db.TransactionCategory.SqlQuery("Select * from [dbo].[TransactionCategory] where UserAccount is null or UserAccount = " + user.ID).ToList();
                foreach (TransactionCategory cat in transactions)
                {
                    if (cat.NameCat == catname)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}