using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Chats list
    /// </summary>
    public class ChatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<Chat> Chats { get; set; }
    }
}
