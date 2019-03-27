using EmployeeWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeWebAPI.Controllers
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        EmployeesInfoEntities db = new EmployeesInfoEntities();

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            //int a = 5;
            //double b = a / 0;
            var employees = db.Employees.ToList();
            return Ok(employees);
        }


        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var employee = db.Employees.Find(id);
            return Ok(employee);
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add(Employee employee)
        {
            db.Employees.Add(employee);
            db.SaveChanges();
            return Ok(employee.Id);
        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update(Employee employee)
        {
            db.Entry<Employee>(employee).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok(employee);
        }

        [Route("remove")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            //db.Employees.Remove()
            var employee = db.Employees.Find(id);
            db.Entry<Employee>(employee).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Ok(true);
        }
    }
}
