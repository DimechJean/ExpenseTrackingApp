﻿using ExpenseTrackingApp.Models;

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
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Add()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("Login","UserAccounts");
            }
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
                
                    string email = auth.Values.Get("Email");
                    var user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).ToList();
                    UserAccount user2 = user.ElementAt(0);
                    //Select all accounts where the user account is the one logged in
                    ViewBag.AccountDescription = new SelectList(db.PersonalAccount.Where(u => u.UserAccount == user2.ID).ToList(), "ID", "AccountDescription");
                    ViewData["Categ"] = new List<SelectListItem>(categories);
                    return View();
            }
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

        public ActionResult AddOnline()
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
        public ActionResult AddOnline(TransactionOnline transaction)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    int ID = 0;
                    foreach (TransactionOnline tp in db.TransactionOnline)
                    {
                        ID++;
                    }
                    transaction.ID = ID;

                    db.TransactionOnline.Add(transaction);
                    db.SaveChanges();
                }
                ModelState.Clear();

                return Content("Transaction Added Successfully");//return RedirectToAction("Index");
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

                    int newID = 0;
                    foreach (TransactionPersonal tp in db.TransactionPersonal)
                    {
                        newID++;
                    }

                    TransactionPersonal newTransaction = new TransactionPersonal();
                    newTransaction.ID = newID+1;
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
            return RedirectToAction("AllTransactions", "View");
        }

        /*public ActionResult StarredOnline(int? id)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    TransactionOnline oldTransaction = db.TransactionOnline.Find(id);

                    int newID = 0;
                    foreach (TransactionOnline to in db.TransactionOnline)
                    {
                        newID++;
                    }

                    TransactionOnline newTransaction = new TransactionOnline();
                    newTransaction.ID = newID+1;
                    newTransaction.Amount = oldTransaction.Amount;
                    newTransaction.TransactionDescription = oldTransaction.TransactionDescription;
                    newTransaction.DateAdded = oldTransaction.DateAdded;
                    newTransaction.TransactionCategory = oldTransaction.TransactionCategory;

                    db.TransactionOnline.Add(newTransaction);
                    db.SaveChanges();
                }
                ModelState.Clear();
            }
            return RedirectToAction("AllTransactions", "View");
        }*/
    }
}