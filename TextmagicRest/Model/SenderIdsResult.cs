using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of SenderId objects
    /// </summary>
    public class SenderIdsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<SenderId> SenderIds { get; set; }
    }
}
