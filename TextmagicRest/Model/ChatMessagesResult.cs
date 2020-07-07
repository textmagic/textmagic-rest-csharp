using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of ChatMessage objects
    /// </summary>
    public class ChatMessagesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<ChatMessage> Messages { get; set; }
    }
}