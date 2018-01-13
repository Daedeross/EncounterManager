namespace Foundation
{
    public static class IdentityExtensions
    {
        public static TRecord AsRecord<TRecord>(this Identity identity)
            where TRecord : IRecord, new()
        {
            if (Equals(identity, Identity.None)) return default(TRecord);
            return new TRecord
            {
                Id = identity
            };
        }

        public static bool Exists(this Identity identity)
        {
            return !Equals(identity, Identity.None);
        }
    }
}