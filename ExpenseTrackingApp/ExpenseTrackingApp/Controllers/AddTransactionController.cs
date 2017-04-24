using ExpenseTrackingApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class AddTransactionController : Controller
    {
        // GET: AddTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            using (Model1 db = new Model1())
            {
                var table = db.TransactionCategory.ToArray();

                foreach (var cat in table)
                {
                    SelectListItem newSLI = new SelectListItem();
                    newSLI.Text = cat.NameCat;
                    newSLI.Value = Convert.ToString(cat.ID);
                    categories.Add(newSLI);
                }
            }

            ViewData["Categ"] = new List<SelectListItem>(categories);

            return View();
        }

        [HttpPost]
        public ActionResult Add(TransactionPersonal transaction)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    int ID = 0;
                    foreach (TransactionPersonal tp in db.TransactionPersonal)
                    {
                        ID++;
                    }
                    transaction.ID = ID;

                    db.TransactionPersonal.Add(transaction);
                    db.SaveChanges();
                }
                ModelState.Clear();

                return Content("Transaction Added Successfully");//return RedirectToAction("Index");
            }

            return Content("Invalid Transaction");
        }
    }
}