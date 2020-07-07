using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Schedule resources
    /// </summary>
    public class SpendingStatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<SpendingStats> SpendingStats { get; set; }
    }
}