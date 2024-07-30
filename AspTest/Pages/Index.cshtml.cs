using AspTest.Models;
using AspTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly RssService _rssService;

    [BindProperty]
    public string RssUrl { get; set; }
    public IEnumerable<RssItem> RssItems { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
        var rssUrl = _configuration["RssFeed:Url"];
        _rssService = new RssService(rssUrl);
    }

    public void OnGet()
    {
        RssItems = _rssService.GetRssFeed();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(RssUrl))
        {
            _rssService.SetRssUrl(RssUrl);
            RssItems = _rssService.GetRssFeed();
        }
        return Page();
    }
}