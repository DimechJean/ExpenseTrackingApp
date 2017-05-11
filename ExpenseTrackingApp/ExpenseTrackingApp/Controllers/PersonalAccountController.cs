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
            var personalAccount = db.PersonalAccount.Include(p => p.FinancialAccountsCategory).Include(p => p.UserAccount1);
            return View(personalAccount.ToList());
        }

        // GET: PersonalAccount/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            return View(personalAccount);
        }

        // GET: PersonalAccount/Create
        public ActionResult Create()
        {
            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat");
            ViewBag.UserAccount = new SelectList(db.UserAccount, "ID", "EmailAcc");
            return View();
        }

        // POST: PersonalAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccountDescription,CategoryAcc,UserAccount")] PersonalAccount personalAccount)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if (auth == null)
            {
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
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
            if (personalAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryAcc = new SelectList(db.FinancialAccountsCategory, "ID", "NameCat", personalAccount.CategoryAcc);
            ViewBag.UserAccount = new SelectList(db.UserAccount, "ID", "EmailAcc", personalAccount.UserAccount);
            return View(personalAccount);
        }

        // POST: PersonalAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountDescription,CategoryAcc")] PersonalAccount personalAccount)
        {
            if (ModelState.IsValid)
            {
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalAccount personalAccount = db.PersonalAccount.Find(id);
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
