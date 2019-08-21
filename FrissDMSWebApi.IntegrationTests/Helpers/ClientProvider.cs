using FrissDMS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace FrissDMSWebApi.IntegrationTests.Helpers
{
    public class ClientProvider : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public ClientProvider()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            _testServer?.Dispose();
            Client?.Dispose();
        }
    }
}
