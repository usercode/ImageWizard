using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.Analytics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageWizard.Analytics.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IAnalyticsData data)
        {
            AnalyticsData = data;
        }

        public IAnalyticsData AnalyticsData { get; }

        public void OnGet()
        {

        }
    }
}
