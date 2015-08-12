using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Resource class
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
    };      

    public class LinkResult: BaseModel
    {
        private string href;
        /// <summary>
        /// Link to a newly created/updated resource
        /// </summary>
        public string Href 
        {
            get { return href; }
            set 
            {
                href = value;

                // Get linked resource class by returned href
                Dictionary<string, ResourceClass> relations = new Dictionary<string, ResourceClass>() 
                {
                    { "messages", ResourceClass.Message },
                    { "bulks", ResourceClass.BulkSession },
                    { "sessions", ResourceClass.Session },
                    { "schedules", ResourceClass.Schedule },
                    { "templates", ResourceClass.Template },
                    { "contacts", ResourceClass.Contact },
                    { "lists", ResourceClass.ContactList },
                    { "customfields", ResourceClass.CustomField }
                };

                Regex r = new Regex(@"(\w+)/(\d+)", RegexOptions.IgnoreCase);
                Match m = r.Match(value);
                if (!m.Success)
                {
                    ResourceClass = ResourceClass.Unknown;
                }
                else
                {
                    string key = m.Groups[1].Value;
                    ResourceClass resourceClass = ResourceClass.Unknown;

                    if (relations.ContainsKey(key))
                    {
                        relations.TryGetValue(key, out resourceClass);
                    }

                    ResourceClass = resourceClass;
                }
            } 
        }

        /// <summary>
        /// Created or updated resource ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Resource class
        /// </summary>
        public ResourceClass ResourceClass { get; set; }
    }
}
