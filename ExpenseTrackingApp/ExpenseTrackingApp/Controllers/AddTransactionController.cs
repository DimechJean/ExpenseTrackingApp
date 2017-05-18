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
        public ActionResult Add()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            List<SelectListItem> accounts = new List<SelectListItem>();
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login","UserAccounts");
            }
            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount useracc = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                foreach (var cat in db.TransactionCategory)
                {
                    if (cat.UserAccount == null || cat.UserAccount == useracc.ID)
                    {
                        SelectListItem newSLI = new SelectListItem();
                        newSLI.Text = cat.NameCat;
                        newSLI.Value = Convert.ToString(cat.ID);
                        categories.Add(newSLI);
                    }
                }
                var accountTable = db.PersonalAccount.ToArray().Where(m=>m.UserAccount == useracc.ID);
                foreach (var acc in accountTable)
                {
                    SelectListItem newSLI = new SelectListItem();
                    newSLI.Text = acc.AccountDescription;
                    newSLI.Value = Convert.ToString(acc.ID);
                    accounts.Add(newSLI);
                }                
                
                var user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).ToList();
                UserAccount user2 = user.ElementAt(0);
                ViewData["Categ"] = new List<SelectListItem>(categories);
                ViewData["Accts"] = new List<SelectListItem>(accounts);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "ID,Amount,TransactionDescription,TransactionCategory,DateAdded,Account")]TransactionPersonal transaction)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    decimal maxId;
                    if (db.TransactionPersonal.Count() == 0)
                        maxId = 0;
                    else
                    {
                        maxId = db.TransactionPersonal.Max(x => x.ID);
                        maxId++;
                    }
                    transaction.ID = maxId;
                    db.TransactionPersonal.Add(transaction);
                    db.SaveChanges();
                }
                ModelState.Clear();

                TempData["notice"] = "Transaction Added Successfully";
                return RedirectToAction("Transactions", "View");
            }
            return Content("Invalid Transaction");
        }
        
        public ActionResult Starred(int? id)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    TransactionPersonal oldTransaction = db.TransactionPersonal.Find(id);
                    Random r = new Random();
                    int value = (int)r.Next(0, Int32.MaxValue);
                    TransactionPersonal account = db.TransactionPersonal.Find(value);
                    if (account != null)
                    {
                        while (account != null)
                        {
                            value = (int)r.Next(0, Int32.MaxValue);
                            account = db.TransactionPersonal.Find(value);
                        }
                    }
                    TransactionPersonal newTransaction = new TransactionPersonal();
                    newTransaction.ID = value;
                    newTransaction.Amount = oldTransaction.Amount;
                    newTransaction.TransactionDescription = oldTransaction.TransactionDescription;
                    newTransaction.DateAdded = oldTransaction.DateAdded;
                    newTransaction.TransactionCategory = oldTransaction.TransactionCategory;
                    newTransaction.Account = oldTransaction.Account;

                    db.TransactionPersonal.Add(newTransaction);
                    db.SaveChanges();
                }
                ModelState.Clear();
            }

            TempData["notice"] = "Transaction Added Successfully";                   
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}