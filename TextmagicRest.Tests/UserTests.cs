using System;
using System.Collections.Generic;
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
        private const int senderId = 49575710;

        private const int messageId = 49575710;
        private const string messageReceiver = "999123456";
        private string[] messageReceivers = { messageReceiver, "999234567", "999345678" };
        private const string messageSender = "447624800500";
        private DateTime messageTime = new DateTime(2015, 05, 25, 06, 40, 45, DateTimeKind.Utc);
        private const DeliveryStatus messageStatus = DeliveryStatus.Queued;
        private const string messageText = "Test C# API message";
        private const string messageCharset = "ISO-8859-1";
        private const string sessionReferenceId = "reference-id-test";
        private const string messageRrule = "FREQ=DAILY;INTERVAL=2;";
        private const bool messageCutExtra = true;

        private const int replyId = 5946228;
        private const string replyReceiver = messageSender;
        private const string replySender = messageReceiver;
        private DateTime replyTime = new DateTime(2015, 05, 25, 06, 45, 45, DateTimeKind.Utc);
        private const string replyText = "Test C# API reply";

        private const int scheduleId = 4466;
        private DateTime scheduleTime = new DateTime(2015, 05, 08, 13, 18, 38);

        private const int sessionId = 34436259;

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

            var content = "{\"id\": 12345, \"username\": \"api.test.user\", \"firstName\": \"Test\", \"lastName\": \"Api\",  \"status\": \"T\", \"balance\": 575.5,  \"company\": \"test company\","
                + "\"currency\": { \"id\": \"EUR\", \"htmlSymbol\": null }, \"timezone\": { \"area\": \"testarea\", \"dst\": 0, \"offset\": 0, \"timezone\": \"mytimezonename\"  }, \"subaccountType\": \"P\"}";

            var testClient = Common.CreateClient<User>(content, null, null);
            client = new Client(testClient);

            var user = client.GetUser();

            Assert.AreEqual(12345, user.Id);
            Assert.AreEqual("api.test.user", user.Username);
            Assert.AreEqual("Test", user.FirstName);
            Assert.AreEqual("Api", user.LastName);
            Assert.AreEqual(AccountStatus.Trial, user.Status);
            Assert.AreEqual(575.5, user.Balance);
            Assert.AreEqual("test company", user.Company);
            Assert.IsNotNull(user.Currency);
            Assert.AreEqual("EUR", user.Currency.Id);
            Assert.IsNull(user.Currency.HtmlSymbol);
            Assert.AreEqual("mytimezonename", user.Timezone.Name);
            Assert.AreEqual(SubAccountType.ParentUser, user.SubaccountType);
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

            var content = "{ \"href\": \"/api/v2/user\" }";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UpdateUser(newFirstName, newLastName, newCompany);

            Assert.IsTrue(link.Success);
            Assert.AreEqual("/api/v2/user", link.Href);
        }

        [Test]
        public void ShouldGetAllInvoices()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<InvoicesResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new InvoicesResult());
            var client = mockClient.Object;

            client.GetInvoices(page, limit);

            mockClient.Verify(trc => trc.Execute<InvoicesResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("invoices", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 3, \"resources\": ["
                + "{ \"id\": 49575710, \"bundle\": 200, \"currency\": \"USD\", \"vat\": 21.00,"
                + "\"paymentMethod\": \"MasterCard ending in 1234\" }"

                + "{ \"id\": 49575710, \"bundle\": 200, \"currency\": \"CHF\", \"vat\": 7.00,"
                + "\"paymentMethod\": \"Visa ending in 2015\" }"

                + "{ \"id\": 49575710, \"bundle\": 200, \"currency\": \"USD\", \"vat\": 2.00,"
                + "\"paymentMethod\": \"American Express ending in 3014\" }"

                + "] }";

            var testClient = Common.CreateClient<InvoicesResult>(content, null, null);
            client = new Client(testClient);

            var invoices = client.GetInvoices(page, limit);

            Assert.IsTrue(invoices.Success);
            Assert.NotNull(invoices.Invoices);
            Assert.AreEqual(3, invoices.Invoices.Count);
            Assert.AreEqual(page, invoices.Page);
            Assert.AreEqual(limit, invoices.Limit);
            Assert.AreEqual(3, invoices.PageCount);
            Assert.IsNotNull(invoices.Invoices[1].PaymentMethod);
            Assert.AreEqual("CHF", invoices.Invoices[1].Currency);
            Assert.AreEqual("USD", invoices.Invoices[2].Currency);
        }

        [Test]
        public void ShouldGetSpendingStats()
        {
            var page = 2;
            var limit = 3;
            var dateTimeStart = DateTime.Now;
            var dateTimeStop = DateTime.Now;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SpendingStatsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SpendingStatsResult());
            var client = mockClient.Object;

            client.GetSpendingStats(page, limit, dateTimeStart, dateTimeStop);

            mockClient.Verify(trc => trc.Execute<SpendingStatsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("stats/spending", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 3, \"resources\": ["
                + "{ \"id\": 49575710, \"date\": \"2014-09-19T00:00:00+0000\", \"balance\": 315.16, \"delta\": -0.15,"
                + "\"type\": \"sms\", \"value\": 315, \"comment\": \"optional comment 0\" }"

                + "{ \"id\": 49575711, \"date\": \"2015-09-19T00:00:00+0000\", \"balance\": 316.16, \"delta\": -0.16,"
                + "\"type\": \"sms\", \"value\": 315, \"comment\": \"optional comment 1\" }"

                + "{ \"id\": 49575712, \"date\": \"2016-09-19T00:00:00+0000\", \"balance\": 317.16, \"delta\": -0.17,"
                + "\"type\": \"number\", \"value\": 315, \"comment\": \"optional comment 2\" }"

                + "] }";

            var testClient = Common.CreateClient<SpendingStatsResult>(content, null, null);
            client = new Client(testClient);

            var spendingStats = client.GetSpendingStats(page, limit, dateTimeStart, dateTimeStop);

            Assert.IsTrue(spendingStats.Success);
            Assert.NotNull(spendingStats.SpendingStats);
            Assert.AreEqual(3, spendingStats.SpendingStats.Count);
            Assert.AreEqual(page, spendingStats.Page);
            Assert.AreEqual(limit, spendingStats.Limit);
            Assert.AreEqual(3, spendingStats.PageCount);
            Assert.IsNotNull(spendingStats.SpendingStats[1].Comment);
            Assert.AreEqual("sms", spendingStats.SpendingStats[1].Type);
            Assert.AreEqual("number", spendingStats.SpendingStats[2].Type);
        }

        [Test]
        public void ShouldGetMessagingStats()
        {
            IRestRequest savedRequest = null;

            var messaginStatsObject = new MessagingStats()
            {
                Date = DateTime.Now,
                Costs = 30.15,
                DeliveryRate = 0.95,
                ReplyRate = 0.32,
                MessagesReceived = 1085,
                MessagesSentParts = 150943,
                MessagesSentDelivered = 123,
                MessagesSentAccepted = 44,
                MessagesSentBuffered = 535,
                MessagesSentFailed = 1,
                MessagesSentRejected = 50
            };

            var messagingStatsInitializer = new List<MessagingStats>();
            messagingStatsInitializer.Add(messaginStatsObject);

            mockClient.Setup(trc => trc.Execute<MessagingStatsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new MessagingStatsResult() { MessagingStats = messagingStatsInitializer });
            var client = mockClient.Object;

            client.GetMessagingStats(MessagingStatsGroupBy.Month, DateTime.Now, DateTime.Today);

            mockClient.Verify(trc => trc.Execute<MessagingStatsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("stats/messaging", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual("month", savedRequest.Parameters.Find(x => x.Name == "by").Value);

            var content = "{ \"resources\": ["
                + "{ \"replyRate\": 0.32, \"date\": null, \"deliveryRate\": 0.95, \"costs\": 21.00,  \"messagesReceived\": 1085,"
                + "\"messagesSentDelivered\": 123, \"messagesSentAccepted\": 44, \"messagesSentBuffered\": 535, \"messagesSentFailed\": 1,  \"messagesSentRejected\": 50, \"messagesSentParts\": 150943 }"

                + "] }";

            var testClient = Common.CreateClient<MessagingStatsResult>(content, null, null);
            client = new Client(testClient);

            var messagingStats = client.GetMessagingStats(MessagingStatsGroupBy.None, DateTime.Now, DateTime.Today);

            Assert.AreEqual(1, messagingStats.MessagingStats.Count);
            Assert.AreEqual(21.00, messagingStats.MessagingStats[0].Costs);
            Assert.AreEqual(0.95, messagingStats.MessagingStats[0].DeliveryRate);
            Assert.AreEqual(0.32, messagingStats.MessagingStats[0].ReplyRate);
            Assert.AreEqual(1085, messagingStats.MessagingStats[0].MessagesReceived);
            Assert.AreEqual(150943, messagingStats.MessagingStats[0].MessagesSentParts);
            Assert.AreEqual(123, messagingStats.MessagingStats[0].MessagesSentDelivered);
            Assert.AreEqual(44, messagingStats.MessagingStats[0].MessagesSentAccepted);
            Assert.AreEqual(535, messagingStats.MessagingStats[0].MessagesSentBuffered);
            Assert.AreEqual(1, messagingStats.MessagingStats[0].MessagesSentFailed);
            Assert.AreEqual(50, messagingStats.MessagingStats[0].MessagesSentRejected);
        }

        [Test]
        public void ShouldDeleteSenderId()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var senderIdToDelete = new SenderId()
            {
                Id = senderId
            };

            client.DeleteSenderId(senderIdToDelete);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("senderids/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(senderId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.DeleteSenderId(senderIdToDelete);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldCreateSenderId()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            string senderIdToCreate = "senderIdToCreate";
            string explanation = "why_I_want_a_senderid";

            client.CreateSenderId(senderIdToCreate, explanation);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("senderids", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(senderIdToCreate, savedRequest.Parameters.Find(x => x.Name == "senderId").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/senderids/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.CreateSenderId(senderIdToCreate, explanation);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/senderids/31337", link.Href);
        }

        [Test]
        public void ShouldGetAvailableSendingSources()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SourcesResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SourcesResult());
            var client = mockClient.Object;

            string country = "US";

            var sourcesResult = client.GetAvailableSendingSources(country);

            mockClient.Verify(trc => trc.Execute<SourcesResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("sources", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(country, savedRequest.Parameters.Find(x => x.Name == "country").Value);

            var content = "{ \"dedicated\": null, \"user\": [], \"shared\": [\"12345\",\"390866\"], \"senderIds\": null }";

            var testClient = Common.CreateClient<SourcesResult>(content, null, null);
            client = new Client(testClient);

            var availableSendingSources = client.GetAvailableSendingSources(country);

            Assert.AreEqual(null, availableSendingSources.Dedicated);
            Assert.AreEqual(null, availableSendingSources.SenderId);
            Assert.AreEqual("12345", availableSendingSources.Shared[0]);
            Assert.AreEqual("390866", availableSendingSources.Shared[1]);
            Assert.AreEqual(new List<SourcesResult>(), availableSendingSources.User);
        }

        [Test]
        public void ShouldFindAvailableDedicatedNumbers()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<AvailableNumbersResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new AvailableNumbersResult());
            var client = mockClient.Object;

            string country = "US";
            string prefix = "447";

            client.FindAvailableDedicatedNumbers(country, prefix);

            mockClient.Verify(trc => trc.Execute<AvailableNumbersResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("numbers/available", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(country, savedRequest.Parameters.Find(x => x.Name == "country").Value);
            Assert.AreEqual(prefix, savedRequest.Parameters.Find(x => x.Name == "prefix").Value);

            var content = "{ \"numbers\": [ \"12345677\", \"35788978\" ], \"price\": 3 }";

            var testClient = Common.CreateClient<AvailableNumbersResult>(content, null, null);
            client = new Client(testClient);

            var availableDedicatedNumbers = client.FindAvailableDedicatedNumbers(country, prefix);

            Assert.IsTrue(availableDedicatedNumbers.Success);
            Assert.AreEqual("12345677", availableDedicatedNumbers.Numbers[0]);
            Assert.AreEqual("35788978", availableDedicatedNumbers.Numbers[1]);
            Assert.AreEqual(3, availableDedicatedNumbers.Price);
        }

        [Test]
        public void ShouldGetDedicatedNumbers()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DedicatedNumbersResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DedicatedNumbersResult());
            var client = mockClient.Object;

            client.GetDedicatedNumbers();

            mockClient.Verify(trc => trc.Execute<DedicatedNumbersResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("numbers", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);

            var content = "{ \"page\": 1, \"limit\": 1, \"pageCount\": 1, \"resources\": [ { \"id\": 1, \"phone\": \"3435657\", \"country\": null, \"status\": \"A\", \"purchasedAt\": null, \"expireAt\": null, \"user\": null } ] }";

            var testClient = Common.CreateClient<DedicatedNumbersResult>(content, null, null);
            client = new Client(testClient);

            var dedicatedNumbers = client.GetDedicatedNumbers();

            Assert.AreEqual(1, dedicatedNumbers.Limit);
            Assert.AreEqual(1, dedicatedNumbers.Page);
            Assert.AreEqual(1, dedicatedNumbers.PageCount);
            Assert.IsTrue(dedicatedNumbers.Success);
            Assert.AreEqual(1, dedicatedNumbers.DedicatedNumbers[0].Id);
            Assert.AreEqual("3435657", dedicatedNumbers.DedicatedNumbers[0].Phone);
            Assert.AreEqual(null, dedicatedNumbers.DedicatedNumbers[0].Country);
            Assert.AreEqual(DedicatedNumberStatus.Active, dedicatedNumbers.DedicatedNumbers[0].Status);
        }

        [Test]
        public void ShouldCancelDedicatedNumber()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var DedicatedNumberToCancel = new DedicatedNumber() { Id = 12345 };
            client.CancelDedicatedNumber(DedicatedNumberToCancel);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("numbers/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(DedicatedNumberToCancel.Id.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            
            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.CancelDedicatedNumber(DedicatedNumberToCancel);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldBuyDedicatedNumber()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            string phone = "0345233412";
            string country = "GB";
            string userId = "324";

            client.BuyDedicatedNumber(phone, country, userId);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("numbers", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(phone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
            Assert.AreEqual(country, savedRequest.Parameters.Find(x => x.Name == "country").Value);
            Assert.AreEqual(userId, savedRequest.Parameters.Find(x => x.Name == "userId").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/numbers/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.BuyDedicatedNumber(phone, country, userId);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/numbers/31337", link.Href);
        }

        [Test]
        public void ShouldGetDedicatedNumber()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DedicatedNumber>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DedicatedNumber());
            var client = mockClient.Object;

            int dedicatedNumberId = 12334;
            client.GetDedicatedNumber(dedicatedNumberId);

            mockClient.Verify(trc => trc.Execute<DedicatedNumber>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("numbers/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);

            var content = "{ \"id\": 1, \"phone\": \"3435657\", \"country\": null, \"status\": \"A\", \"purchasedAt\": null, \"expireAt\": null, \"user\": null }";

            var testClient = Common.CreateClient<DedicatedNumber>(content, null, null);
            client = new Client(testClient);

            var dedicatedNumbers = client.GetDedicatedNumber(dedicatedNumberId);

            Assert.AreEqual(1, dedicatedNumbers.Id);
            Assert.AreEqual("3435657", dedicatedNumbers.Phone);
            Assert.AreEqual(null, dedicatedNumbers.PurchasedAt);
            Assert.AreEqual(null, dedicatedNumbers.ExpireAt);
            Assert.AreEqual(null, dedicatedNumbers.Country);
            Assert.AreEqual(DedicatedNumberStatus.Active, dedicatedNumbers.Status);
            Assert.AreEqual(null, dedicatedNumbers.User);
            Assert.IsTrue(dedicatedNumbers.Success);
        }

        [Test]
        public void ShouldGetSenderId()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SenderId>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SenderId());
            var client = mockClient.Object;

            client.GetSenderId(senderId);

            mockClient.Verify(trc => trc.Execute<SenderId>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("senderids/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);


            var content = "{ \"Id\": 1, \"senderId\": \"senderId1\", \"status\": \"A\", \"user\": null }";

            var testClient = Common.CreateClient<SenderId>(content, null, null);
            client = new Client(testClient);

            var resultSenderId = client.GetSenderId(senderId);

            Assert.IsTrue(resultSenderId.Success);
            Assert.AreEqual(1, resultSenderId.Id);
            Assert.AreEqual("senderId1", resultSenderId.Name);
            Assert.AreEqual(SenderIdStatus.Active, resultSenderId.Status);
            Assert.AreEqual(null, resultSenderId.User);
        }

        [Test]
        public void ShouldGetSenderIds()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SenderIdsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SenderIdsResult());
            var client = mockClient.Object;

            int page = 1;
            int limit = 1;
            client.GetSenderIds(page, limit);

            mockClient.Verify(trc => trc.Execute<SenderIdsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("senderids", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);

            var content = "{ \"page\": 1, \"limit\": 1, \"pageCount\": 1, \"resources\": [ { \"Id\": 37, \"senderId\": \"NameOfSenderId\", \"status\": \"A\", \"user\": null } ] }";

            var testClient = Common.CreateClient<SenderIdsResult>(content, null, null);
            client = new Client(testClient);

            var resultSenderIds = client.GetSenderIds(page, limit);

            Assert.IsTrue(resultSenderIds.Success);
            Assert.AreEqual(1, resultSenderIds.Page);
            Assert.AreEqual(1, resultSenderIds.Limit);
            Assert.AreEqual(1, resultSenderIds.PageCount);
            Assert.AreEqual(37, resultSenderIds.SenderIds[0].Id);
            Assert.AreEqual("NameOfSenderId", resultSenderIds.SenderIds[0].Name);
            Assert.AreEqual(SenderIdStatus.Active, resultSenderIds.SenderIds[0].Status);
            Assert.AreEqual(null, resultSenderIds.SenderIds[0].User);
        }        

        [Test]
        public void ShouldGetSingleSubaccount()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<User>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new User());
            var client = mockClient.Object;

            int Id = 12345;
            client.GetSingleSubaccount(Id);

            mockClient.Verify(trc => trc.Execute<User>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("subaccounts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);

            var content = "{\"id\": 12345, \"username\": \"api.test.user\", \"firstName\": \"Test\", \"lastName\": \"Api\",  \"status\": \"T\", \"balance\": 575.5,  \"company\": \"test company\","
                + "\"currency\": { \"id\": \"EUR\", \"htmlSymbol\": null }, \"timezone\": null, \"subaccountType\": \"P\"}";

            var testClient = Common.CreateClient<User>(content, null, null);
            client = new Client(testClient);

            var user = client.GetSingleSubaccount(Id);

            Assert.AreEqual(12345, user.Id);
            Assert.AreEqual("api.test.user", user.Username);
            Assert.AreEqual("Test", user.FirstName);
            Assert.AreEqual("Api", user.LastName);
            Assert.AreEqual(AccountStatus.Trial, user.Status);
            Assert.AreEqual(575.5, user.Balance);
            Assert.AreEqual("test company", user.Company);
            Assert.IsNotNull(user.Currency);
            Assert.AreEqual("EUR", user.Currency.Id);
            Assert.IsNull(user.Currency.HtmlSymbol);
            Assert.IsNull(user.Timezone);
            Assert.AreEqual(SubAccountType.ParentUser, user.SubaccountType);
        }

        [Test]
        public void ShouldCloseSubaccount()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            int Id = 12345;
            client.CloseSubaccount(Id);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("subaccounts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(Id.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.CancelDedicatedNumber(Id);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldGetSubAccounts()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<UserResults>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new UserResults());
            var client = mockClient.Object;

            int page = 1;
            int limit = 1;
            client.GetSubAccounts(page, limit);

            mockClient.Verify(trc => trc.Execute<UserResults>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("subaccounts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);

            var content = "{ \"page\": 1, \"limit\": 1, \"pageCount\": 1, \"resources\": [ {\"id\": 12345, \"username\": \"api.test.user\", \"firstName\": \"Test\", \"lastName\": \"Api\",  \"status\": \"T\", \"balance\": 575.5,  \"company\": \"test company\", \"currency\": { \"id\": \"EUR\", \"htmlSymbol\": null }, \"timezone\": null, \"subaccountType\": \"P\"} ] }";

            var testClient = Common.CreateClient<UserResults>(content, null, null);
            client = new Client(testClient);

            var resultSubAccounts = client.GetSubAccounts(page, limit);

            Assert.IsTrue(resultSubAccounts.Success);
            Assert.AreEqual(1, resultSubAccounts.Page);
            Assert.AreEqual(1, resultSubAccounts.Limit);
            Assert.AreEqual(1, resultSubAccounts.PageCount);
            Assert.AreEqual(12345, resultSubAccounts.Subaccounts[0].Id);
            Assert.AreEqual("api.test.user", resultSubAccounts.Subaccounts[0].Username);
            Assert.AreEqual(575.5, resultSubAccounts.Subaccounts[0].Balance);
        }

        [Test]
        public void ShouldInviteSubAccount()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            string email = "test@company.com";
            string role = "A";

            client.InviteSubAccount(email, role);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("subaccounts", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);

            var content = "{}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var inviteResult = client.InviteSubAccount(email, role);

            Assert.IsTrue(inviteResult.Success);
        }
    }
}
