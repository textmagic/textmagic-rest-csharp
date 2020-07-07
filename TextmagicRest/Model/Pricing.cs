using System.Collections.Generic;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Pricing information
    /// </summary>
    public class Pricing : BaseModel
    {
        /// <summary>
        ///     Total session cost
        /// </summary>
        public float Total { get; set; }

        /// <summary>
        ///     SMS parts count
        /// </summary>
        public int Parts { get; set; }

        /// <summary>
        ///     Price by country
        /// </summary>
        public Dictionary<string, CountryPricing> Countries { get; set; }
    }
}