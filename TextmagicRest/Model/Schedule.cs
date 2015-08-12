using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Message sending schedule
    /// </summary>
    public class Schedule: BaseModel
    {
        /// <summary>
        /// Schedule ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Next scheduled sending time (if any)
        /// </summary>
        public DateTime? NextSend { get; set; }

        /// <summary>
        /// Message session
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// iCal RRULE string for this session
        /// </summary>
        public string Rrule { get; set; }
    }
}
