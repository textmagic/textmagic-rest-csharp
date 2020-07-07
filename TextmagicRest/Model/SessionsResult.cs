using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Session resources
    /// </summary>
    public class SessionsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Session> Sessions { get; set; }
    }
}