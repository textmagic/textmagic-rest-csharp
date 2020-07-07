﻿using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using RestSharp;
using RestSharp.Extensions;

namespace TextmagicRest
{
    public partial class Client
    {
        private const string _defaultUserAgent = "textmagic-rest-csharp/{0} (.NET {1}; {2})";

        /// <summary>
        ///     HTTP client instance
        /// </summary>
        protected IRestClient _client;

        /// <summary>
        ///     Last request time (to not to exceed maximum 2 requests per second)
        /// </summary>
        protected DateTime _lastExecuted;

        /// <summary>
        ///     Initialize TextMagic REST client instance
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
        ///     Initialize TextMagic REST client instance with special instance of RestClient
        /// </summary>
        /// <param name="client">RestClient instance</param>
        public Client(IRestClient client)
        {
            _client = client;
        }

        /// <summary>
        ///     Initialize TextMagic REST client instance with default baseUrl and timeout
        /// </summary>
        /// <param name="username">Account username</param>
        /// <param name="token">REST API access token (key)</param>
        public Client(string username, string token)
            : this(username, token, "https://rest.textmagic.com/api/v2")
        {
        }

        /// <summary>
        ///     Initialize TextMagic REST client instance with default timeout
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <param name="baseUrl"></param>
        public Client(string username, string token, string baseUrl) : this(username, token, baseUrl, 20000)
        {
        }

        /// <summary>
        ///     TextMagic account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     TextMagic REST API token (https://my.textmagic.com/online/api/rest-api/keys)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Connection User-Agent
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        ///     TextMagic REST API base URL
        /// </summary>
        public string BaseUrl { get; private set; }

        protected void _init(string username, string token, string baseUrl, int timeout)
        {
            Username = username;
            Token = token;
            BaseUrl = baseUrl;

            _client.BaseUrl = new Uri(baseUrl);
            _client.Timeout = timeout;
            _client.AddDefaultHeader("Accept-Charset", "utf-8");
            _client.Authenticator = new TextmagicAuthenticator(Username, Token);
            var assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            _client.UserAgent = string.Format(_defaultUserAgent, assemblyName.Version, Environment.Version,
                Environment.OSVersion);
        }

        /// <summary>
        ///     Check last request execution time and make delay, if needed
        /// </summary>
        protected void checkExecutionTime()
        {
            var diff = DateTime.Now - _lastExecuted;

            if (diff.TotalMilliseconds < 500) Thread.Sleep(Convert.ToInt32(500 - diff.TotalMilliseconds));

            _lastExecuted = DateTime.Now;
        }

        /// <summary>
        ///     Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public virtual T Execute<T>(IRestRequest request) where T : new()
        {
            checkExecutionTime();
            request.OnBeforeDeserialization = resp =>
            {
                // if HTTP status code >= 400 - create and save ClientException
                if ((int) resp.StatusCode >= 400)
                {
                    var clientException = "{{ \"ClientException\" : {0} }}";
                    var content = resp.RawBytes.AsString();
                    var newJson = string.Format(clientException, content);

                    resp.Content = null;
                    resp.RawBytes = Encoding.UTF8.GetBytes(newJson);
                }

                // if HTTP status code is 201 No content, add null ClientException to be success
                if (resp.StatusCode == HttpStatusCode.NoContent)
                {
                    resp.ContentType = "application/json";
                    var clientException = "{{ \"ClientException\" : null }}";
                    var content = resp.RawBytes.AsString();
                    var newJson = string.Format(clientException, content);

                    resp.Content = null;
                    resp.RawBytes = Encoding.UTF8.GetBytes(newJson);
                }
            };

            var response = _client.Execute<T>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
                throw new ClientException("Network error: " + response.StatusDescription);

            if (response.ErrorException != null)
                throw new ClientException("Invalid input: " + response.ErrorMessage, response.ErrorException);

            return response.Data;
        }

        /// <summary>
        ///     Execute a manual REST request
        /// </summary>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public virtual IRestResponse Execute(IRestRequest request)
        {
            checkExecutionTime();
            return _client.Execute(request);
        }

        /// <summary>
        ///     Convert DateTime object to timestamp
        /// </summary>
        /// <param name="dateTime">DateTime object</param>
        /// <returns></returns>
        public static int DateTimeToTimestamp(DateTime dateTime)
        {
            return Convert.ToInt32((dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }
    }
}