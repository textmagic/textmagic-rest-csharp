using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    public enum SenderIdStatus
    {
        Active = 'A',
        Pending = 'P',
        Rejected = 'R'
    }

    public class SenderId : BaseModel
    {
        /// <summary>
        ///     Sender ID numeric ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Alphanumeric Sender ID itself
        /// </summary>
        [DeserializeAs(Name = "senderId")]
        public string Name { get; set; }

        /// <summary>
        ///     User who applied for this Sender ID
        /// </summary>
        public User User { get; set; }

        [DeserializeAs(Name = "status")] public char StatusChar { get; set; }

        /// <summary>
        ///     Dedicated number status
        /// </summary>
        [DeserializeAs(Name = "fake-unused-name")]
        public SenderIdStatus Status
        {
            get => (SenderIdStatus) StatusChar;
            set => StatusChar = value.ToString()[0];
        }
    }
}