using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    public enum DedicatedNumberStatus
    {
        Active = 'A',
        Unused = 'U'
    }

    /// <summary>
    /// Dedicated number class
    /// </summary>
    public class DedicatedNumber: BaseModel
    {
        /// <summary>
        /// Dedicated number ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Dedicated number assignee
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Dedicated number purchase date
        /// </summary>
        public DateTime? PurchasedAt { get; set; }

        /// <summary>
        /// Dedicated number expiration date
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        /// Dedicated phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Dedicated number country
        /// </summary>
        public Country Country { get; set; }

        [DeserializeAs(Name = "status")]
        public char StatusChar { get; set; }
        /// <summary>
        /// Dedicated number status
        /// </summary>
        [DeserializeAs(Name = "fake-unused-name")]
        public DedicatedNumberStatus Status
        {
            get { return (DedicatedNumberStatus)StatusChar; }
            set { StatusChar = value.ToString()[0]; }
        }
    }
}
