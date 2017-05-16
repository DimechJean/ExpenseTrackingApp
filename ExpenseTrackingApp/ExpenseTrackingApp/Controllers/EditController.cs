using ExpenseTrackingApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class EditController : Controller
    {
        // GET: Edit
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Category(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("../Home");
            }

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                TransactionCategory cat = db.TransactionCategory.Find(id);
                if (cat == null)
                    return HttpNotFound();

                if (cat.UserAccount != user.ID)
                {
                    return RedirectToAction("../Home");
                }

                ViewBag.param_name = cat.NameCat;
            }
            ViewBag.param_id = id;
            return View();
        }

        // note 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Category(TransactionCategory category)
        {
            HttpCookie auth = Request.Cookies["auth"];
            string email = auth.Values.Get("Email");
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    if (AddCategoryController.CategoryExists(category.NameCat,email))
                    {
                        return RedirectToAction("Add", "AddCategory");
                    }

                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                ModelState.Clear();
                TempData["notice"] = "Category Edited Successfully";
                return RedirectToAction("TransactionCategories", "View");
            }
            return Content("Invalid Category");
        }

        public ActionResult Transaction(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("../Home");
            }
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                TransactionPersonal tp = db.TransactionPersonal.Find(id);
                PersonalAccount personal = db.PersonalAccount.Where(m => m.ID == tp.Account).First();
                if(personal.UserAccount != user.ID)
                {
                    return RedirectToAction("../Home");
                }

                if (tp == null)
                    return HttpNotFound();

                List<SelectListItem> categories = new List<SelectListItem>();
                List<SelectListItem> accounts = new List<SelectListItem>();
                var table = db.TransactionCategory.ToArray();
                foreach (var cat in table)
                {
                    SelectListItem newSLI = new SelectListItem();
                    newSLI.Text = cat.NameCat;
                    newSLI.Value = Convert.ToString(cat.ID);
                    categories.Add(newSLI);
                }
                UserAccount useracc = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                var accountTable = db.PersonalAccount.ToArray().Where(m => m.UserAccount == useracc.ID);
                foreach (var acc in accountTable)
                {
                    SelectListItem newSLI = new SelectListItem();
                    newSLI.Text = acc.AccountDescription;
                    newSLI.Value = Convert.ToString(acc.ID);
                    accounts.Add(newSLI);

                    if(acc.ID == tp.Account)
                        ViewBag.accountSelected = acc.AccountDescription;
                }
                string month;
                if (tp.DateAdded.Month < 10)
                    month = "0" + tp.DateAdded.Month;
                else
                    month = tp.DateAdded.Month.ToString();
                string day;
                if (tp.DateAdded.Day < 10)
                    day = "0" + tp.DateAdded.Day;
                else
                    day = tp.DateAdded.Day.ToString();
                ViewBag.param_date = tp.DateAdded.Year + "-" + month + "-" + day;
                ViewData["Categ"] = new List<SelectListItem>(categories);
                ViewData["Accts"] = new List<SelectListItem>(accounts);
                ViewBag.param_desc = tp.TransactionDescription;
                ViewBag.param_amnt = tp.Amount;
                ViewBag.categorySelected = tp.TransactionCategory1.NameCat;       
            }
            ViewBag.param_id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transaction(TransactionPersonal transaction)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                ModelState.Clear();
                TempData["notice"] = "Transaction Edited Successfully";
                return RedirectToAction("Transactions", "View");
            }
            return Content("Invalid Category");
        }

        public ActionResult TransactionOnline(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("../Home");
            }
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                TransactionOnline to = db.TransactionOnline.Find(id);
                OnlineAccount online = db.OnlineAccount.Where(m => m.ID == to.Account).First();
                if(online.UserAccount != user.ID)
                {
                    return RedirectToAction("../Home");
                }
                if (to == null)
                    return HttpNotFound();

                List<SelectListItem> categories = new List<SelectListItem>();
                var table = db.TransactionCategory.ToArray();
                foreach (var cat in table)
                {
                    SelectListItem newSLI = new SelectListItem();
                    newSLI.Text = cat.NameCat;
                    newSLI.Value = Convert.ToString(cat.ID);
                    categories.Add(newSLI);
                }
                UserAccount useracc = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                var accountTable = db.PersonalAccount.ToArray().Where(m => m.UserAccount == useracc.ID);
                foreach (var acc in accountTable)
                {
                    if (acc.ID == to.Account)
                        ViewBag.accountSelected = acc.AccountDescription;
                }
                string month;
                if (to.DateAdded.Month < 10)
                    month = "0" + to.DateAdded.Month;
                else
                    month = to.DateAdded.Month.ToString();
                string day;
                if (to.DateAdded.Day < 10)
                    day = "0" + to.DateAdded.Day;
                else
                    day = to.DateAdded.Day.ToString();
                ViewBag.param_date = to.DateAdded.Year + "-" + month + "-" + day;
                ViewData["Categ"] = new List<SelectListItem>(categories);
                ViewBag.param_desc = to.TransactionDescription;
                ViewBag.param_amnt = to.Amount;
                ViewBag.categorySelected = to.TransactionCategory1.NameCat;
            }
            ViewBag.param_id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransactionOnline(TransactionOnline transaction)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                ModelState.Clear();
                TempData["notice"] = "Transaction Edited Successfully";
                return RedirectToAction("Transactions", "View");
            }
            return Content("Invalid Category");
        }
    }
}