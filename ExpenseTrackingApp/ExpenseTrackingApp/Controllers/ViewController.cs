using ExpenseTrackingApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class ViewController : Controller
    {
        public ActionResult TransactionCategories()
        {
            List<TransactionCategory> catList = new List<TransactionCategory>();
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            using (Model1 db = new Model1())
            {
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                foreach (TransactionCategory tc in db.TransactionCategory)
                {
                    if (tc.UserAccount == null || tc.UserAccount == user.ID)
                        catList.Add(tc);
                }
            }

            ViewData["CatList"] = new List<TransactionCategory>(catList);

            return View();
        }

        public ActionResult Transactions()
        {
            List<TransactionPersonal> transList = new List<TransactionPersonal>(); 
            List<string> categories = new List<string>();
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            using (Model1 db = new Model1())
            {
                string email = auth.Values.Get("Email");
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                List<PersonalAccount> personal = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
                foreach (PersonalAccount p in personal)
                {
                    foreach(TransactionPersonal t in db.TransactionPersonal.Where(m=>m.Account == p.ID))
                    {
                        transList.Add(t);
                        categories.Add(t.TransactionCategory1.NameCat);
                    }
                }
            }

            ViewData["TransList"] = new List<TransactionPersonal>(transList);
            ViewData["categories"] = new List<string>(categories);

            return View();
        }

        public ActionResult TransactionsOnline()
        {
            
            List<TransactionOnline> transList = new List<TransactionOnline>(); 
            List<string> categories = new List<string>();
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            using (Model1 db = new Model1())
            {
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                List<OnlineAccount> online = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
                foreach(OnlineAccount o in online)
                {
                    foreach(TransactionOnline to in db.TransactionOnline.Where(m=>m.Account == o.ID))
                    {
                        transList.Add(to);
                        categories.Add(to.TransactionCategory1.NameCat);
                    }
                }
            }

            ViewData["TransList"] = new List<TransactionOnline>(transList); 
            ViewData["categories"] = new List<string>(categories);

            return View();
        }

        public ActionResult AllTransactions()
        {
            List<TransactionPersonal> transPersList = new List<TransactionPersonal>();
            List<TransactionOnline> transOnlineList = new List<TransactionOnline>();
            List<string> categories = new List<string>();
            HttpCookie auth = Request.Cookies["auth"];
            if (auth == null)
            {
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            using (Model1 db = new Model1())
            {
                UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).FirstOrDefault();
                List<OnlineAccount> online = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
                List<PersonalAccount> personal = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
                foreach (PersonalAccount p in personal)
                {
                    foreach (TransactionPersonal t in db.TransactionPersonal.Where(m => m.Account == p.ID))
                    {
                        transPersList.Add(t);
                        categories.Add(t.TransactionCategory1.NameCat);
                    }
                }
                foreach (OnlineAccount o in online)
                {
                    foreach (TransactionOnline to in db.TransactionOnline.Where(m => m.Account == o.ID))
                    {
                        transOnlineList.Add(to);
                        categories.Add(to.TransactionCategory1.NameCat);
                    }
                }
            }
            ViewData["TransPersList"] = new List<TransactionPersonal>(transPersList);
            ViewData["TransOnlineList"] = new List<TransactionOnline>(transOnlineList);
            ViewData["categories"] = new List<string>(categories);

            return View();
        }
    }
}