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
using Facebook;
using Newtonsoft.Json.Linq;

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

        public ActionResult Login([Bind(Include = "EmailAcc,PasswordAcc")] UserAccount userAccount,string submit)
        {

                if (ModelState.IsValid)
                {
                    string password = hashPassword(userAccount.PasswordAcc);
                    userAccount.PasswordAcc = password;
                    foreach (UserAccount user in db.UserAccount)
                    {
                        if (user.EmailAcc.Equals(userAccount.EmailAcc) && user.PasswordAcc.Equals(userAccount.PasswordAcc))
                        {
                            string namesurname = user.NameAcc + " " + user.SurnameAcc;
                            string email = user.EmailAcc;
                            HttpCookie auth = new HttpCookie("auth");
                            auth.Values.Add("NameSurname", namesurname);
                            auth.Values.Add("Email", email);
                            auth.Expires = DateTime.Now.AddHours(12);
                            Response.Cookies.Add(auth);
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
            HttpCookie cookie = Request.Cookies["auth"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return Redirect("Index");
        }

        
        //GET: /UserAccounts/Edit
        public ActionResult Edit()
        {
            return View();
        }

        //GET: /UserAccounts/EditName

        public ActionResult EditName()
        {
            return View();
        }

        //GET: /UserAccounts/EditSurname

        public ActionResult EditSurname()
        {
            return View();
        }

        //GET: /UserAccounts/EditEmail

        public ActionResult EditEmail()
        {
            return View();
        }

        //GET: /UserAccounts/EditPassword

        public ActionResult EditPassword()
        {
            return View();
        }
        //POST: /UserAccounts/EditName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditName([Bind(Include = "NameAcc")] UserAccount userAccount)
        {
                userAccount.EmailAcc = "";
                userAccount.PasswordAcc = "";
                ModelState.Clear();
                if (ModelState.IsValid)
                {
                    HttpCookie auth = Request.Cookies["auth"];
                    string email = auth.Values.Get("Email");
                    UserAccount user2 = new UserAccount();
                    foreach (UserAccount user in db.UserAccount)
                    {
                        if (user.EmailAcc == email)
                        {
                            user2 = user;
                            break;
                        }
                    }

                    user2.NameAcc = userAccount.NameAcc;
                    db.Entry(user2).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("Index");
            }
            return View();
        }

        //POST: /UserAccounts/EditSurname

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditSurname([Bind(Include = "SurnameAcc")] UserAccount userAccount)
        {

            userAccount.EmailAcc = "";
            userAccount.PasswordAcc = "";
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                HttpCookie auth = Request.Cookies["auth"];
                string email = auth.Values.Get("Email");
                UserAccount user2 = new UserAccount();
                foreach (UserAccount user in db.UserAccount)
                {
                    if (user.EmailAcc == email)
                    {
                        user2 = user;
                        break;
                    }
                }

                user2.SurnameAcc = userAccount.SurnameAcc;
                db.Entry(user2).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("Index");
            }
            return View();
        }

        //POST: /UserAccounts/EditEmail

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult EditEmail([Bind(Include = "EmailAcc")] UserAccount userAccount)
        {
            userAccount.PasswordAcc = "";
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                HttpCookie auth = Request.Cookies["auth"];
                string email = auth.Values.Get("Email");
                var address = new System.Net.Mail.MailAddress(email);
                if(address.Address != email)
                {
                    return View("WrongEmail");
                }
                UserAccount user2 = new UserAccount();
                foreach (UserAccount user in db.UserAccount)
                {
                    if (user.EmailAcc == email)
                    {
                        user2 = user;
                        break;
                    }
                }
                if(email == "")
                {
                    return View("WrongEmail");
                }
                JsonResult obj = EmailAddressDB(userAccount);
                var result = obj.Data;
                if((bool)result == false)
                {
                    return View("UserExist");
                }
                user2.EmailAcc = userAccount.EmailAcc;
                db.Entry(user2).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("Index");
            }
            return View();
        }

        //POST: /UserAccounts/EditPassword

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditPassword([Bind(Include = "PasswordAcc")] UserAccount userAccount)
        {
            userAccount.EmailAcc = "";
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                HttpCookie auth = Request.Cookies["auth"];
                string email = auth.Values.Get("Email");
                UserAccount user2 = new UserAccount();
                foreach (UserAccount user in db.UserAccount)
                {
                    if (user.EmailAcc == email)
                    {
                        user2 = user;
                        break;
                    }
                }
                if(userAccount.PasswordAcc.Length < 6 || userAccount.PasswordAcc.Where(char.IsDigit).Count() == 0 || userAccount.PasswordAcc.Where(char.IsUpper).Count() == 0)
                {
                    return View("WrongPassword");
                }
                userAccount.PasswordAcc = hashPassword(userAccount.PasswordAcc);
                user2.PasswordAcc = userAccount.PasswordAcc;
                db.Entry(user2).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("Index");
            }
            return View();
        }

        // GET: UserAccounts/Delete/
        public ActionResult Delete()
        {
            HttpCookie auth = Request.Cookies["auth"];
            string email = auth.Values.Get("Email");
            UserAccount user = findUser(email);
            return View(user);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            HttpCookie auth = Request.Cookies["auth"];
            string email = auth.Values.Get("Email");
            UserAccount user = findUser(email);
            db.UserAccount.Remove(user);
            int id = Convert.ToInt32(db.UserAccount.OrderByDescending(i => i.ID).FirstOrDefault().ToString());
            if(user.ID != id)
            {
                foreach(UserAccount user2 in db.UserAccount)
                {
                    if(user2.ID > user.ID)
                    {
                        user2.ID -= 1;
                        db.Entry(user2).State = EntityState.Modified;
                    }
                }
            }
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
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "767483813401881",
                client_secret = "56216ff9ab795a33c74893fc1c0e85f6",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "767483813401881",
                client_secret = "56216ff9ab795a33c74893fc1c0e85f6",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=first_name,last_name,email");
            string email = me.email;
            string name = me.first_name;
            string surname = me.last_name;
            string url = Request.UrlReferrer.ToString();

            if (url.Contains("/UserAccounts/Login"))
            {
                //Check if users Exists and if user exists create session cookie and redirect to home
                foreach (UserAccount user in db.UserAccount)
                {
                    if (user.EmailAcc == email && user.NameAcc == name && user.SurnameAcc == surname)
                    {
                        string namesurname = user.NameAcc + " " + user.SurnameAcc;
                        HttpCookie auth = new HttpCookie("auth");
                        auth.Values.Add("NameSurname", namesurname);
                        auth.Values.Add("Email", user.EmailAcc);
                        auth.Expires = DateTime.Now.AddHours(12);
                        Response.Cookies.Add(auth);
                        return RedirectToAction("Index");
                    }
                }
                return View("UserNotExist");
            }
            else if (url.Contains("/UserAccounts/Create"))
            {
                //Check if user already exists
                foreach (UserAccount user in db.UserAccount)
                {
                    if (user.EmailAcc == email && user.NameAcc == name && user.SurnameAcc == surname)
                    {
                        return View("UserExist");
                    }
                }
                //Register User
                Random r = new Random();
                Random r2 = new Random();
                Random r3 = new Random();
                Random r4 = new Random();
                string password = "";
                for (int i = 0; i < 15; i++)
                {
                    int num = r.Next(0, 9);
                    int num2 = r.Next(65, 90);
                    int num3 = r.Next(97, 122);
                    int num4 = r.Next(40, 47);
                    char cnum = Convert.ToChar(num);
                    char cnum2 =Convert.ToChar(num2);
                    char cnum3 =Convert.ToChar(num3);
                    char cnum4 =Convert.ToChar(num4);
                    password += cnum.ToString() + cnum2.ToString() + cnum3.ToString() + cnum4.ToString();
                }
                password = hashPassword(password);
                //Increment ID
                int ID = 0;
                foreach (UserAccount user in db.UserAccount)
                {
                    ID++;
                }
                UserAccount user2 = new UserAccount();
                user2.ID = ID;
                user2.NameAcc = name;
                user2.SurnameAcc = surname;
                user2.EmailAcc = email;
                user2.PasswordAcc = password;
                db.UserAccount.Add(user2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Redirect("Create");
        }

        [HttpPost]

        //Check if Email Address exists in the database
        public JsonResult EmailAddressDB(UserAccount email)
        {
            string url = Request.UrlReferrer.ToString();
            var result = true;
            UserAccount user2 = null;
            if (url.Contains("/UserAccounts/Create") || url.Contains("/UserAccounts/EditEmail"))
            {
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
            }else
            {
                return Json(true, JsonRequestBehavior.AllowGet);  
            }
        }

        //Check if password contains at least one uppercase and one number
        public JsonResult PasswordDB(UserAccount password)
        {
            string url = Request.UrlReferrer.ToString();

            if (url.Contains("/UserAccounts/Create") || url.Contains("/UserAccounts/EditPassword"))
            {
                string pass = password.PasswordAcc;
                if(pass.Any(c=> char.IsDigit(c)) == false)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (pass.Any(c => char.IsUpper(c)) == false)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //Hashing Algorithm

        [HttpPost]
        public string hashPassword(string password)
        {             
            SHA512Managed sha = new SHA512Managed();
            byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedpass = Convert.ToBase64String(hashed);
            return hashedpass;
        }

        [HttpPost]
        public UserAccount findUser(string email)
        {
            foreach (UserAccount user in db.UserAccount)
            {
                if (user.EmailAcc == email)
                {
                    return user;
                }
            }
            return null;
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
