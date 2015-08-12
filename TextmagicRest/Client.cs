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
using RestSharp.Extensions;

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        /// TextMagic account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// TextMagic REST API token (https://my.textmagic.com/online/api/rest-api/keys)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Connection User-Agent
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// TextMagic REST API base URL
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// HTTP client instance
        /// </summary>
        protected IRestClient _client;

        /// <summary>
        /// Last request time (to not to exceed maximum 2 requests per second)
        /// </summary>
        protected DateTime _lastExecuted;

        private const string _defaultUserAgent = "textmagic-rest-csharp/{0} (.NET {1}; {2})";

        protected void _init(string username, string token, string baseUrl, int timeout) 
        {
            Username = username;
            Token = token;
            BaseUrl = baseUrl;

            _client.BaseUrl = new Uri(baseUrl);
            _client.Timeout = timeout;
            _client.AddDefaultHeader("Accept-Charset", "utf-8");
            _client.Authenticator = new TextmagicAuthenticator(Username, Token);
            System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName);            
            _client.UserAgent = String.Format(_defaultUserAgent, assemblyName.Version, Environment.Version.ToString(), Environment.OSVersion.ToString());
        }

        /// <summary>
        /// Initialize TextMagic REST client instance
        /// </summary>
        /// <param name="username">Account username</param>
        /// <param name="token">REST API access token (key)</param>
        /// <param name="baseUrl">API base URL</param>
        /// <param name="timeout">Request timeout</param>
        public Client(string username, string token, string baseUrl, int timeout)
        {
            _client = new RestClient();
            _init(username, token, baseUrl, timeout);
        }

        /// <summary>
        /// Initialize TextMagic REST client instance with special instance of RestClient
        /// </summary>
        /// <param name="client">RestClient instance</param>
        public Client(IRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Initialize TextMagic REST client instance with default baseUrl and timeout
        /// </summary>
        /// <param name="username">Account username</param>
        /// <param name="token">REST API access token (key)</param>
        public Client(string username, string token)
            : this(username, token, "https://rest.textmagic.com/api/v2")
        {

        }

        /// <summary>
        /// Initialize TextMagic REST client instance with default timeout
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <param name="baseUrl"></param>
        public Client(string username, string token, string baseUrl) : this(username, token, baseUrl, 20000)
        {

        }

        /// <summary>
        /// Check last request execution time and make delay, if needed
        /// </summary>
        protected void checkExecutionTime()
        {
            var diff = DateTime.Now - _lastExecuted;

            if (diff.TotalMilliseconds < 500)
            {
                System.Threading.Thread.Sleep(Convert.ToInt32(500 - diff.TotalMilliseconds));
            }

            _lastExecuted = DateTime.Now;
        }

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public virtual T Execute<T>(IRestRequest request) where T : new()
        {
            checkExecutionTime();
            request.OnBeforeDeserialization = (resp) =>
            {
                // if HTTP status code >= 400 - create and save ClientException
                if (((int)resp.StatusCode) >= 400)
                {
                    string clientException = "{{ \"ClientException\" : {0} }}";
                    var content = resp.RawBytes.AsString();
                    var newJson = string.Format(clientException, content);

                    resp.Content = null;
                    resp.RawBytes = Encoding.UTF8.GetBytes(newJson.ToString());
                }

                // if HTTP status code is 201 No content, add null ClientException to be success
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    resp.ContentType = "application/json";
                    string clientException = "{{ \"ClientException\" : null }}";
                    var content = resp.RawBytes.AsString();
                    var newJson = string.Format(clientException, content);

                    resp.Content = null;
                    resp.RawBytes = Encoding.UTF8.GetBytes(newJson.ToString());
                }
            };

            var response = _client.Execute<T>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new ClientException("Network error: " + response.StatusDescription);
            }

            /*switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new ClientException("Invalid username and password supplied");
                case System.Net.HttpStatusCode.BadGateway:
                    throw new ClientException("Service is temporary unavailable");
                // 429 Too many requests
                case (System.Net.HttpStatusCode)429:                 
                    throw new ClientException("Too many requests");

            }*/

            if (response.ErrorException != null)
            {
                throw new ClientException("Invalid input: " + response.ErrorMessage, response.ErrorException);
            }

            return response.Data;
        }

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public virtual IRestResponse Execute(IRestRequest request)
        {
            checkExecutionTime();
            return _client.Execute(request);
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
        /// Convert DateTime object to timestamp
        /// </summary>
        /// <param name="dateTime">DateTime object</param>
        /// <returns></returns>
        public static int DateTimeToTimestamp(DateTime dateTime)
        {
            return Convert.ToInt32((dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
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
        public List<MessagingStats> GetMessagingStats(MessagingStatsGroupBy groupBy, DateTime start, DateTime end)
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

            return Execute<List<MessagingStats>>(request);
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
            request.AddQueryParameter("end", DateTimeToTimestamp(start).ToString());

            return Execute<SpendingStatsResult>(request);
        }
    }
}
