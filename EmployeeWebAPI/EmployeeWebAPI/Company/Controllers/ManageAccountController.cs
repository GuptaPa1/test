using Company.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Company.Controllers
{
    public class ManageAccountController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        // GET: Account
        public ActionResult Signup(UserModel user)
        {
            user.BirthDate = DateTime.Now;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<UserModel>("api/Account/Register", user);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Signin");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(user);

        }

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(LoginModel login)
        {
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(apiUrl);
            //    client.
            //    login.grant_type = "password";
            //    //HTTP POST
            //    var postTask = client.PostAsJsonAsync<LoginModel>("token", login);
            //    postTask.Wait();

            //    var result = postTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        return RedirectToAction("GetAllEmployees");
            //    }
            //}

            //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            //var loginAddress = "www.mywebsite.com/login";
            //var loginData = new NameValueCollection
            //    {
            //      { "username", "shimmy" },
            //      { "password", "mypassword" }
            //    };

            //var client = new CookieAwareWebClient();
            //client.Login(loginAddress, loginData);

            var container = Login(login.username, login.password);
            if(container != null)
                Session["token"] = container;
            if (Session["token"] != null)
                return RedirectToAction("GetAllEmployees", "Employees");
            ViewBag.Message = "Username or password is incorrect";
            return View(login);
        }

        protected static CookieContainer Login(string userName, string password)
        {
            //string userName = "my.test.user.name";
            //string password = "my.test.password";
            CookieContainer cookieContainer = null;

            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = "Username=" + userName + "&Password=" + password + "&grant_type=" + "password";
                byte[] postDataBytes = encoding.GetBytes(postData);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:59094/token");

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = postDataBytes.Length;
                httpWebRequest.AllowAutoRedirect = false;

                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(postDataBytes, 0, postDataBytes.Length);
                    stream.Close();
                }

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        cookieContainer = new CookieContainer();
                        foreach (Cookie cookie in httpWebResponse.Cookies)
                        {
                            cookieContainer.Add(cookie);
                        }
                    }
                }
            }
            catch (System.Net.WebException e)
            {}

            return cookieContainer;
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session["token"] = null;
            return RedirectToAction("Signin", "ManageAccount");
        }
    }
}