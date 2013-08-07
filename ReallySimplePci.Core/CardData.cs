using System;

namespace ReallySimplePci.Core
{
    public class CardData
    {
        public virtual int Id { get; set; }
        public virtual string EncryptedPan { get; set; }
        public virtual DateTime PurgeAt { get; set; }
    }
}