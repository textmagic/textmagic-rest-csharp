using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of MessagingStats object
    /// </summary>
    public class MessagingStatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<MessagingStats> MessagingStats { get; set; }
    }
}
