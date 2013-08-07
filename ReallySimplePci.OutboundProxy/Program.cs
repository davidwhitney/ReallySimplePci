using System;
using System.Collections.Generic;
using ReallySimplePci.OutboundProxy.ProxyInterceptors;
using ReallySimpleProxy;

namespace ReallySimplePci.OutboundProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var bodyProcessors = new List<Type> {typeof (InsertCreditCardNumbersIntoOutboundRequests)};

            var proxy = new ReallySimpleProxyHost(args, "ReallySimplePci.OutboundProxy");
            proxy.Host("locahost", 12345, bodyProcessors);
        }
    }
}
