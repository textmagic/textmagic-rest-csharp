using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Chats list
    /// </summary>
    public class ChatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Chat> Chats { get; set; }
    }
}