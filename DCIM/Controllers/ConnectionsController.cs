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
    public class ConnectionsController : Controller
    {
        private readonly ILogger<ConnectionsController> _logger;
        private DCIMServices _service;
        public ConnectionsController(ILogger<ConnectionsController> logger, DCIMServices services)
        {
            _logger = logger;
            _service = services;
        }
        public IActionResult Index()
        {
            return View(_service.GetConnectionLists());
        }

        [HttpPost]
        public IActionResult DisconnectPath(ConnectionList connectionList)
        {
            return Ok(_service.DisconnectPath(connectionList));
        }
    }
}
