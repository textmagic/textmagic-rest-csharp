using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of Schedule resources
    /// </summary>
    public class SpendingStatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<SpendingStats> SpendingStats { get; set; }
    }
}
