using System;
using System.Net;
using Nancy;
using ReallySimpleProxy.RequestProxying;

namespace ReallySimplePci.OutboundProxy.ProxyInterceptors
{
    public class LogIncomingRequests : IRequestModifier
    {
        public void Modify(string outgoingUri, Request incomingRequest, HttpWebRequest outgoingRequest)
        {
            Console.WriteLine("Proxying to " + outgoingUri);
        }
    }
}