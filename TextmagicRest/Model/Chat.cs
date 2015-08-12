using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Chat class
    /// </summary>
    public class Chat : BaseModel
    {
        /// <summary>
        /// Chat ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Recipient phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Recipient Contact object
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Unread messages count
        /// </summary>
        public int Unread { get; set; }

        /// <summary>
        /// Time when last incoming message has been arrived
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
