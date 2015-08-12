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
        /// Get all available sender settings which could be used in "from" parameter of POST messages method.
        /// </summary>
        /// <returns></returns>
        public SourcesResult GetAvailableSendingSources()
        {
            return GetAvailableSendingSources(null);
        }

        /// <summary>
        /// Get all available sender settings which could be used in "from" parameter of POST messages method.
        /// </summary>
        /// <param name="country">2-letter ISO Country ID</param>
        /// <returns></returns>
        public SourcesResult GetAvailableSendingSources(string country)
        {
            var request = new RestRequest();
            request.Resource = "sources";
            if (!String.IsNullOrEmpty(country)) request.AddQueryParameter("country", country);

            return Execute<SourcesResult>(request);
        }

        /// <summary>
        /// Get all user Sender IDs.
        /// </summary>
        /// <returns></returns>
        public SenderIdsResult GetSenderIds()
        {
            return GetSenderIds(null);
        }

        /// <summary>
        /// Get all user Sender IDs.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public SenderIdsResult GetSenderIds(int? page)
        {
            return GetSenderIds(page, null);
        }

        /// <summary>
        /// Get all user Sender IDs.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public SenderIdsResult GetSenderIds(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "senderids";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<SenderIdsResult>(request);
        }

        /// <summary>
        /// Get a single Sender ID.
        /// </summary>
        /// <param name="id">Sender ID numeric ID</param>
        /// <returns></returns>
        public SenderId GetSenderId(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "senderids/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<SenderId>(request);
        }

        /// <summary>
        /// Delete a single Sender ID.
        /// </summary>
        /// <param name="id">Sender ID numeric ID</param>
        /// <returns></returns>
        public DeleteResult DeleteSenderId(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "senderids/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete a single Sender ID.
        /// </summary>
        /// <param name="senderId">SenderID object</param>
        /// <returns></returns>
        public DeleteResult DeleteSenderId(SenderId senderId)
        {
            return DeleteSenderId(senderId.Id);
        }

        /// <summary>
        /// Apply for a new Sender ID.
        /// </summary>
        /// <param name="senderId">Alphanumeric Sender ID (maximum 11 characters)</param>
        /// <param name="explanation">Explain why do you need this Sender ID</param>
        /// <returns></returns>
        public LinkResult CreateSenderId(string senderId, string explanation)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "senderids";
            request.AddParameter("senderId", senderId);
            request.AddParameter("explanation", explanation);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        /// Find available dedicated numbers to buy.
        /// </summary>
        /// <param name="country">Dedicated number country</param>
        /// <param name="prefix">Desired number prefix. Should include country code (i.e. 447 for GB)</param>
        /// <returns></returns>
        public AvailableNumbersResult FindAvailableDedicatedNumbers(string country, string prefix)
        {
            var request = new RestRequest();
            request.Resource = "numbers/available";

            request.AddQueryParameter("country", country);
            if (!string.IsNullOrEmpty(prefix)) request.AddQueryParameter("prefix", prefix);

            return Execute<AvailableNumbersResult>(request);
        }

        /// <summary>
        /// Get a single dedicated number.
        /// </summary>
        /// <param name="id">Dedicated number ID</param>
        /// <returns></returns>
        public DedicatedNumber GetDedicatedNumber(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "numbers/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DedicatedNumber>(request);
        }

        /// <summary>
        /// Get all dedicated numbers.
        /// </summary>
        /// <returns></returns>
        public DedicatedNumbersResult GetDedicatedNumbers()
        {
            return GetDedicatedNumbers(null);
        }

        /// <summary>
        /// Get all dedicated numbers.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public DedicatedNumbersResult GetDedicatedNumbers(int? page)
        {
            return GetDedicatedNumbers(page, null);
        }

        /// <summary>
        /// Get all dedicated numbers.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public DedicatedNumbersResult GetDedicatedNumbers(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "numbers";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<DedicatedNumbersResult>(request);
        }

        /// <summary>
        /// Cancel dedicated number subscription.
        /// </summary>
        /// <param name="id">Dedicated number ID</param>
        /// <returns></returns>
        public DeleteResult CancelDedicatedNumber(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "numbers/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Cancel dedicated number subscription.
        /// </summary>
        /// <param name="senderId">DedicatedNumber object</param>
        /// <returns></returns>
        public DeleteResult CancelDedicatedNumber(DedicatedNumber number)
        {
            return CancelDedicatedNumber(number.Id);
        }

        /// <summary>
        /// Buy a dedicated number and assign it to the specified account.
        /// </summary>
        /// <param name="phone">Desired dedicated phone number in international E.164 format</param>
        /// <param name="country">Dedicated number country</param>
        /// <param name="userId">Number assignee</param>
        /// <returns></returns>
        public LinkResult BuyDedicatedNumber(string phone, string country, string userId)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "numbers";
            request.AddParameter("phone", phone);
            request.AddParameter("country", country);
            request.AddParameter("userId", userId);

            return Execute<LinkResult>(request);
        }
    }
}
