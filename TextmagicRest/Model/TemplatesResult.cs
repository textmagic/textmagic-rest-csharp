using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Template objects
    /// </summary>
    public class TemplatesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Template> Templates { get; set; }
    }
}