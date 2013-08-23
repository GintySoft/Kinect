using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using NetduinoAPI.Models;

namespace NetduinoAPI.Controllers
{
    public class TemperatureController : ApiController
    {
        // GET /api/values
        public IEnumerable<Temperature> GetAllTemperature()
        {
            List<Temperature> temps = new List<Temperature>(Temperature.temps.Count);
            foreach (Temperature t in Temperature.temps)
            {
                temps.Add(t);
            }
            return temps;
        }

        // GET /api/values/5
        public Temperature GetTemperature(Temperature t)
        {
            Temperature t3 = Temperature.temps.Single(t2 => t.ID == t2.ID);
            if (t3 == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return t3;
        }

        // POST /api/values
        public HttpResponseMessage<Temperature> PostTemperature(Temperature t)
        {
            Temperature.temps.Add(t);

            var response = new HttpResponseMessage<Temperature>(t, HttpStatusCode.Created);
            var relativePath = "/api/Temerature/" + t.ID;
            response.Headers.Location = new Uri(Request.RequestUri, relativePath); 
            return response;
        }

        // PUT /api/values/5
        public void PutTemperature(Temperature t)
        {
            bool found = false;
            int index = -1;
            foreach (Temperature t1 in Temperature.temps)
            {
                if (t1.ID == t.ID)
                {
                    found = true;
                    break;
                }
                index++;
            }
            if (found)
            {
                Temperature.temps[index] = t;
            }
            else
            {
                Temperature.temps.Add(t);
            }
        }

        // DELETE /api/values/5
        public HttpResponseMessage DeleteTemperature(Temperature t)
        {
            int index = -1;
            for (int i = 0; i < Temperature.temps.Count; i++)
            {
                if (Temperature.temps[i].DeviceID == t.ID)
                {
                    index = i;
                    break;
                }
            }
            if (index > 0)
            {
                Temperature.temps.RemoveAt(index);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}