using System;
using System.IO;
using System.Net;
using System.Text;

namespace ReallySimplePci.OutboundProxy.Test.Integration
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    var url = "http://www.google.co.uk";
                    Console.WriteLine("Requesting " + url);

                    var request = (HttpWebRequest) WebRequest.Create(url);
                    request.Proxy = new WebProxy("localhost", 44466);
                    request.Method = "GET";
                    var outbody = string.Empty;// "@CardNumber:1";
                    
                    if (!string.IsNullOrWhiteSpace( outbody ))
                    {
                        request.Method = "POST"; 
                        var requestStream = request.GetRequestStream();
                        using (var writer = new StreamWriter(requestStream))
                        {
                            writer.Write(outbody);
                            writer.Flush();
                        }
                    };

                    //request.Headers.Add("X-Forwarded-Proto", "https");

                    var response = (HttpWebResponse) request.GetResponse();
                    var status = (int)response.StatusCode;
                    Console.WriteLine("Status code: " + status);
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var body = sr.ReadToEnd();
                        Console.WriteLine(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("Try again, hit return...");
                Console.ReadLine();

            }


        }
    }
}

