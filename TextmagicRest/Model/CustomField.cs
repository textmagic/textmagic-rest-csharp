using System;

namespace TextmagicRest.Model
{
    public class CustomField : BaseModel
    {
        /// <summary>
        ///     Custom field ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Custom field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Custom field creation time
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///     Custom field value (when assigned)
        /// </summary>
        public string Value { get; set; }
    }
}