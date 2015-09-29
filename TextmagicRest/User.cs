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
        /// Get all Sender IDs.
        /// </summary>
        /// <returns></returns>
        public SenderIdsResult GetSenderIds()
        {
            return GetSenderIds(null);
        }

        /// <summary>
        /// Get all Sender IDs.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public SenderIdsResult GetSenderIds(int? page)
        {
            return GetSenderIds(page, null);
        }

        /// <summary>
        /// Get all Sender IDs.
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
        /// <param name="userId">User ID this number will be assigned to</param>
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

        /// <summary>
        /// Get current user info.
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {
            var request = new RestRequest();
            request.Resource = "user";

            return Execute<User>(request);
        }

        /// <summary>
        /// Update current user info.
        /// </summary>
        /// <param name="firstName">Account first name</param>
        /// <param name="lastName">Account last name</param>
        /// <param name="company">Account company</param>
        /// <returns></returns>
        public LinkResult UpdateUser(string firstName, string lastName, string company)
        {
            Require.Argument("firstName", firstName);
            Require.Argument("lastName", lastName);
            Require.Argument("company", company);

            var request = new RestRequest(Method.PUT);
            request.Resource = "user";
            request.AddParameter("firstName", firstName);
            request.AddParameter("lastName", lastName);
            request.AddParameter("company", company);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        /// Get all account invoices.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public InvoicesResult GetInvoices()
        {
            return GetInvoices(null);
        }

        /// <summary>
        /// Get all account invoices.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public InvoicesResult GetInvoices(int? page)
        {
            return GetInvoices(page, null);
        }

        /// <summary>
        /// Get all account invoices.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public InvoicesResult GetInvoices(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "invoices";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<InvoicesResult>(request);
        }

        /// <summary>
        /// Return messaging statistics.
        /// </summary>
        /// <param name="groupBy">Group results by specified period: day, month, year or none. Default is none</param>
        /// <param name="start">(Optional) Start date in unix timestamp format. Default is 7 days ago</param>
        /// <param name="end">(Optional) End date in unix timestamp format. Default is now</param>
        /// <returns></returns>
        public MessagingStatsResult GetMessagingStats(MessagingStatsGroupBy groupBy, DateTime start, DateTime end)
        {
            var request = new RestRequest();
            request.Resource = "stats/messaging";

            string groupByString = "off";
            switch (groupBy)
            {
                case MessagingStatsGroupBy.Day:
                    groupByString = "day";
                    break;
                case MessagingStatsGroupBy.Month:
                    groupByString = "month";
                    break;
                case MessagingStatsGroupBy.Year:
                    groupByString = "year";
                    break;
            }
            request.AddQueryParameter("by", groupByString);
            request.AddQueryParameter("start", DateTimeToTimestamp(start).ToString());
            request.AddQueryParameter("end", DateTimeToTimestamp(start).ToString());

            return Execute<MessagingStatsResult>(request);
        }

        /// <summary>
        /// Return account spending statistics.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="start">(Optional) Start date in unix timestamp format. Default is 7 days ago</param>
        /// <param name="end">(Optional) End date in unix timestamp format. Default is now</param>
        /// <returns></returns>
        public SpendingStatsResult GetSpendingStats(int? page, int? limit, DateTime start, DateTime end)
        {
            var request = new RestRequest();
            request.Resource = "stats/spending";

            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            request.AddQueryParameter("start", DateTimeToTimestamp(start).ToString());
            request.AddQueryParameter("end", DateTimeToTimestamp(end).ToString());

            return Execute<SpendingStatsResult>(request);
        }       

        /// <summary>
        /// Get a single subaccount.
        /// </summary>
        /// <param name="id">SubAccount Id</param>
        /// <returns></returns>
        public User GetSingleSubaccount(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "subaccounts/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<User>(request);
        }

        /// <summary>
        /// Close subaccount.
        /// </summary>
        /// <param name="id">SubAccount Id</param>
        /// <returns></returns>
        public DeleteResult CloseSubaccount(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "subaccounts/{id}";
            request.AddUrlSegment("id", id.ToString());
            request.Method = Method.DELETE;

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Get all subaccounts.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public UserResults GetSubAccounts(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "subaccounts";

            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<UserResults>(request);
        }

        /// <summary>
        /// Invite new Subaccount.
        /// </summary>
        /// <param name="email">Subaccount email</param>
        /// <param name="role">Subaccount role: administrator (A) or regular user (U)</param>
        /// <returns></returns>
        public LinkResult InviteSubAccount(string email, string role)
        {
            var request = new RestRequest();
            request.Resource = "subaccounts";
            request.Method = Method.POST;

            request.AddQueryParameter("email", email);
            request.AddQueryParameter("role", role);

            return Execute<LinkResult>(request);
        }
    }
}
