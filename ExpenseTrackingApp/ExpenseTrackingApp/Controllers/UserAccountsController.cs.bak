using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpenseTrackingApp.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace ExpenseTrackingApp.Controllers
{
    public class UserAccountsController : Controller
    {
        private Model1 db = new Model1();

        // GET: UserAccounts
        public ActionResult Index()
        {
            return RedirectToAction("../Home");
        }

        // GET: UserAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmailAcc,PasswordAcc,NameAcc,SurnameAcc")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                int ID = 0;
                foreach (UserAccount user in db.UserAccount)
                {
                    ID++;
                }
                userAccount.ID = ID;
                string password = hashPassword(userAccount.PasswordAcc);
                userAccount.PasswordAcc = password;
                if (UserExists(userAccount.EmailAcc))
                {
                    return View("UserExist");
                }
                db.UserAccount.Add(userAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userAccount);
        }

        //GET: UserAccounts/Login

        public ActionResult Login()
        {
            return View();
        }

        //Post: UserAccounts/Login

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login([Bind(Include = "EmailAcc,PasswordAcc")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                string password = hashPassword(userAccount.PasswordAcc);
                userAccount.PasswordAcc = password;
                foreach(UserAccount user in db.UserAccount)
                {
                    if(user.EmailAcc.Equals(userAccount.EmailAcc) && user.PasswordAcc.Equals(userAccount.PasswordAcc))
                    {
                        FormsAuthentication.SetAuthCookie(userAccount.NameAcc+ " " + userAccount.SurnameAcc,true);
                        return RedirectToAction("Index");
                    }
                }
                return View("UserNotExist");
            }
            return View(userAccount);
        }

        //Get: UserAccounts/LogOff

        [HttpGet]

        public ActionResult LogOff()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("Index");
        }

        // GET: UserAccounts/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccount.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmailAcc,PasswordAcc,NameAcc,SurnameAcc")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccount.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            UserAccount userAccount = db.UserAccount.Find(id);
            db.UserAccount.Remove(userAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool UserExists(string username)
        {
            foreach (UserAccount user in db.UserAccount)
            {
                if (user.EmailAcc == username)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult EmailAddressDB(UserAccount email)
        {
            var result = true;
            UserAccount user2 = null;
            foreach (UserAccount user in db.UserAccount)
            {
                if (user.EmailAcc == email.EmailAcc)
                {
                    user2 = user;
                }
            }
            if (user2 != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Hashing Algorithm
        public string hashPassword(string password)
        {             
            SHA512Managed sha = new SHA512Managed();
            byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedpass = Convert.ToBase64String(hashed);
            return hashedpass;
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
