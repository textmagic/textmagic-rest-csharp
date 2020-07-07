using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     Price by country
    /// </summary>
    public class CountryPricing
    {
        /// <summary>
        ///     2-letter ISO country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     Amount of messages to this country
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Single message price to this country
        /// </summary>
        [DeserializeAs(Name = "max")]
        public float Price { get; set; }
    }
}