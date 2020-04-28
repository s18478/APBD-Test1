using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Services;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionsController : ControllerBase
    {
        private IPrescriptionDbService _service;

        public PrescriptionsController(IPrescriptionDbService service)
        {
            _service = service;
        }
        
        public IActionResult GetPrescriptions(string lastName)
        {
            List<Prescription> prescriptions = _service.GetPrescriptions(lastName);
         
            if (prescriptions.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(prescriptions);
        }
    }
}