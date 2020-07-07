using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Contact custom fields list
    /// </summary>
    public class CustomFieldsResult
    {
        [DeserializeAs(Name = "resources")] public List<CustomField> CustomFields { get; set; }
    }
}