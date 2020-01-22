using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics.Controllers
{
    [Route("analytics")]
    public class AnalyticsController : Controller
    {
        [HttpGet]
        public IActionResult Default()
        {
            return Ok("123");
        }
    }
}
