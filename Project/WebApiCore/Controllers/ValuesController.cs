﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Data;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/values
        [HttpGet("GetCities")]
        public JsonResult GetCities()
        {
            return new JsonResult( CitiesDataStore.Current.Cities);
        }

        [HttpGet("GetCity/{id}")]
        public IActionResult GetCity(int Id)
        {
           var city = CitiesDataStore.Current.Cities.Where(w => w.Id == Id).FirstOrDefault();

            if (city == null)
            {
                return NotFound();
            }

            return new JsonResult(city);
        }
    }
}