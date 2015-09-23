using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using TextmagicRest.Model;
using RestSharp.Validation;

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        /// Ping the API
        /// </summary>
        /// <returns></returns>
        public PingResult Ping()
        {
            var request = new RestRequest();
            request.Resource = "ping";

            return Execute<PingResult>(request);
        }
    }
}
