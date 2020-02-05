using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Analytics.Controllers
{
    [Route("analytics")]
    public class AnalyticsController : Controller
    {
        public AnalyticsController(IAnalyticsData data)
        {
            AnalyticsData = data;
        }

        private IAnalyticsData AnalyticsData { get; }

        [HttpGet]
        public IActionResult Default()
        {
            return Ok(AnalyticsData);
        }
    }
}
