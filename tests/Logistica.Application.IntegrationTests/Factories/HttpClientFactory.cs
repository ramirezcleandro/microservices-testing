using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.IntegrationTests.Factories
{
    public class HttpClientFactory
    {
        public static HttpClient createClient() {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost");
            return client;
        }
    }
}
