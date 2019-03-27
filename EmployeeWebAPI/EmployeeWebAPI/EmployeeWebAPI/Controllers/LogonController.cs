using EmployeeWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeWebAPI.Controllers
{
    public class LogonController : ApiController
    {
        EmployeesInfoEntities db = new EmployeesInfoEntities();
        // GET api/<controller>
        public bool Register(Login login)
        {
            db.Logins.Add(login);
            if (login.Id > 0)
                return true;
            return false;
        }

        // GET api/<controller>/5
        public string Login(Login login)
        {
            var user = db.Logins.FirstOrDefault(m => m.UserName == login.UserName && m.PasswordHash == login.PasswordHash);
            if(user != null)
            {
                //Session[]
            }
            return "";
        }
    }
}