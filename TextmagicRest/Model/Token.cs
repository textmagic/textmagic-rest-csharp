using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Token class
    /// </summary>
    public class TokenResult : BaseModel
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration Date
        /// </summary>
        public string Expires { get; set; }
    }
}
