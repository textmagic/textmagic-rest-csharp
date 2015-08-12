using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Inbound message class
    /// </summary>
    public class Reply: BaseModel
    {
        /// <summary>
        /// Inbound message ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Message sender
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Message receiver
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Message receiving time
        /// </summary>
        public DateTime? MessageTime { get; set; }

        /// <summary>
        /// Inbound message text
        /// </summary>
        public string Text { get; set; }
    }
}
