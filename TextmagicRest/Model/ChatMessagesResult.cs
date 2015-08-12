using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of ChatMessage objects
    /// </summary>
    public class ChatMessagesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<ChatMessage> Messages { get; set; }
    }
}
