using DCIM.Models;
using DCIM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCIM.Controllers
{
    public class RackServerMapController : Controller
    {
        private readonly ILogger<RackServerMapController> _logger;
        private DCIMServices _service;
        public RackServerMapController(ILogger<RackServerMapController> logger, DCIMServices services)
        {
            _logger = logger;
            _service = services;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetRackImage(int rackId)
        {
            return Ok(_service.GetRackImage(rackId));
        }
        [HttpGet]
        public IActionResult GetRackUnit(int rackId)
        {
            return Ok(_service.GetRackUnit(rackId));
        }
        [HttpPost]
        public IActionResult MapServerToRack(RackServerMap rackServerMap)
        {
            return Ok(_service.MapServerToRack(rackServerMap));
        } 

        [HttpGet]
        public IActionResult GetNotMappedServer()
        {
            return Ok(_service.GetNotMappedServers());
        }

        [HttpPost]
        public IActionResult GetNotMappedServerImage(RackServerMap rackServerMap)
        {
            return Ok(_service.GetNotMappedServerImage(rackServerMap));
        }

        [HttpPost]
        public IActionResult GetRackUnitImage(RackServerMap rackServerMap)
        {
            return Ok(_service.GetRackUnitImage(rackServerMap));
        }
    }
}
