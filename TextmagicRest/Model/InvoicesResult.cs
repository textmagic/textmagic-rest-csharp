using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Invoice resources
    /// </summary>
    public class InvoicesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Invoice> Invoices { get; set; }
    }
}