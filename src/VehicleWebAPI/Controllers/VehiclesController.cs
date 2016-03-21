using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VehicleWebAPI.Models;

// In the test client, PUT and DELETE return 500 instead of recommended values when requests are sent
// on now-missing records (i.e. I deleted the records, then sent PUT/DELETE requests to test idempotence).
// Should I note this when submitting?
namespace VehicleWebAPI.Controllers
{
    [Route("[controller]")]
    public class VehiclesController : Controller
    {
        [FromServices]
        public IVehicleRepository VehicleRepository { get; set; }

        public VehiclesController(IVehicleRepository repo)
        {
            VehicleRepository = repo;
        }

        //[HttpGet]
        //public IEnumerable<Vehicle> Get()
        //{
        //    return VehicleRepository.GetAllVehicles();
        //}

        [HttpGet("{id}", Name = "GetVehicle"), Route("Vehicles/{id}")]
        public IActionResult GetById(int id)
        {
            var vehicle = VehicleRepository.Read(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet, Route("Vehicles")]
        public IEnumerable<Vehicle> Get(int year = 0, string make = "", string model = "")
        {
            var vehicles = VehicleRepository.GetAllVehicles();
            
            if (year != 0)
            {
                vehicles = vehicles.Where(x => x.Year == year);
            }

            if (!string.IsNullOrEmpty(make))
            {
                vehicles = vehicles.Where(x => x.Make == make);
            }

            if (!string.IsNullOrEmpty(model))
            {
                vehicles = vehicles.Where(x => x.Model == model);
            }

            return vehicles;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Vehicle v)
        {
            if (v == null)
            {
                return HttpBadRequest();
            }

            if (!VehicleRepository.Create(v))
            {
                return new HttpStatusCodeResult(409);
            }

            return CreatedAtRoute("GetVehicle", new { controller = "vehicles", id = v.Id }, v);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Vehicle v)
        {
            if (v == null)
            {
                return HttpBadRequest();
            }

            if (!VehicleRepository.Update(v))
            {
                return HttpNotFound();
            }

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var v = VehicleRepository.Delete(id);
            if (v == null)
            {
                return HttpNotFound();
            }

            return new NoContentResult();
        }
    }
}
