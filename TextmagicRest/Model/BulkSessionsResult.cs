using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of BulkSession objects
    /// </summary>
    public class BulkSessionsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<BulkSession> BulkSessions { get; set; }
    }
}
