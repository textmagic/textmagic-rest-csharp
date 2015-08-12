using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest
{
    /// <summary>
    /// TextMagic REST API exception class
    /// </summary>  
    public class ClientException: Exception
    {
        /// <summary>
        /// HTTP code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Brief error message
        /// </summary>
        public new string Message { get; set; }

        /// <summary>
        /// Array of errors, grouped by input field
        /// </summary>
        public ValidationErrors Errors { get; set; }

        public ClientException() : base() { }
        public ClientException(string message) : base(message) { }
        public ClientException(string message, Exception inner) : base(message, inner) { }
    }
}
