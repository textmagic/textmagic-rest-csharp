using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of DedicatedNumber objects
    /// </summary>
    public class DedicatedNumbersResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<DedicatedNumber> DedicatedNumbers { get; set; }
    }
}