using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of contact List object
    /// </summary>
    public class ContactListsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<ContactList> ContactLists { get; set; }
    }
}