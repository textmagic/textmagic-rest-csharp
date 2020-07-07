using System.Net;
using Moq;
using RestSharp;
using RestSharp.Serialization.Json;

namespace TextmagicRest.Tests
{
    public static class Common
    {
        public static string Username = "csharp-test-username";
        public static string Token = "csharp-test-token";

        public static IRestClient CreateClient<T>(string json, ResponseStatus? responseStatus,
            HttpStatusCode? statusCode) where T : new()
        {
            var resp = new RestResponse<T>
            {
                ContentType = "application/json",
                ResponseStatus = responseStatus.HasValue ? (ResponseStatus) responseStatus : ResponseStatus.Completed,
                StatusCode = statusCode.HasValue ? (HttpStatusCode) statusCode : HttpStatusCode.OK,
                Content = json
            };

            var deserializer = new JsonDeserializer();
            resp.Data = deserializer.Deserialize<T>(resp);

            var mock = new Mock<IRestClient>();
            mock.Setup(x => x.Execute<T>(It.IsAny<IRestRequest>()))
                .Returns(resp);

            return mock.Object;
        }
    }
}