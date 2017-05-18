using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpenseTrackingApp.Models;

namespace ExpenseTrackingApp.Controllers
{
    public class PersonalAccountController : Controller
    {
        private Model1 db = new Model1();

        // GET: PersonalAccount
        public ActionResult Index()
        {
            HttpCookie auth = Request.Cookies["auth"];
            if (auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
            var personalAccount = db.PersonalAccount.Include(p => p.FinancialAccountsCategory).Include(p => p.UserAccount1).Where(m=>m.UserAccount == user.ID);
            return View(personalAccount.ToList());
        }

        // GET: PersonalAccount/Create
        public ActionResult Create()
        {
            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat");
            return View();
        }

        // POST: PersonalAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccountDescription,CategoryAcc,UserAccount")] PersonalAccount personalAccount)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if (auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            if (ModelState.IsValid)
            {
                decimal maxId;
                if (db.PersonalAccount.Count() == 0)
                    maxId = 0;
                else
                {
                    maxId = db.PersonalAccount.Max(x => x.ID);
                    maxId++;
                }
                personalAccount.ID = maxId;
                string myEmail = auth.Values.Get("Email");
                var temp = db.UserAccount.Where(x => x.EmailAcc == myEmail).FirstOrDefault();
                personalAccount.UserAccount = temp.ID;
                db.PersonalAccount.Add(personalAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat", personalAccount.CategoryAcc);
            ViewBag.UserAccount = new SelectList(db.UserAccount, "ID", "EmailAcc", personalAccount.UserAccount);
            return View(personalAccount);
        }

        // GET: PersonalAccount/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
            if(personalAccount.UserAccount != user.ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat", personalAccount.CategoryAcc);
            ViewBag.UserAccount = new SelectList(db.UserAccount, "ID", "EmailAcc", personalAccount.UserAccount);
            return View(personalAccount);
        }

        // POST: PersonalAccount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountDescription,CategoryAcc")] PersonalAccount personalAccount)
        {
            if (ModelState.IsValid)
            {
                HttpCookie auth = Request.Cookies["auth"];
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                personalAccount.UserAccount = user.ID;
                db.Entry(personalAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat", personalAccount.CategoryAcc);
            ViewBag.UserAccount = new SelectList(db.UserAccount, "ID", "EmailAcc", personalAccount.UserAccount);
            return View(personalAccount);
        }

        // GET: PersonalAccount/Delete/5
        public ActionResult Delete(decimal id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login","UserAccounts");
            }
            string email = auth.Values.Get("Email");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
            if(personalAccount.UserAccount != user.ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            return View(personalAccount);
        }

        // POST: PersonalAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
            db.PersonalAccount.Remove(personalAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
