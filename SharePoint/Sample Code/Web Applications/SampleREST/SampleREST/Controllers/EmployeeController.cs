using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeRESTServer.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET: api/Employee
        public IEnumerable<string> Get()
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
            // return new string[] { "SomeValue1", "SomeValue2", "etc" };
        }

        // GET: api/Employee/1234567
        public Dictionary<string, string> Get(int id)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("id", id.ToString());
            response.Add("firstName", "Jeremy");
            response.Add("lastName", "Morel");
            response.Add("email", "jeremy_morel@trg.com");
            return response;
        }

        // GET: api/Employee/1234567/firstName
        public Dictionary<string, string> Get(int id, string property)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("id", id.ToString());
            response.Add(property, getEmployeeProperty(id, property));
            return response;
        }

        // GET: api/Employee/1234567/phoneNumber/home
        public Dictionary<string, string> Get(int id, string property, string type)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("id", id.ToString());
            response.Add(property, getEmployeePropertyByType(id, property, type));
            return response;
        }

        private string getEmployeePropertyByType(int id, string property, string type)
        {
            if (property.ToLowerInvariant() == "phonenumber")
            {
                if (type.ToLowerInvariant() == "home") return "000-000-0000";
                return "123-456-7890";
            }
            return "Some " + property + " of type " + type;
        }

        private string getEmployeeProperty(int id, string property)
        {
            if (property.ToLowerInvariant() == "badproperty")
            {
                throw new HttpResponseException(Request.CreateErrorResponse
                    (HttpStatusCode.NotImplemented, "'" + property + "' is invalid, " +
                     "or unable to be returned by this service."));
            }

            return "Here's Some Property Value for employee " + id;
        }

        // POST: api/Employee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employee/1234567
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employee/1234567
        public void Delete(int id)
        {
        }
    }
}