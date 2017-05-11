﻿using ExpenseTrackingApp.Models;

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
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionCategory cat = db.TransactionCategory.Find(id);
                if (cat == null)
                    return HttpNotFound();
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
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionPersonal tp = db.TransactionPersonal.Find(id);
                if (tp == null)
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

                ViewData["Categ"] = new List<SelectListItem>(categories);
                ViewBag.param_desc = tp.TransactionDescription;
                ViewBag.param_amnt = tp.Amount;
                ViewBag.param_date = tp.DateAdded;
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
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            using (Model1 db = new Model1())
            {
                TransactionOnline to = db.TransactionOnline.Find(id);
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

                ViewData["Categ"] = new List<SelectListItem>(categories);
                ViewBag.param_desc = to.TransactionDescription;
                ViewBag.param_amnt = to.Amount;
                ViewBag.param_date = to.DateAdded;
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