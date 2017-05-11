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
        // GET: View
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult TransactionCategories()
        {
            List<TransactionCategory> catList = new List<TransactionCategory>();

            using (Model1 db = new Model1())
            {
                foreach(TransactionCategory tc in db.TransactionCategory)
                {
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

            using (Model1 db = new Model1())
            {
                foreach (TransactionPersonal tp in db.TransactionPersonal)
                {
                    transList.Add(tp);
                    categories.Add(tp.TransactionCategory1.NameCat);
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

            using (Model1 db = new Model1())
            {
                foreach (TransactionOnline to in db.TransactionOnline)
                {
                    transList.Add(to);
                    categories.Add(to.TransactionCategory1.NameCat);
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

            using (Model1 db = new Model1())
            {
                foreach (TransactionPersonal tp in db.TransactionPersonal)
                {
                    transPersList.Add(tp);
                    categories.Add(tp.TransactionCategory1.NameCat);
                }

                foreach (TransactionOnline to in db.TransactionOnline)
                {
                    transOnlineList.Add(to);
                    categories.Add(to.TransactionCategory1.NameCat);
                }
            }

            ViewData["TransPersList"] = new List<TransactionPersonal>(transPersList);
            ViewData["TransOnlineList"] = new List<TransactionOnline>(transOnlineList);
            ViewData["categories"] = new List<string>(categories);

            return View();
        }
    }
}