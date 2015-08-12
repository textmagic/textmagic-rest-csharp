using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Contact custom fields list
    /// </summary>
    public class CustomFieldsResult
    {
        [DeserializeAs(Name = "resources")]
        public List<CustomField> CustomFields { get; set; }
    }
}
