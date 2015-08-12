using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// User statements
    /// </summary>
    public class SpendingStats : BaseModel
    {
        /// <summary>
        /// Statement date
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Balance change value
        /// </summary>
        public double Delta { get; set; }

        /// <summary>
        /// Statement type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Statement value (i.e. dedicated phone number, sent SMS count etc)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Statement comment
        /// </summary>
        public string Comment { get; set; }
    }
}
