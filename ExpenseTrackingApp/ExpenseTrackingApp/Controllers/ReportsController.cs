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
                TempData["notice"] = "You Need to be Logged to Use this Feature";
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
                TempData["notice"] = "You Need to be Logged to Use this Feature";
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
            List<OnlineAccount> OAccounts = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
            List<TransactionOnline> TransactionsO = new List<TransactionOnline>();
            List<TransactionPersonal> Transactions = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.DateAdded >= report.From && m.DateAdded <= report.To).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    Transactions.Add(tr[i]);
                }
            }
            foreach(OnlineAccount o in OAccounts)
            {
                List<TransactionOnline> tr = db.TransactionOnline.Where(m => m.DateAdded >= report.From && m.DateAdded <= report.To).Where(m => o.ID.Equals(m.Account)).ToList();
                for(int i = 0; i < tr.Count; i++)
                {
                    TransactionsO.Add(tr[i]);
                }
            }
            if(report.From.Year == report.To.Year)
            {
                int monthFrom = report.From.Month;
                int monthTo = report.To.Month;
                int totalMonths = monthTo - monthFrom ;
                List<TransactionPersonal>[] transactionsMonth = new List<TransactionPersonal>[totalMonths+1];
                List<TransactionOnline>[] transactionsMonthO = new List<TransactionOnline>[totalMonths + 1];
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
                k = monthFrom;
                for(int i = 0; i <= totalMonths; i++)
                {
                    transactionsMonthO[i] = new List<TransactionOnline>();
                    for(int j = 0; j < TransactionsO.Count; j++)
                    {
                        if (TransactionsO.ElementAt(j).DateAdded.Month.Equals(k))
                        {
                            transactionsMonthO[i].Add(TransactionsO.ElementAt(j));
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
                    for(int j = 0; j < transactionsMonthO[i].Count; j++)
                    {
                        totalPerMonth[i] += Convert.ToDouble(transactionsMonthO[i].ElementAt(j).Amount);
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
                List<TransactionOnline>[] transactionsYearO = new List<TransactionOnline>[totalYears + 1];
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
                    for(int j = 0; j < TransactionsO.Count; j++)
                    {
                        if (TransactionsO[j].DateAdded.Year.Equals(k))
                        {
                            transactionsYearO[i].Add(TransactionsO[j]);
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
                    for(int j = 0; j < transactionsYearO[i].Count; j++)
                    {
                        totalPerYear[i] += Convert.ToDouble(transactionsYearO[i].ElementAt(j).Amount);
                    }
                }
                ViewBag.TableTotal = totalPerYear;
            }
            int noCategories;
            List<TransactionCategory> Category = db.TransactionCategory.Where(m=>m.UserAccount == user.ID || m.UserAccount == null).ToList();
            noCategories = Category.Count;
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
                TempData["notice"] = "You Need to be Logged to Use this Feature";
                return RedirectToAction("Login", "UserAccounts");
            }
            List<TransactionCategory> Category = db.TransactionCategory.ToList();
            List<TransactionPersonal> transactionWeek = TransactionWeek();
            List<TransactionPersonal> transactionMonth = TransactionsMonth();
            List<TransactionPersonal> transactionYear = TransactionsYear();
            List<TransactionPersonal> transactionDay = TransactionsDay();
            List<TransactionOnline> transactionWeekO = TransactionsWeekO();
            List<TransactionOnline> transactionMonthO = TransactionsMonthO();
            List<TransactionOnline> transactionYearO = TransactionsYearO();
            List<TransactionOnline> transactionDayO = TransactionsDayO();
            List<TransactionPersonal>[] tranDay = new List<TransactionPersonal>[Category.Count];
            List<TransactionPersonal>[] tranWeek = new List<TransactionPersonal>[Category.Count];
            List<TransactionPersonal>[] tranMonth = new List<TransactionPersonal>[Category.Count];
            List<TransactionPersonal>[] tranYear = new List<TransactionPersonal>[Category.Count];
            List<TransactionOnline>[] tranDayO = new List<TransactionOnline>[Category.Count];
            List<TransactionOnline>[] tranWeekO = new List<TransactionOnline>[Category.Count];
            List<TransactionOnline>[] tranMonthO = new List<TransactionOnline>[Category.Count];
            List<TransactionOnline>[] tranYearO = new List<TransactionOnline>[Category.Count];
            double[] totalDay = new double[Category.Count];
            double[] totalWeek = new double[Category.Count];
            double[] totalMonth = new double[Category.Count];
            double[] totalYear = new double[Category.Count];
            for (int j=0;j<Category.Count;j++)
            {
                tranDay[j] = new List<TransactionPersonal>();
                tranWeek[j] = new List<TransactionPersonal>();
                tranMonth[j] = new List<TransactionPersonal>();
                tranYear[j] = new List<TransactionPersonal>();
                tranDayO[j] = new List<TransactionOnline>();
                tranWeekO[j] = new List<TransactionOnline>();
                tranMonthO[j] = new List<TransactionOnline>();
                tranYearO[j] = new List<TransactionOnline>();
                for(int i = 0; i < transactionDay.Count; i++)
                {
                    if(transactionDay.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranDay[j].Add(transactionDay.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionWeek.Count; i++)
                {
                    if(transactionWeek.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranWeek[j].Add(transactionWeek.ElementAt(i));
                    }
                }
                for(int i = 0; i < transactionDayO.Count; i++)
                {
                    if(transactionDayO.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranDayO[j].Add(transactionDayO.ElementAt(i));
                    }
                }
                for(int i = 0; i < transactionWeekO.Count; i++)
                {
                    if(transactionWeekO.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranWeekO[j].Add(transactionWeekO.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionMonth.Count; i++)
                {
                    if (transactionMonth.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranMonth[j].Add(transactionMonth.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionMonthO.Count; i++)
                {
                    if (transactionMonthO.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranMonthO[j].Add(transactionMonthO.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionYear.Count; i++)
                {
                    if (transactionYear.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranYear[j].Add(transactionYear.ElementAt(i));
                    }
                }
                for (int i = 0; i < transactionYearO.Count; i++)
                {
                    if (transactionYearO.ElementAt(i).TransactionCategory == Category[j].ID)
                    {
                        tranYearO[j].Add(transactionYearO.ElementAt(i));
                    }
                }
            }
            for(int i = 0; i < Category.Count; i++)
            {
                double total = 0;
                for(int j = 0; j < tranDay[i].Count; j++)
                {
                    total += Convert.ToDouble(tranDay[i][j].Amount);
                }
                for (int j = 0; j < tranDayO[i].Count; j++)
                {
                    total += Convert.ToDouble(tranDayO[i][j].Amount);
                }
                totalDay[i] = total;
                total = 0;
                for (int j = 0; j < tranWeek[i].Count; j++)
                {
                    total += Convert.ToDouble(tranWeek[i][j].Amount);
                }
                for(int j = 0; j < tranWeekO[i].Count; j++)
                {
                    total += Convert.ToDouble(tranWeekO[i][j].Amount);
                }
                totalWeek[i] = total;
                total = 0;
                for (int j = 0; j < tranMonth[i].Count; j++)
                {
                    total += Convert.ToDouble(tranMonth[i][j].Amount);
                }
                for (int j = 0; j < tranMonthO[i].Count; j++)
                {
                    total += Convert.ToDouble(tranMonthO[i][j].Amount);
                }
                totalMonth[i] = total;
                total = 0;
                for (int j = 0; j < tranYear[i].Count; j++)
                {
                    total += Convert.ToDouble(tranYear[i][j].Amount);
                }
                for (int j = 0; j< tranYearO[i].Count; j++)
                {
                    total += Convert.ToDouble(tranYearO[i][j].Amount);
                }
                totalYear[i] = total;
            }
            ViewBag.transactionsDay = totalDay;
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
        public List<TransactionOnline> TransactionsWeekO()
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
            List<OnlineAccount> Accounts = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
            List<TransactionOnline> transactionsWeek = new List<TransactionOnline>();
            foreach (OnlineAccount a in Accounts)
            {
                List<TransactionOnline> tr = db.TransactionOnline.Where(m => m.DateAdded >= monday && m.DateAdded <= now).Where(m => a.ID.Equals(m.Account)).ToList();
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

        public List<TransactionOnline> TransactionsMonthO()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<OnlineAccount> Accounts = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, days);
            DateTime startMonth = new DateTime(thisMonth.Year, thisMonth.Month, 1);
            List<TransactionOnline> transactionsMonth = new List<TransactionOnline>();
            foreach (OnlineAccount a in Accounts)
            {
                List<TransactionOnline> tr = db.TransactionOnline.Where(m => m.DateAdded >= startMonth && m.DateAdded <= thisMonth).Where(m => a.ID.Equals(m.Account)).ToList();
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
        public List<TransactionOnline> TransactionsYearO()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<OnlineAccount> Accounts = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
            DateTime thisYear = new DateTime(DateTime.Now.Year, 12, 31);
            DateTime startYear = new DateTime(thisYear.Year, 1, 1);
            List<TransactionOnline> transactionsYear = new List<TransactionOnline>();
            foreach (OnlineAccount a in Accounts)
            {
                List<TransactionOnline> tr = db.TransactionOnline.Where(m => m.DateAdded >= startYear && m.DateAdded <= thisYear).Where(m => a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsYear.Add(tr[i]);
                }
            }
            return transactionsYear;
        }
        public List<TransactionPersonal> TransactionsDay()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<PersonalAccount> Accounts = db.PersonalAccount.Where(m => m.UserAccount == user.ID).ToList();
            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day);
            DateTime tomorrow = new DateTime(today.Year, today.Month, today.Day + 1);
            List<TransactionPersonal> transactionsDay = new List<TransactionPersonal>();
            foreach (PersonalAccount a in Accounts)
            {
                List<TransactionPersonal> tr = db.TransactionPersonal.Where(m => m.Account.Equals(a.ID) && m.DateAdded.Equals(today)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsDay.Add(tr[i]);
                }
            }
            return transactionsDay;
        }

        public List<TransactionOnline> TransactionsDayO()
        {
            HttpCookie cookie = Request.Cookies["auth"];
            string email = cookie.Values.Get("Email");
            UserAccount user = db.UserAccount.Where(m => m.EmailAcc == email).FirstOrDefault();
            List<OnlineAccount> Accounts = db.OnlineAccount.Where(m => m.UserAccount == user.ID).ToList();
            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day);
            List<TransactionOnline> transactionsDay = new List<TransactionOnline>();
            foreach (OnlineAccount a in Accounts)
            {
                List<TransactionOnline> tr = db.TransactionOnline.Where(m => m.DateAdded.Equals(today) && a.ID.Equals(m.Account)).ToList();
                for (int i = 0; i < tr.Count; i++)
                {
                    transactionsDay.Add(tr[i]);
                }
            }
            return transactionsDay;
        }
    }
}