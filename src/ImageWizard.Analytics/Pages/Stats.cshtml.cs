using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageWizard.Analytics.Pages
{
    public class StatsModel : PageModel
    {
        public StatsModel(IAnalyticsData analyticsData)
        {
            AnalyticsData = analyticsData;
        }

        public IAnalyticsData AnalyticsData { get; }

        public void OnGet()
        {
        }
    }
}
