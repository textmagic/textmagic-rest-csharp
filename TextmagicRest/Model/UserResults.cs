using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Subaccounts
    /// </summary>
    public class UserResults : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<User> Subaccounts { get; set; }
    }
}