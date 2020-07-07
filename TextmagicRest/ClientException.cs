using System;

namespace TextmagicRest
{
    /// <summary>
    ///     TextMagic REST API exception class
    /// </summary>
    public class ClientException : Exception
    {
        public ClientException()
        {
        }

        public ClientException(string message) : base(message)
        {
        }

        public ClientException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        ///     HTTP code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     Brief error message
        /// </summary>
        public new string Message { get; set; }

        /// <summary>
        ///     Array of errors, grouped by input field
        /// </summary>
        public ValidationErrors Errors { get; set; }
    }
}