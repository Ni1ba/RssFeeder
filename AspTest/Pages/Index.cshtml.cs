using AspTest.Models;
using AspTest.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public IEnumerable<RssItem> RssItems { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        var rssUrl = _configuration["RssFeed:Url"];
        var rssService = new RssService(rssUrl);
        RssItems = rssService.GetRssFeed();
    }
}