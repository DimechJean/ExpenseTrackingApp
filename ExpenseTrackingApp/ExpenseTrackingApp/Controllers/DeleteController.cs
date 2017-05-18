using ExpenseTrackingApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class DeleteController : Controller
    {
        public ActionResult Category(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
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
                else
                {
                    if (cat.UserAccount != user.ID)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    db.Entry(cat).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            TempData["notice"] = "Category Deleted Successfully";
            return RedirectToAction("TransactionCategories", "View");
        }

        public ActionResult Transaction(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                TransactionPersonal tp = db.TransactionPersonal.Find(id);
                if (tp == null)
                    return HttpNotFound();
                else
                {
                    PersonalAccount personal = db.PersonalAccount.Where(m => m.ID == tp.Account).First();
                    if (personal.UserAccount != user.ID)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    db.Entry(tp).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            TempData["notice"] = "Transaction Deleted Successfully";
            return RedirectToAction("Transactions", "View");
        }

        public ActionResult TransactionOnline(int? id)
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("Login", "UserAccounts");
            }

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                TransactionOnline tp = db.TransactionOnline.Find(id);
                if (tp == null)
                    return HttpNotFound();
                else
                {
                    OnlineAccount online = db.OnlineAccount.Where(m => m.ID == tp.Account).First();
                    if (online.UserAccount != user.ID)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    db.Entry(tp).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            TempData["notice"] = "Transaction Deleted Successfully";
            return RedirectToAction("Transactions", "View");
        }
    }
}