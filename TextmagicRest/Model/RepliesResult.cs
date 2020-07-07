using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Reply objects
    /// </summary>
    public class RepliesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Reply> Replies { get; set; }
    }
}