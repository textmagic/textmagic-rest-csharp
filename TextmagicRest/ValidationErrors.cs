using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest
{
    /// <summary>
    /// Validation errors
    /// </summary>
    public class ValidationErrors
    {
        /// <summary>
        /// Validation global form errors
        /// </summary>
        public List<string> Common { get; set; }

        /// <summary>
        /// Validation form field errors
        /// </summary>
        public Dictionary<string, List<string>> Fields { get; set; }
    }
}
