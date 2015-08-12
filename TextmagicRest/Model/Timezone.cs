using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Account timezone class
    /// </summary>
    public class Timezone
    {
        /// <summary>
        /// Timezone area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Is daylight saving time used (raw string data)
        /// </summary>
        public int Dst { get; set; }

        /// <summary>
        /// Timezone offset in ms
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Timezone name (probably what you need to display)
        /// </summary>
        [DeserializeAs(Name = "timezone")]
        public string Name { get; set; }
    }
}
