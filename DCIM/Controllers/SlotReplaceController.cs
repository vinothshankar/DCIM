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
    public class SlotReplaceController : Controller
    {
        private readonly ILogger<SlotReplaceController> _logger;
        private DCIMServices _service;
        public SlotReplaceController(ILogger<SlotReplaceController> logger, DCIMServices services)
        {
            _logger = logger;
            _service = services;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSlotMasters()
        {
            return Ok(_service.GetSlotMasters());
        }

        [HttpPost]
        public IActionResult GetSlotMasterImages(int DSlot)
        {
            return Ok(_service.GetSlotImages(DSlot));
        }

        [HttpPost]
        public IActionResult InsertSlotDetails(DCIMModel dCIMModel)
        {
            var data = _service.InsertSlotDetails(dCIMModel);
            return Ok(data.Item2);
        }

        public IActionResult GetSlotImage(DCIMModel dCIMModel)
        {
            return Ok(_service.GetServerSlotImage(dCIMModel, false));
        }

        



    }
}
