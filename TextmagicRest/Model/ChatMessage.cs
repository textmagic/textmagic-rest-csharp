using System;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    public enum ChatMessageDirection
    {
        Outgoing = 'o',
        Incoming = 'i'
    }

    /// <summary>
    ///     Chat message class
    /// </summary>
    public class ChatMessage : BaseModel
    {
        /// <summary>
        ///     Message ID
        /// </summary>
        public int Id { get; set; }

        [DeserializeAs(Name = "direction")] public char DirectionChar { get; set; }

        /// <summary>
        ///     Message direction
        /// </summary>
        [DeserializeAs(Name = "fake-unused-name")]
        public ChatMessageDirection Direction
        {
            get => (ChatMessageDirection) DirectionChar;
            set => DirectionChar = value.ToString()[0];
        }

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
        ///     Recipient first name (if given by contact or Email2SMS)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Recipient last name (if given by contact or Email2SMS)
        /// </summary>
        public string LastName { get; set; }
    }
}