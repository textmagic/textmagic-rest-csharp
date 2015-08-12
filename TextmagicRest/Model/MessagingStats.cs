using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    public enum MessagingStatsGroupBy
    {
        None,
        Day,
        Month,
        Year
    }

    /// <summary>
    /// Messaging Statistics entry
    /// </summary>
    public class MessagingStats : BaseModel
    {
        /// <summary>
        /// Reply rate
        /// </summary>
        public double ReplyRate { get; set; }

        /// <summary>
        /// Delivery rate
        /// </summary>
        public double DeliveryRate { get; set; }

        /// <summary>
        /// Date (when grouped)
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Total costs
        /// </summary>
        public double Costs { get; set; }

        /// <summary>
        /// Received messages count
        /// </summary>
        public int MessagesReceived { get; set; }

        /// <summary>
        /// Delivered messages count
        /// </summary>
        public int MessagesSentDelivered { get; set; }

        /// <summary>
        /// Amount of messages in Accepted status
        /// </summary>
        public int MessagesSentAccepted { get; set; }

        /// <summary>
        /// Amount of messages in Buffered status
        /// </summary>
        public int MessagesSentBuffered { get; set; }

        /// <summary>
        /// Amount of messages in Failed status
        /// </summary>
        public int MessagesSentFailed { get; set; }

        /// <summary>
        /// Amount of messages in Rejected status
        /// </summary>
        public int MessagesSentRejected { get; set; }

        /// <summary>
        /// Amount of sent messages parts
        /// </summary>
        public int MessagesSentParts { get; set; }
    }
}
