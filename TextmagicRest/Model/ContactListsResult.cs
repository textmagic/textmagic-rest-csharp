using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// List of contact List object
    /// </summary>
    public class ContactListsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")]
        public List<ContactList> ContactLists { get; set; }
    }
}
