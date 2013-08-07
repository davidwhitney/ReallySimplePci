using System;
using System.Text.RegularExpressions;
using ReallySimplePci.Core;
using ReallySimplePci.Core.Encryption;
using ReallySimpleProxy.RequestProxying;

namespace ReallySimplePci.OutboundProxy.ProxyInterceptors
{
    public class InsertCreditCardNumbersIntoOutboundRequests : IRequestBodyProcessor
    {
        private readonly ICardDataStore _dataStore;
        private readonly ITwoWaySharedKeyEncryption _encryption;
        private readonly IEncryptionKeys _keys;

        public InsertCreditCardNumbersIntoOutboundRequests(ICardDataStore dataStore, ITwoWaySharedKeyEncryption encryption, IEncryptionKeys keys)
        {
            _dataStore = dataStore;
            _encryption = encryption;
            _keys = keys;
        }
         
        public string ProcessBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return body;
            }

            if (!body.Contains("@CardNumber:"))
            {
                return body;
            }

            var captures = Regex.Matches(body, "@CardNumber:(?<id>[0-9]+?)", RegexOptions.IgnoreCase);
            var token = captures[0].Groups[0].Value;
            var ids = captures[0].Groups[1].Value;
            var id = Int32.Parse(ids);

            var record = _dataStore.Get(id);

            var cardNumber = _encryption.Decrypt(_keys.PrivateCardNumberEncryptionKey, record.EncryptedPan);
            body = body.Replace(token, cardNumber);

            return body;
        }
    }
}