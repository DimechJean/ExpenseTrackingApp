using ExpenseTrackingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackingApp.Controllers
{
    public class ReportsController : Controller
    {
        Model1 db = new Model1();


        // GET: Reports

        [HttpGet]
        public ActionResult Index()
        {
            HttpCookie auth = Request.Cookies["auth"];
            if (auth == null)
            {
                return RedirectToAction("Login", "UserAccounts");
            }
            string email = auth.Values.Get("Email");
            Reports Report = new Reports();
            return View(Report);
        }

        //Post :Reports/Index

        [HttpPost]
        public ActionResult Index(Reports report)
        {
            HttpCookie auth = Request.Cookies["auth"];
            string email = auth.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc.Equals(email)).First();
            List<PersonalAccount> Accounts = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
            List<TransactionPersonal> Transactions = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.DateAdded >= report.From && m.DateAdded <= report.To).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    Transactions.Add(tr[i]);
                }
            }
            if(report.From.Year == report.To.Year)
            {
                int monthFrom = report.From.Month;
                int monthTo = report.To.Month;
                int totalMonths = monthTo - monthFrom ;
                List<TransactionPersonal>[] transactionsMonth = new List<TransactionPersonal>[totalMonths+1];
                int k = monthFrom;
                for(int i = 0; i <= totalMonths; i++)
                {
                    transactionsMonth[i] = new List<TransactionPersonal>();
                    for(int j = 0; j < Transactions.Count; j++)
                    {
                        if (Transactions.ElementAt(j).DateAdded.Month.Equals(k))
                        {
                            transactionsMonth[i].Add(Transactions.ElementAt(j));
                        }
                    }
                    k++;
                }
                double[] totalPerMonth = new double[totalMonths+1];
                for(int i = 0; i <= totalMonths; i++)
                {
                    for(int j = 0; j < transactionsMonth[i].Count; j++)
                    {
                        totalPerMonth[i] += Convert.ToDouble(transactionsMonth[i].ElementAt(j).Amount);
                    }
                }
                ViewBag.TableTotal = totalPerMonth;
            }
            else
            {
                int YearFrom = report.From.Year;
                int YearTo = report.To.Year;
                int totalYears = YearTo - YearFrom;
                List<TransactionPersonal>[] transactionsYear = new List<TransactionPersonal>[totalYears+1];
                int k = YearFrom;
                for (int i = 0; i <= totalYears; i++)
                {
                    transactionsYear[i] = new List<TransactionPersonal>();
                    for (int j = 0; j < Transactions.Count; j++)
                    {
                        if (Transactions[j].DateAdded.Year.Equals(k))
                        {
                            transactionsYear[i].Add(Transactions[j]);
                        }
                        k++;
                    }
                }
                double[] totalPerYear = new double[totalYears+1];
                for (int i = 0; i <= totalYears; i++)
                {
                    for (int j = 0; j < transactionsYear[i].Count; j++)
                    {
                        totalPerYear[i] += Convert.ToDouble(transactionsYear[i].ElementAt(j).Amount);
                    }
                }
                ViewBag.TableTotal = totalPerYear;
            }
            int noCategories = db.TransactionCategory.ToList().Count;
            List<TransactionCategory> Category = db.TransactionCategory.ToList();
            List<TransactionPersonal>[] categories = new List<TransactionPersonal>[noCategories]; 
            for(int i = 0; i < noCategories; i++)
            {
                categories[i] = new List<TransactionPersonal>();
                for(int j = 0; j < Transactions.Count; j++)
                {
                    if(Transactions[j].TransactionCategory == i)
                    {
                        categories[i].Add(Transactions[j]);
                    }
                }
            }
            ViewBag.Categories = categories;
            ViewBag.CategoryName = Category;
            ViewBag.DateFrom = report.From;
            ViewBag.DateTo = report.To;
            return PartialView("Report");
        }
    }
}