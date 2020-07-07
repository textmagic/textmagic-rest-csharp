using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of SenderId objects
    /// </summary>
    public class SenderIdsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<SenderId> SenderIds { get; set; }
    }
}