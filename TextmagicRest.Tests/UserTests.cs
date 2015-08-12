using TextmagicRest.Model;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace TextmagicRest.Tests
{
    [TestFixture]
    public class UserTests
    {
        private Mock<Client> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetUser()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<User>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new User());
            var client = mockClient.Object;

            client.GetUser();

            mockClient.Verify(trc => trc.Execute<User>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("user", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);

            var content = "{\"id\": 12345, \"username\": \"api.test.user\", \"firstName\": \"Test\", \"lastName\": \"Api\", \"balance\": 575.5, " 
                + "\"currency\": { \"id\": \"EUR\", \"htmlSymbol\": null }, \"timezone\": null }";

            var testClient = Common.CreateClient<User>(content, null, null);
            client = new Client(testClient);

            var user = client.GetUser();

            Assert.AreEqual("api.test.user", user.Username);
            Assert.AreEqual("Test", user.FirstName);
            Assert.AreEqual("Api", user.LastName);
            Assert.AreEqual(575.5, user.Balance);
            Assert.IsNotNull(user.Currency);
            Assert.AreEqual("EUR", user.Currency.Id);
            Assert.IsNull(user.Currency.HtmlSymbol);
            Assert.IsNull(user.Timezone);
        }

        [Test]
        public void ShouldUpdateUser()
        {
            const string newFirstName = "New";
            const string newLastName = "name";
            const string newCompany = "and Company";

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.UpdateUser(newFirstName, newLastName, newCompany);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("user", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var firstName = savedRequest.Parameters.Find(x => x.Name == "firstName");
            Assert.IsNotNull(firstName);
            Assert.AreEqual(newFirstName, firstName.Value);
            var lastName = savedRequest.Parameters.Find(x => x.Name == "lastName");
            Assert.IsNotNull(firstName);
            Assert.AreEqual(newLastName, lastName.Value);
            var company = savedRequest.Parameters.Find(x => x.Name == "company");
            Assert.IsNotNull(firstName);
            Assert.AreEqual(newLastName, lastName.Value);
        }       
    }
}
