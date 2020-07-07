using System.Collections.Generic;

namespace TextmagicRest.Model
{
    public class AvailableNumbersResult : BaseModel
    {
        /// <summary>
        ///     Available numbers list
        /// </summary>
        public List<string> Numbers { get; set; }

        /// <summary>
        ///     Number monthly fee
        /// </summary>
        public double Price { get; set; }
    }
}