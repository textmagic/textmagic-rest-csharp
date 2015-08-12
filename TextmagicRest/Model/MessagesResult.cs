using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of Message objects
    /// </summary>
    public class MessagesResult: BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<Message> Messages { get; set; }
    }
}
