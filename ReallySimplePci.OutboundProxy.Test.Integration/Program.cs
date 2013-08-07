using System.IO;
using System.Net;
using NUnit.Framework;

namespace ReallySimplePci.OutboundProxy.Test.Integration
{
    [TestFixture]
    public class Program
    {
        [Test]
        public void Main()
        {
            var url = "http://localhost:53465";

            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Proxy = new WebProxy("localhost", 12345);
            request.Headers.Add("X-Forwarded-Proto", "http");
            request.Method = "POST";

            var requestStream = request.GetRequestStream();
            using (var writer = new StreamWriter(requestStream))
            {
                writer.Write("@CardNumber:1");
                writer.Flush();
            }

            var response = (HttpWebResponse) request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var body = sr.ReadToEnd();
            }

            var status = (int) response.StatusCode;
        }
    }
}
