using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of UnsubscribedContact objects
    /// </summary>
    public class UnsubscribedContactsResult
    {
        [DeserializeAs(Name = "resources")] public List<UnsubscribedContact> UnsubscribedContacts { get; set; }
    }
}