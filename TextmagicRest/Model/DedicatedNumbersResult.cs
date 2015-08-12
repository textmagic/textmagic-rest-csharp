using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of DedicatedNumber objects
    /// </summary>
    public class DedicatedNumbersResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<DedicatedNumber> DedicatedNumbers { get; set; }
    }
}
