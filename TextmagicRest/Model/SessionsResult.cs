using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of Session resources
    /// </summary>
    public class SessionsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<Session> Sessions { get; set; }
    }
}
