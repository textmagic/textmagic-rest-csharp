using System;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Unsubscribed contact class. Note that it isn't ordinary Contact; i.e. ID of Contact and UsubscribedContact
    ///     (if you unsubscribe it) WOULD NOT equal
    /// </summary>
    public class UnsubscribedContact : BaseModel
    {
        /// <summary>
        ///     Unsubscribed contact ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Unsubscribed contact phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Unsubscribed contact first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Unsubscribed contact last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Time when contact unsubscribed from your lists
        /// </summary>
        [DeserializeAs(Name = "unsubscribe_time")]
        public DateTime? UnsubscribedAt { get; set; }
    }
}