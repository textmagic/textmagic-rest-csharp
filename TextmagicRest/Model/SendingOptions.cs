using System;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     SendMessage() options
    /// </summary>
    public class SendingOptions
    {
        private int? templateId;
        private string text;

        /// <summary>
        ///     Message text. Required if Template is not set
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                templateId = null;
            }
        }

        /// <summary>
        ///     Template used instead of message text. Required if Text is not set
        /// </summary>
        public int? TemplateId
        {
            get => templateId;
            set
            {
                templateId = value;
                text = null;
            }
        }

        /// <summary>
        ///     Message sending time in unix timestamp format. Optional (required with Rrule set)
        /// </summary>
        public DateTime? SendingTime { get; set; }

        /// <summary>
        ///     Array of Contact object IDs message will be sent to
        /// </summary>
        public int[] ContactIds { get; set; }

        /// <summary>
        ///     Array of List object IDs message will be sent to
        /// </summary>
        public int[] ListIds { get; set; }

        /// <summary>
        ///     Array of phone number in international E.164 format message will be sent to
        /// </summary>
        public string[] Phones { get; set; }

        /// <summary>
        ///     Optional. Should sending method cut extra characters which not fit supplied parts_count or return 400 Bad request
        ///     response instead. Default is false
        /// </summary>
        public bool? CutExtra { get; set; }

        /// <summary>
        ///     Optional. Maximum message parts count (TextMagic allows sending 1 to 6 message parts). Default is 6
        /// </summary>
        public int? PartsCount { get; set; }

        /// <summary>
        ///     Optional. Custom message reference id which can be used in your application infrastructure
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        ///     Optional. One of allowed Sender ID (phone number or alphanumeric sender ID). If specified Sender ID is not allowed
        ///     for some destinations, a fallback default Sender ID will be used to ensure delivery
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     Optional. iCal RRULE parameter to create recurrent scheduled messages. When used, SendingTime is mandatory as start
        ///     point of sending
        /// </summary>
        public string Rrule { get; set; }
    }
}