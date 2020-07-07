using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of BulkSession objects
    /// </summary>
    public class BulkSessionsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<BulkSession> BulkSessions { get; set; }
    }
}