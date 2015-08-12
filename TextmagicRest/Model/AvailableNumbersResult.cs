using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    public class AvailableNumbersResult: BaseModel
    {
        /// <summary>
        /// Available numbers list
        /// </summary>
        public List<string> Numbers { get; set; }

        /// <summary>
        /// Number monthly fee
        /// </summary>
        public double Price { get; set; }
    }
}
