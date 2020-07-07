using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Contact objects
    /// </summary>
    public class ContactsResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Contact> Contacts { get; set; }
    }
}