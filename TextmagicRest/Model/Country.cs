using System;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Country representation class
    /// </summary>
    public class Country
    {
        /// <summary>
        /// 2-letter ISO country ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        public string Name { get; set; }
    }
}
