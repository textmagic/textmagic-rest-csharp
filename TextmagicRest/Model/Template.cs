using System;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Message template class
    /// </summary>
    public class Template: BaseModel
    {
        /// <summary>
        /// Template ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Template name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Template content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Last modification date
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
