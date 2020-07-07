using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Resource class
    /// </summary>
    public enum ResourceClass
    {
        Unknown,
        Message,
        BulkSession,
        Session,
        Schedule,
        Template,
        Contact,
        ContactList,
        CustomField
    }

    public class LinkResult : BaseModel
    {
        private string href;

        /// <summary>
        ///     Link to a newly created/updated resource
        /// </summary>
        public string Href
        {
            get => href;
            set
            {
                href = value;

                // Get linked resource class by returned href
                var relations = new Dictionary<string, ResourceClass>
                {
                    {"messages", ResourceClass.Message},
                    {"bulks", ResourceClass.BulkSession},
                    {"sessions", ResourceClass.Session},
                    {"schedules", ResourceClass.Schedule},
                    {"templates", ResourceClass.Template},
                    {"contacts", ResourceClass.Contact},
                    {"lists", ResourceClass.ContactList},
                    {"customfields", ResourceClass.CustomField}
                };

                var r = new Regex(@"(\w+)/(\d+)", RegexOptions.IgnoreCase);
                var m = r.Match(value);
                if (!m.Success)
                {
                    ResourceClass = ResourceClass.Unknown;
                }
                else
                {
                    var key = m.Groups[1].Value;
                    var resourceClass = ResourceClass.Unknown;

                    if (relations.ContainsKey(key)) relations.TryGetValue(key, out resourceClass);

                    ResourceClass = resourceClass;
                }
            }
        }

        /// <summary>
        ///     Created or updated resource ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Resource class
        /// </summary>
        public ResourceClass ResourceClass { get; set; }
    }
}