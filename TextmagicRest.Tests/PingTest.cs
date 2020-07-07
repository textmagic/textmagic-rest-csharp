using Moq;
using NUnit.Framework;
using RestSharp;
using TextmagicRest.Model;

namespace TextmagicRest.Tests
{
    [TestFixture]
    public class PingTest
    {
        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        private Mock<Client> mockClient;

        [Test]
        public void ShouldPing()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<PingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new PingResult());
            var client = mockClient.Object;

            client.Ping();

            mockClient.Verify(trc => trc.Execute<PingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("ping", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);

            var content = "{ \"ping\": \"pong\" }";

            var testClient = Common.CreateClient<PingResult>(content, null, null);
            client = new Client(testClient);

            var pingResult = client.Ping();

            Assert.IsTrue(pingResult.Success);
            Assert.AreEqual("pong", pingResult.Ping);
        }
    }
}