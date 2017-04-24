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
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TransactionCategory category)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    int ID = 0;
                    foreach (TransactionCategory tc in db.TransactionCategory)
                    {
                        ID++;
                    }
                    category.ID = ID;

                    if (CategoryExists(category.NameCat))
                    {
                        return Content("Category Already Exist");
                    }

                    db.TransactionCategory.Add(category);
                    db.SaveChanges();
                }

                ModelState.Clear();
                return Content("Category Added Successfully");//return RedirectToAction("Index");
            }

            return Content("Invalid Category");
        }

        /**
         * Helper Method
         */
        private bool CategoryExists(string catname)
        {
            using (Model1 db = new Model1())
            {
                foreach (TransactionCategory cat in db.TransactionCategory)
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