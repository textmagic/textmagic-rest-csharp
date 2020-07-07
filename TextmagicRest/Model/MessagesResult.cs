using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Message objects
    /// </summary>
    public class MessagesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Message> Messages { get; set; }
    }
}