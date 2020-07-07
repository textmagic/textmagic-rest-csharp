namespace TextmagicRest.Model
{
    /// <summary>
    ///     TextMagic Invoice class
    /// </summary>
    public class Invoice : BaseModel
    {
        /// <summary>
        ///     Invoice ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Invoice bundle
        /// </summary>
        public int Bundle { get; set; }

        /// <summary>
        ///     Invoice currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        ///     Invoice VAT amount
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        ///     Payment method description
        /// </summary>
        public string PaymentMethod { get; set; }
    }
}