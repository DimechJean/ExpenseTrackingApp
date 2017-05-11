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
        // GET: Delete
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Category(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionCategory cat = db.TransactionCategory.Find(id);
                if (cat == null)
                    return HttpNotFound();
                else
                {
                    db.Entry(cat).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("TransactionCategories", "View");
        }

        public ActionResult Transaction(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionPersonal tp = db.TransactionPersonal.Find(id);
                if (tp == null)
                    return HttpNotFound();
                else
                {
                    db.Entry(tp).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("AllTransactions", "View");
        }

        public ActionResult TransactionOnline(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionOnline tp = db.TransactionOnline.Find(id);
                if (tp == null)
                    return HttpNotFound();
                else
                {
                    db.Entry(tp).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("AllTransactions", "View");
        }
    }
}