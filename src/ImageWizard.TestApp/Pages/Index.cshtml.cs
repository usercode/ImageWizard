// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace ImageWizard.TestApp.Pages;

public class IndexModel : PageModel
{
    public double? DPR { get; set; }

    public int? Width { get; set; }

    public int? ViewportWidth { get; set; }

    public void OnGet()
    {
        if (Request.Headers.TryGetValue("DPR", out StringValues dpr) && dpr.Count != 0)
        {
            DPR = double.Parse($"{dpr[0]}", CultureInfo.InvariantCulture);
        }

        if (Request.Headers.TryGetValue("Width", out StringValues width) && width.Count != 0)
        {
            Width = int.Parse($"{width[0]}", CultureInfo.InvariantCulture);
        }

        if (Request.Headers.TryGetValue("Viewport-Width", out StringValues viewportWidth) && viewportWidth.Count != 0)
        {
            ViewportWidth = int.Parse($"{viewportWidth[0]}", CultureInfo.InvariantCulture);
        }
    }
}
