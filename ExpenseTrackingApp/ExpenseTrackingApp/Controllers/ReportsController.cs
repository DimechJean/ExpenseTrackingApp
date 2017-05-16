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
            if(auth == null)
            {
                return RedirectToAction("Login", "UserAccounts");
            }
            if(report.From > report.To)
            {
                TempData["notice"] = "Th Date To cannot be earlier than From";
                return RedirectToAction("Index","Reports");
            }
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
                    }
                    k++;
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

        //GET Reports/Reporting
        public ActionResult Reporting()
        {
            HttpCookie auth = Request.Cookies["auth"];
            if(auth == null)
            {
                return RedirectToAction("Login", "UserAccounts");
            }
            List<TransactionCategory> Category = db.TransactionCategory.ToList();
            List<TransactionPersonal> transactionWeek = TransactionWeek();
            List<TransactionPersonal> transactionMonth = TransactionsMonth();
            List<TransactionPersonal> transactionYear = TransactionsYear();
            List<TransactionPersonal>[] tranWeek = new List<TransactionPersonal>[Category.Count];
            List<TransactionPersonal>[] tranMonth = new List<TransactionPersonal>[Category.Count];
            List<TransactionPersonal>[] tranYear = new List<TransactionPersonal>[Category.Count];
            double[] totalWeek = new double[Category.Count];
            double[] totalMonth = new double[Category.Count];
            double[] totalYear = new double[Category.Count];
            for (int j=0;j<Category.Count;j++)
            {
                tranWeek[j] = new List<TransactionPersonal>();
                tranMonth[j] = new List<TransactionPersonal>();
                tranYear[j] = new List<TransactionPersonal>();
                for (int i = 0; i < transactionWeek.Count; i++)
                {
                    if(transactionWeek.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranWeek[j].Add(transactionWeek.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionMonth.Count; i++)
                {
                    if (transactionMonth.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranMonth[j].Add(transactionMonth.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionYear.Count; i++)
                {
                    if (transactionYear.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranYear[j].Add(transactionYear.ElementAt(i));
                    }
                }
            }
            for(int i = 0; i < Category.Count; i++)
            {
                double total = 0;
                for(int j = 0; j < tranWeek[i].Count; j++)
                {
                    total += Convert.ToDouble(tranWeek[i][j].Amount);
                }
                totalWeek[i] = total;
                total = 0;
                for (int j = 0; j < tranMonth[i].Count; j++)
                {
                    total += Convert.ToDouble(tranMonth[i][j].Amount);
                }
                totalMonth[i] = total;
                total = 0;
                for (int j = 0; j < tranYear[i].Count; j++)
                {
                    total += Convert.ToDouble(tranYear[i][j].Amount);
                }
                totalYear[i] = total;
            }
            ViewBag.transactionsWeek = totalWeek;
            ViewBag.transactionsMonth = totalMonth;
            ViewBag.transactionsYear = totalYear;
            ViewBag.Category = Category;
            return View();
        }

        public List<TransactionPersonal> TransactionWeek()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            DateTime now = DateTime.Now;
            int delta = DayOfWeek.Monday - now.DayOfWeek;
            if (delta > 0)
            {
                delta -= 7;
            }
            DateTime monday = now.AddDays(delta);
            int day = monday.Day + 6;
            now = new DateTime(monday.Year, monday.Month, day);
            List<PersonalAccount> Accounts = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
            List<TransactionPersonal> transactionsWeek = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.DateAdded >= monday && m.DateAdded <= now).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsWeek.Add(tr[i]);
                }
            }
            return transactionsWeek;
        }

        public List<TransactionPersonal> TransactionsMonth()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<PersonalAccount> Accounts = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            DateTime startMonth = new DateTime(thisMonth.Year, thisMonth.Month, 1);
            List<TransactionPersonal> transactionsMonth = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.DateAdded >= startMonth && m.DateAdded <= thisMonth).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsMonth.Add(tr[i]);
                }
            }
            return transactionsMonth;
        }
        public List<TransactionPersonal> TransactionsYear()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<PersonalAccount> Accounts = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
            DateTime thisYear = new DateTime(DateTime.Now.Year,12,31);
            DateTime startYear = new DateTime(thisYear.Year, 1, 1);
            List<TransactionPersonal> transactionsYear = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.DateAdded >= startYear && m.DateAdded <= thisYear).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsYear.Add(tr[i]);
                }
            }
            return transactionsYear;
        }
    }
}