using System;

namespace TextmagicRest.Model
{
    /// <summary>
    /// Account currency object
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// 3-letter ISO currency code
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// HTML-ready currency symbol (i.e. &pound; for GBP)
        /// </summary>
        public string HtmlSymbol { get; set; }
    }
}
