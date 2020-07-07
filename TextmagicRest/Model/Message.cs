using System;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    public enum DeliveryStatus
    {
        Delivered = 'd',
        Queued = 'q',
        Enrouted = 'r',
        Acked = 'a',
        Scheduled = 's',
        Failed = 'f',
        Rejected = 'j',
        Unknown = 'u'
    }

    /// <summary>
    ///     TextMagic outbound message class
    /// </summary>
    public class Message : BaseModel
    {
        /// <summary>
        ///     Message ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Message text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Message sender phone number or sender ID
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///     Message receiver phone number
        /// </summary>
        public string Receiver { get; set; }

        [DeserializeAs(Name = "status")] public char StatusChar { get; set; }

        /// <summary>
        ///     Message delivery status
        /// </summary>
        [DeserializeAs(Name = "fake-unused-name")]
        public DeliveryStatus Status
        {
            get => (DeliveryStatus) StatusChar;
            set => StatusChar = value.ToString()[0];
        }

        /// <summary>
        ///     Message sending time
        /// </summary>
        public DateTime MessageTime { get; set; }

        /// <summary>
        ///     Message text charset
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        ///     SMS parts count
        /// </summary>
        public int PartsCount { get; set; }

        /// <summary>
        ///     Message sending price (in account currency)
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        ///     Destination country 2-letter ISO code
        /// </summary>
        [DeserializeAs(Name = "country")]
        public string CountryId { get; set; }

        /// <summary>
        ///     Recipient first name (if given by contact or Email2SMS)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Recipient last name (if given by contact or Email2SMS)
        /// </summary>
        public string LastName { get; set; }
    }
}