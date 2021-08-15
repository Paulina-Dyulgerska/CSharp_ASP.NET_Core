namespace ConformityCheck.Data.Models
{
    using System;

    public class DistributedCache
    {
        public string Id { get; set; }

        public byte[] Value { get; set; }

        public DateTimeOffset ExpiresAtTime { get; set; }

        public long? SlidingExpirationInSeconds { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}
