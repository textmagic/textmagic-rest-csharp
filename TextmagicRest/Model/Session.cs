using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    public enum SendingSource
    {
        Api = 'A',
        WebApp = 'O',
        Tmm = 'T',
        EmailToSms = 'E',
        DistributionList = 'X'        
    }

    /// <summary>
    /// Message sending session
    /// </summary>
    public class Session: BaseModel
    {
        /// <summary>
        /// Session ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sesssion start time
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Session text. Contains
        /// </summary>
        public string Text { get; set; }

        [DeserializeAs(Name="source")]
        public char SourceChar { get; set; }
        /// <summary>
        /// Session sending source
        /// </summary>
        [DeserializeAs(Name = "fake-unused-name")]
        public SendingSource Source 
        {
            get { return (SendingSource)SourceChar; }
            set { SourceChar = value.ToString()[0]; } 
        }

        /// <summary>
        /// Custom Reference ID
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Session price (in account currency)
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// Session unique recipients count
        /// </summary>
        public int NumbersCount { get; set; }
    }
}
