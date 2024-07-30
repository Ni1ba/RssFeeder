using AspTest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspTest.Services
{
    public class RssRefreshService : IHostedService, IDisposable
    {
        private readonly RssService _rssService;
        private readonly IConfiguration _configuration;
        private Timer _timer;

        public RssRefreshService(RssService rssService, IConfiguration configuration)
        {
            _rssService = rssService;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var refreshInterval = _configuration.GetValue<int>("RssFeed:RefreshIntervalMinutes");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(refreshInterval));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _rssService.GetRssFeed();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}