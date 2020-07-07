namespace TextmagicRest.Model
{
    /// <summary>
    ///     Base class for TextMagic REST API resources
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        ///     API exception thrown (if any)
        /// </summary>
        public ClientException ClientException { get; set; }

        /// <summary>
        ///     Is request successful
        /// </summary>
        public bool Success => ClientException == null;
    }
}