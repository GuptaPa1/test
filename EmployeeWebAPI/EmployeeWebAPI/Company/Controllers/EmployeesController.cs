using Company.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Company.Controllers
{
    public class EmployeesController : Controller
    {

        string apiUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();
        // GET: Employees
        public ActionResult GetAllEmployees()
        {
            if (Session["token"] == null)
                return RedirectToAction("Signin", "ManageAccount");
            //int a = 5;
            //double b = a / 0;
            IEnumerable<EmployeeDto> employees = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                var responseTask = client.GetAsync("api/employees");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EmployeeDto>>();
                    readTask.Wait();

                    employees = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    employees = Enumerable.Empty<EmployeeDto>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(employees);
        }

        public ActionResult AddEmployee()
        {
            if (Session["token"] == null)
                return RedirectToAction("Signin", "ManageAccount");
            ViewBag.Genders = new List<string> { "Male", "Female" };
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeDto employeeDto)
        {
            employeeDto.CreatedOn = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<EmployeeDto>("api/employees/add", employeeDto);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllEmployees");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            ViewBag.Genders = new List<string> { "Male", "Female" };
            return View(employeeDto);
        }

        public ActionResult UpdateEmployee(int id)
        {
            if (Session["token"] == null)
                return RedirectToAction("Signin", "ManageAccount");
            EmployeeDto employee = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                //HTTP GET
                var responseTask = client.GetAsync("api/employees/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EmployeeDto>();
                    readTask.Wait();

                    employee = readTask.Result;
                }
            }
            employee.Genders = new List<string> { "Male", "Female" };
            return View(employee);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeDto employeeDto)
        {
            employeeDto.UpdatedOn = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                //HTTP POST
                var putTask = client.PutAsJsonAsync<EmployeeDto>("api/employees/update", employeeDto);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("GetAllEmployees");
                }
            }
            employeeDto.Genders = new List<string> { "Male", "Female" };
            return View(employeeDto);
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            if (Session["token"] == null)
                return RedirectToAction("Signin", "ManageAccount");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/employees/remove?id=" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("GetAllEmployees");
                }
            }

            return RedirectToAction("GetAllEmployees");
        }
    }
}