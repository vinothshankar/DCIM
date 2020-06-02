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
    public class PortReplaceController : Controller
    {
        private readonly ILogger<PortReplaceController> _logger;
        private DCIMServices _service;
        public PortReplaceController(ILogger<PortReplaceController> logger, DCIMServices services)
        {
            _logger = logger;
            _service = services;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPortMaster()
        {
            return Ok(_service.GetPortMasters());
        }

        [HttpPost]
        public String GetReplacePortImage(DCIMModel info)
        {
            return _service.GetReplacePortImage(info);
        }

        [HttpPost]
        public IActionResult MakePortReplace(DCIMModel info)
        {

            return Ok(_service.MakePortReplace(info));
        }

        [HttpPost]
        public IActionResult ServerPortView(DCIMModel dCIMModel)
        {
            var data = _service.ServerPortView(dCIMModel, false);
            return Ok(data);
        }
    }
}
