using RestSharp;
using RestSharp.Authenticators;

namespace TextmagicRest
{
    public class TextmagicAuthenticator : IAuthenticator
    {
        public string Username { set; get; }
        public string Token { set; get; }


        public TextmagicAuthenticator(string username, string token)
        {
            Username = username;
            Token = token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            client.AddDefaultHeader("X-TM-Username", Username);
            client.AddDefaultHeader("X-TM-Key", Token);
        }
    }
}
