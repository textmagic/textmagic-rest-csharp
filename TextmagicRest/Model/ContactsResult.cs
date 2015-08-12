using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of Contact objects
    /// </summary>
    public class ContactsResult: BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<Contact> Contacts { get; set; }
    }
}
