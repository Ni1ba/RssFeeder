using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using AspTest.Models;


namespace AspTest.Services
{
    public class RssService
    {
        private readonly string _rssUrl;

        public RssService(string rssUrl)
        {
            _rssUrl = rssUrl;
        }

        public IEnumerable<RssItem> GetRssFeed()
        {
            var reader = XmlReader.Create(_rssUrl);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            return feed.Items.Select(item => new RssItem
            {
                Title = item.Title.Text,
                PubDate = item.PublishDate.DateTime,
                Description = StripHtml(HttpUtility.HtmlDecode(item.Summary?.Text ?? item.Content?.ToString() ?? string.Empty)),
                Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? string.Empty
            });
        }
        private string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
