using System;
using TextmagicRest.Model;
using Moq;
using NUnit.Framework;
using RestSharp;
using System.Text;
using RestSharp.Deserializers;

namespace TextmagicRest.Tests
{
    [TestFixture]
    public class SendingTests
    {
        private Mock<Client> mockClient;

        private const int messageId = 49575710;
        private const string messageReceiver = "999123456";
        private string[] messageReceivers = { messageReceiver, "999234567", "999345678" }; 
        private const string messageSender = "447624800500";
        private DateTime messageTime = new DateTime(2015, 05, 25, 06, 40, 45, DateTimeKind.Utc);
        private const DeliveryStatus messageStatus = DeliveryStatus.Queued;
        private const string messageText = "Test C# API message";
        private const string messageCharset = "ISO-8859-1";
        private int messagePartsCount = 1;
        private const string sessionReferenceId = "reference-id-test";
        private const string messageRrule = "FREQ=DAILY;INTERVAL=2;";
        private const bool messageCutExtra = true;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldSendSimpleMessageToOnePhone()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var link = client.SendMessage(messageText, messageReceiver);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(messageText, savedRequest.Parameters.Find(x => x.Name == "text").Value);
            Assert.AreEqual(messageReceiver, savedRequest.Parameters.Find(x => x.Name == "phones").Value);
        }

        [Test]
        public void ShouldSendSimpleMessageToArrayOfPhones()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var link = client.SendMessage(messageText, messageReceivers);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(messageText, savedRequest.Parameters.Find(x => x.Name == "text").Value);
            Assert.AreEqual(string.Join(",", messageReceivers), savedRequest.Parameters.Find(x => x.Name == "phones").Value);
        }

        [Test]
        public void ShouldSendMessageToArrayOfPhonesInComplexOptions()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var options = new SendingOptions 
            {
                Phones = messageReceivers,
                Text = messageText
            };
            var link = client.SendMessage(options);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(messageText, savedRequest.Parameters.Find(x => x.Name == "text").Value);
            Assert.AreEqual(string.Join(",", messageReceivers), savedRequest.Parameters.Find(x => x.Name == "phones").Value);
        }

        [Test]
        public void ShouldSendScheduledMessage()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var options = new SendingOptions
            {
                Phones = messageReceivers,
                Text = messageText,
                SendingTime = messageTime
            };
            var link = client.SendMessage(options);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(messageText, savedRequest.Parameters.Find(x => x.Name == "text").Value);
            Assert.AreEqual(string.Join(",", messageReceivers), savedRequest.Parameters.Find(x => x.Name == "phones").Value);
            Assert.AreEqual(Client.DateTimeToTimestamp(messageTime).ToString(), savedRequest.Parameters.Find(x => x.Name == "sendingTime").Value);
        }

        [Test]
        public void ShouldSendAllSetParameters()
        {
            int[] contactIds = { 385, 15 };
            int[] listIds = { 424, 454, 223 };

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var options = new SendingOptions
            {
                Phones = messageReceivers,
                Text = messageText,
                SendingTime = messageTime,
                ContactIds = contactIds,
                ListIds = listIds,
                From = messageSender,
                CutExtra = messageCutExtra,
                PartsCount = messagePartsCount,
                ReferenceId = sessionReferenceId,
                Rrule = messageRrule
            };
            var link = client.SendMessage(options);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(10, savedRequest.Parameters.Count);
            Assert.AreEqual(messageText, savedRequest.Parameters.Find(x => x.Name == "text").Value);
            Assert.AreEqual(string.Join(",", messageReceivers), savedRequest.Parameters.Find(x => x.Name == "phones").Value);
            Assert.AreEqual(string.Join(",", contactIds), savedRequest.Parameters.Find(x => x.Name == "contacts").Value);
            Assert.AreEqual(string.Join(",", listIds), savedRequest.Parameters.Find(x => x.Name == "lists").Value);
            Assert.AreEqual(Client.DateTimeToTimestamp(messageTime).ToString(), savedRequest.Parameters.Find(x => x.Name == "sendingTime").Value);
            Assert.AreEqual(messageSender, savedRequest.Parameters.Find(x => x.Name == "from").Value);
            Assert.AreEqual(messageCutExtra? "1": "0", savedRequest.Parameters.Find(x => x.Name == "cutExtra").Value);
            Assert.AreEqual(messagePartsCount.ToString(), savedRequest.Parameters.Find(x => x.Name == "partsCount").Value);
            Assert.AreEqual(sessionReferenceId, savedRequest.Parameters.Find(x => x.Name == "referenceId").Value);
            Assert.AreEqual(messageRrule, savedRequest.Parameters.Find(x => x.Name == "rrule").Value);
        }

        [Test]
        public void ShouldSendTemplate()
        {
            var templateId = 318;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new SendingResult());
            var client = mockClient.Object;

            var options = new SendingOptions
            {
                Phones = messageReceivers,
                TemplateId = templateId
            };
            var link = client.SendMessage(options);

            mockClient.Verify(trc => trc.Execute<SendingResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("messages", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(string.Join(",", messageReceivers), savedRequest.Parameters.Find(x => x.Name == "phones").Value);
            Assert.AreEqual(templateId.ToString(), savedRequest.Parameters.Find(x => x.Name == "templateId").Value);
        }
    }
}
