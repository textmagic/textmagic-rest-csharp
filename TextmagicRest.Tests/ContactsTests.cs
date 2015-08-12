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
    public class ContactsTests
    {
        private Mock<Client> mockClient;

        private const int contactId = 31337;
        private const string contactPhone = "999123456";
        private const string contactFirstName = "John";
        private const string contactLastName = "Doe";
        private const string contactEmail = "john@example.com";
        private const string countryId = "GB";
        private const string countryName = "United Kingdom";
        private const int customFieldId = 73;
        private const string customFieldName = "Secure ID";
        private const string customFieldValue = "ABC";
        private DateTime customFieldValueCreatedAt = new DateTime(2007, 12, 27, 13, 04, 20, DateTimeKind.Utc);
        private int[] listIds = { 123, 456 };

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetSingleContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Contact>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new Contact());
            var client = mockClient.Object;

            client.GetContact(contactId);

            mockClient.Verify(trc => trc.Execute<Contact>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);

            var content = "{ \"id\": \"31337\", \"firstName\": \"John\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123456\", \"email\": \"john@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ { \"id\": 73, \"name\": \"Secure ID\", \"value\": \"ABC\", \"createdAt\": \"2007-12-27T13:04:20+0000\" } ] }";

            var testClient = Common.CreateClient<Contact>(content, null, null);
            client = new Client(testClient);

            var contact = client.GetContact(contactId);

            Assert.IsTrue(contact.Success);
            Assert.AreEqual(contactId, contact.Id);
            Assert.AreEqual(contactFirstName, contact.FirstName);
            Assert.AreEqual(contactLastName, contact.LastName);
            Assert.AreEqual(contactPhone, contact.Phone);
            Assert.IsNull(contact.CompanyName);
            Assert.AreEqual(contactEmail, contact.Email);
            Assert.AreEqual(countryId, contact.Country.Id);
            Assert.AreEqual(countryName, contact.Country.Name);
            Assert.Greater(contact.CustomFields.Count, 0);
            Assert.AreEqual(customFieldId, contact.CustomFields[0].Id);
            Assert.AreEqual(customFieldName, contact.CustomFields[0].Name);
            Assert.AreEqual(customFieldValue, contact.CustomFields[0].Value);
            Assert.AreEqual(customFieldValueCreatedAt.ToLocalTime(), contact.CustomFields[0].CreatedAt);
        }

        [Test]
        public void ShouldGetAllContacts()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.GetContacts(page, limit);

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 3, \"resources\": ["
                + "{ \"id\": \"31337\", \"firstName\": \"John\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123456\", \"email\": \"john@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ { \"id\": 73, \"name\": \"Secure ID\", \"value\": \"ABC\", \"createdAt\": \"2007-12-27T13:04:20+0000\" } ] }"

                + "{ \"id\": \"31338\", \"first_name\": \"Jack\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123457\", \"email\": \"jack@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ ] }"
                + "] }";

            var testClient = Common.CreateClient<ContactsResult>(content, null, null);
            client = new Client(testClient);

            var contacts = client.GetContacts(2, 3);

            Assert.IsTrue(contacts.Success);
            Assert.NotNull(contacts.Contacts);
            Assert.AreEqual(2, contacts.Contacts.Count);
            Assert.AreEqual(page, contacts.Page);
            Assert.AreEqual(limit, contacts.Limit);
            Assert.AreEqual(3, contacts.PageCount);
            Assert.IsNotNull(contacts.Contacts[0].CustomFields);
            Assert.AreEqual(1, contacts.Contacts[0].CustomFields.Count);
            Assert.IsEmpty(contacts.Contacts[1].CustomFields);
        }

        [Test]
        public void ShouldUpdateContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var contact = new Contact()
            {
                Id = contactId,
                Phone = contactPhone
            };
            var link = client.UpdateContact(contact, listIds);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
            Assert.AreEqual(string.Join(",", listIds), savedRequest.Parameters.Find(x => x.Name == "lists").Value);
        }

        [Test]
        public void ShouldCreateContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var link = client.CreateContact(contactPhone, listIds, contactFirstName, contactLastName);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            Assert.AreEqual(contactFirstName, savedRequest.Parameters.Find(x => x.Name == "firstName").Value);
            Assert.AreEqual(contactLastName, savedRequest.Parameters.Find(x => x.Name == "lastName").Value);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
            Assert.AreEqual(string.Join(",", listIds), savedRequest.Parameters.Find(x => x.Name == "lists").Value);
        }

        [Test]
        public void ShouldDeleteContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var contact = new Contact()
            {
                Id = contactId
            };

            var result = client.DeleteContact(contact);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
        }

        [Test]
        public void ShouldGetSingleUnsubscribedContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<UnsubscribedContact>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new UnsubscribedContact());
            var client = mockClient.Object;

            client.GetUnsubscribedContact(contactId);

            mockClient.Verify(trc => trc.Execute<UnsubscribedContact>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("unsubscribers/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);

            var content = "{ \"id\": \"31337\", \"firstName\": \"John\", \"lastName\": \"Doe\","
                + "\"phone\": \"999123456\", \"unsubscribeTime\": \"2007-12-27T13:04:20+0000\" } ] }";

            var testClient = Common.CreateClient<UnsubscribedContact>(content, null, null);
            client = new Client(testClient);

            var contact = client.GetUnsubscribedContact(contactId);

            Assert.IsTrue(contact.Success);
            Assert.AreEqual(contactId, contact.Id);
            Assert.AreEqual(contactFirstName, contact.FirstName);
            Assert.AreEqual(contactLastName, contact.LastName);
            Assert.AreEqual(contactPhone, contact.Phone);
            Assert.AreEqual(customFieldValueCreatedAt.ToLocalTime(), contact.UnsubscribedAt);
        }

        [Test]
        public void ShouldGetAllUnsubscribedContacts()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<UnsubscribedContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new UnsubscribedContactsResult());
            var client = mockClient.Object;

            client.GetUnsubscribedContacts(2, 3);

            mockClient.Verify(trc => trc.Execute<UnsubscribedContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("unsubscribers", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
        }

        [Test]
        public void ShouldUnsubscribeContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var contact = new Contact() { Phone = contactPhone };
            var link = client.UnsubscribeContact(contact);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("unsubscribers", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
        }

        [Test]
        public void ShouldGetSingleCustomField()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<CustomField>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new CustomField());
            var client = mockClient.Object;

            client.GetCustomField(customFieldId);

            mockClient.Verify(trc => trc.Execute<CustomField>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldGetAllCustomFields()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<CustomFieldsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new CustomFieldsResult());
            var client = mockClient.Object;

            client.GetCustomFields(2, 3);

            mockClient.Verify(trc => trc.Execute<CustomFieldsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
        }

        [Test]
        public void ShouldCreateCustomField()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var link = client.CreateCustomField(customFieldName);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldName, savedRequest.Parameters.Find(x => x.Name == "fieldName").Value);
        }

        [Test]
        public void ShouldUpdateCustomField()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var cf = new CustomField()
            {
                Id = customFieldId,
                Name = customFieldName
            };
            var link = client.UpdateCustomField(cf);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(customFieldName, savedRequest.Parameters.Find(x => x.Name == "fieldName").Value);
        }

        [Test]
        public void ShouldAssignCustomFieldValueToContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Contact>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new Contact());
            var client = mockClient.Object;

            var cf = new CustomField()
            {
                Id = customFieldId,
                Name = customFieldName
            };
            var contact = client.SetCustomFieldValue(customFieldId, contactId, customFieldValue);

            mockClient.Verify(trc => trc.Execute<Contact>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}/update", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "contactId").Value);
            Assert.AreEqual(customFieldValue, savedRequest.Parameters.Find(x => x.Name == "value").Value);
        }
    }
}
