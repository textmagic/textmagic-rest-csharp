using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of Reply objects
    /// </summary>
    public class RepliesResult: BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<Reply> Replies { get; set; }
    }
}
