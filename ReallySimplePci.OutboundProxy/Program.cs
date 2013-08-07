using ReallySimpleProxy;

namespace ReallySimplePci.OutboundProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new ReallySimpleProxyHost(args, "ReallySimplePci.OutboundProxy");
            proxy.Host("locahost", 12345);
        }
    }
}
