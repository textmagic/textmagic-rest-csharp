namespace TextmagicRest.Model
{
    /// <summary>
    ///     Message sending result
    /// </summary>
    public class SendingResult : LinkResult
    {
        /// <summary>
        ///     Resulting entity type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Session ID (if any)
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        ///     Bulk Session ID (if any)
        /// </summary>
        public int BulkId { get; set; }

        /// <summary>
        ///     Message ID (if any)
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        ///     Schedule ID (if any)
        /// </summary>
        public int ScheduleId { get; set; }
    }
}