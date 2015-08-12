using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Available source sender IDs list
    /// </summary>
    public class SourcesResult
    {
        /// <summary>
        /// Dedicated numbers list
        /// </summary>
        public List<string> Dedicated { get; set; }

        /// <summary>
        /// Shared numbers list
        /// </summary>
        public List<string> Shared { get; set; }

        /// <summary>
        /// Alphanumeric Sender IDs
        /// </summary>
        public List<string> SenderId { get; set; }

        /// <summary>
        /// Approved user phone numbers
        /// </summary>
        public List<string> User { get; set; }
    }
}
