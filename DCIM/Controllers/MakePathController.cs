using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DCIM.Models;
using System.IO;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Drawing;
using System.Buffers.Text;
using DCIM.Extension;
using DCIM.Services;

namespace DCIM.Controllers
{
    public class MakePathController : Controller
    {
        private readonly ILogger<MakePathController> _logger;
        private DCIMServices _service;
        public MakePathController(ILogger<MakePathController> logger, DCIMServices services)
        {
            _logger = logger;
            _service = services;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetRacks(string type)
        {
            return Ok(_service.GetRacks(type));
        }
        public IActionResult GetServers(int rackId, string type)
        {
            return Ok(_service.GetServers(rackId, type));
        }
        public IActionResult GetServerSlot(int serverId)
        {
            return Ok(_service.GetServerSlots(serverId));
        }
        public IActionResult GetServerPorts(int serverId, int slotId)
        {
            return Ok(_service.GetServerPorts(serverId, slotId));
        }
        public IActionResult GetDestinationIds(int portId)
        {
            return Ok(_service.GetDestinationIds(portId));
        }


        [HttpPost]
        public String GetSServerImage(DCIMModel info)
        {
            return _service.GetServerImage(info, true);
        }
        [HttpPost]
        public String GetDServerImage(DCIMModel info)
        {
            return _service.GetServerImage(info, false);
        }

        [HttpPost]
        public String GetServerDPortImage(DCIMModel info)
        {
            return _service.GetServerPortImage(info, true);
        }
        [HttpPost]
        public String GetServerSPortImage(DCIMModel info)
        {
            return _service.GetServerPortImage(info, false);
        }


        [HttpPost]
        public IActionResult MakePath(DCIMModel info)
        {
            return Ok(_service.MakePath(info));
        }
    }
}
