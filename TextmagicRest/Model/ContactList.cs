namespace TextmagicRest.Model
{
    /// <summary>
    ///     TextMagic contact list class
    /// </summary>
    public class ContactList : BaseModel
    {
        /// <summary>
        ///     List ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     List name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     List description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     List members count
        /// </summary>
        public int MembersCount { get; set; }

        /// <summary>
        ///     Is contact list shared
        /// </summary>
        public bool Shared { get; set; }
    }
}