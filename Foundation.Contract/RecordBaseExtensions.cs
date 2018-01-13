namespace Foundation
{
    public static class RecordBaseExtensions
    {
        public static bool Exists(this IRecord record)
        {
            return record != null && !Equals(record.Id, Identity.None);
        }
    }
}