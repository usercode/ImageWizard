// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageWizard.TestApp.Pages;

public class IndexModel : PageModel
{
    public double? DPR { get; set; }

    public int? Width { get; set; }

    public int? ViewportWidth { get; set; }

    public void OnGet()
    {
        if (Request.Headers.ContainsKey("DPR"))
        {
            DPR = double.Parse(Request.Headers["DPR"], CultureInfo.InvariantCulture);
        }

        if (Request.Headers.ContainsKey("Width"))
        {
            Width = int.Parse(Request.Headers["Width"], CultureInfo.InvariantCulture);
        }

        if (Request.Headers.ContainsKey("Viewport-Width"))
        {
            ViewportWidth = int.Parse(Request.Headers["Viewport-Width"], CultureInfo.InvariantCulture);
        }
    }
}
