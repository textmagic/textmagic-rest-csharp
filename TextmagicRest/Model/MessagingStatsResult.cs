using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of MessagingStats object
    /// </summary>
    public class MessagingStatsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<MessagingStats> MessagingStats { get; set; }
    }
}