using System;
using System.Collections.Generic;
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
        private const string contactCompanyName = "JohnCo";
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

            var contacts = client.GetContacts(page, limit);

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
        public void ShouldGetAllContactsWithDefaultParameters()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.GetContacts();

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);

            var content = "{ \"page\": 1,  \"limit\": 10, \"pageCount\": 3, \"resources\": ["
                + "{ \"id\": \"31337\", \"firstName\": \"John\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123456\", \"email\": \"john@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ { \"id\": 73, \"name\": \"Secure ID\", \"value\": \"ABC\", \"createdAt\": \"2007-12-27T13:04:20+0000\" } ] }"

                + "{ \"id\": \"31338\", \"first_name\": \"Jack\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123457\", \"email\": \"jack@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ ] }"
                + "] }";

            var testClient = Common.CreateClient<ContactsResult>(content, null, null);
            client = new Client(testClient);

            var contacts = client.GetContacts();

            Assert.IsTrue(contacts.Success);
            Assert.NotNull(contacts.Contacts);
            Assert.AreEqual(2, contacts.Contacts.Count);
            Assert.AreEqual(1, contacts.Page);
            Assert.AreEqual(10, contacts.Limit);
            Assert.AreEqual(3, contacts.PageCount);
            Assert.IsNotNull(contacts.Contacts[0].CustomFields);
            Assert.AreEqual(1, contacts.Contacts[0].CustomFields.Count);
            Assert.IsEmpty(contacts.Contacts[1].CustomFields);
        }

        [Test]
        public void ShouldGetAllContactsWithSharedIncluded()
        {
            var page = 2;
            var limit = 3;
            var shared = true;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.GetContacts(page, limit, shared);

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
            Assert.AreEqual(Convert.ToInt32(shared).ToString(), savedRequest.Parameters.Find(x => x.Name == "shared").Value);

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

            var contacts = client.GetContacts(page, limit);

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
            mockClient.Setup(trc => trc.SetCustomFieldValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var contact = new Contact()
            {
                Id = contactId,
                Phone = contactPhone,
                FirstName = contactFirstName,
                LastName = contactLastName,
                CompanyName = contactCompanyName,
                Country = new Country() { Id = countryId, Name = countryName },
                Email = contactEmail
            };

            CustomField testCustomField = new CustomField() { Id = 1, Name = "Id1", Value = "StringId1" };
            contact.CustomFields = new List<CustomField>();
            contact.CustomFields.Add(testCustomField);

            client.UpdateContact(contact, listIds);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            mockClient.Verify(trc => trc.SetCustomFieldValue(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
            Assert.AreEqual(string.Join(",", listIds), savedRequest.Parameters.Find(x => x.Name == "lists").Value);
            Assert.AreEqual(contactFirstName, savedRequest.Parameters.Find(x => x.Name == "firstName").Value);
            Assert.AreEqual(contactLastName, savedRequest.Parameters.Find(x => x.Name == "lastName").Value);
            Assert.AreEqual(contactEmail, savedRequest.Parameters.Find(x => x.Name == "email").Value);
            Assert.AreEqual(contactCompanyName, savedRequest.Parameters.Find(x => x.Name == "companyName").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UpdateContact(contact, listIds);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
        }

        [Test]
        public void ShouldCreateContact()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.CreateContact(contactPhone, listIds, contactFirstName, contactLastName, contactCompanyName, contactEmail);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            Assert.AreEqual(contactFirstName, savedRequest.Parameters.Find(x => x.Name == "firstName").Value);
            Assert.AreEqual(contactLastName, savedRequest.Parameters.Find(x => x.Name == "lastName").Value);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);
            Assert.AreEqual(contactEmail, savedRequest.Parameters.Find(x => x.Name == "email").Value);
            Assert.AreEqual(contactCompanyName, savedRequest.Parameters.Find(x => x.Name == "companyName").Value);
            Assert.AreEqual(string.Join(",", listIds), savedRequest.Parameters.Find(x => x.Name == "lists").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.CreateContact(contactPhone, listIds, contactFirstName, contactLastName, contactCompanyName, contactEmail);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
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

            client.DeleteContact(contact);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.DeleteContact(contact);

            Assert.IsTrue(result.Success);
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

            client.GetUnsubscribedContacts(page, limit);

            mockClient.Verify(trc => trc.Execute<UnsubscribedContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("unsubscribers", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                + "{ \"id\": \"276\", \"firstName\": \"John\", \"lastName\": \"Doe\","
                + "\"phone\": \"999123456\", \"unsubscribeTime\": \"2007-12-27T13:04:20+0000\" } ] }";

            var testClient = Common.CreateClient<UnsubscribedContactsResult>(content, null, null);
            client = new Client(testClient);

            var results = client.GetUnsubscribedContacts(page, limit);

            Assert.IsTrue(results.UnsubscribedContacts[0].Success);
            Assert.AreEqual(276, results.UnsubscribedContacts[0].Id);
            Assert.AreEqual("John", results.UnsubscribedContacts[0].FirstName);
            Assert.AreEqual("Doe", results.UnsubscribedContacts[0].LastName);
            Assert.AreEqual("999123456", results.UnsubscribedContacts[0].Phone);
            Assert.IsNotNull(results.UnsubscribedContacts[0].UnsubscribedAt);
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
            client.UnsubscribeContact(contact);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("unsubscribers", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(contactPhone, savedRequest.Parameters.Find(x => x.Name == "phone").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UnsubscribeContact(contact);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
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
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{ \"id\": \"31337\", \"name\": \"customfieldname\", \"createdAt\": \"2007-12-27T13:04:20+0000\"}";

            var testClient = Common.CreateClient<CustomField>(content, null, null);
            client = new Client(testClient);

            var link = client.GetCustomField(customFieldId);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("customfieldname", link.Name);
            Assert.IsNotNull(link.CreatedAt);
            Assert.IsNull(link.Value);
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

            client.GetCustomFields(page, limit);

            mockClient.Verify(trc => trc.Execute<CustomFieldsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                          + "{ \"id\": 73, \"name\": \"Secure ID\", \"value\": \"ABC\", \"createdAt\": \"2007-12-27T13:04:20+0000\" },"
                          + "{ \"id\": 589, \"name\": \"Secure ID2\", \"value\": \"ABCD\", \"createdAt\": \"2007-12-27T13:04:20+0000\" }"
                          + "] }";

            var testClient = Common.CreateClient<CustomFieldsResult>(content, null, null);
            client = new Client(testClient);

            var results = client.GetCustomFields(page, limit);

            Assert.IsTrue(results.CustomFields[0].Success);
            Assert.AreEqual(73, results.CustomFields[0].Id);
            Assert.AreEqual("Secure ID", results.CustomFields[0].Name);
            Assert.AreEqual("ABC", results.CustomFields[0].Value);
            Assert.IsNotNull(results.CustomFields[0].CreatedAt);

            Assert.IsTrue(results.CustomFields[1].Success);
            Assert.AreEqual(589, results.CustomFields[1].Id);
            Assert.AreEqual("Secure ID2", results.CustomFields[1].Name);
            Assert.AreEqual("ABCD", results.CustomFields[1].Value);
            Assert.IsNotNull(results.CustomFields[1].CreatedAt);
        }

        [Test]
        public void ShouldCreateCustomField()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.CreateCustomField(customFieldName);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldName, savedRequest.Parameters.Find(x => x.Name == "name").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.CreateCustomField(customFieldName);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
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
            client.UpdateCustomField(cf);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(customFieldName, savedRequest.Parameters.Find(x => x.Name == "name").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UpdateCustomField(cf);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
        }

        [Test]
        public void ShouldDeleteCustomField()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var customField = new CustomField()
            {
                Id = customFieldId
            };

            client.DeleteCustomField(customField);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            
            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.DeleteCustomField(customField);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldSearchContacts()
        {
            var page = 2;
            var limit = 3;
            var shared = false;
            int[] ids = {276};
            int listId = 123;
            string query = "my_query";

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.SearchContacts(page, limit, shared, ids, listId, query);

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/search", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
            Assert.AreEqual(Convert.ToInt32(shared).ToString(), savedRequest.Parameters.Find(x => x.Name == "shared").Value);
            Assert.AreEqual(ids[0].ToString(), savedRequest.Parameters.Find(x => x.Name == "ids").Value);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "listId").Value);
            Assert.AreEqual(query.ToString(), savedRequest.Parameters.Find(x => x.Name == "query").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                + "{ \"id\": \"276\", \"firstName\": \"John\", \"lastName\": \"Doe\", \"companyName\": null,"
                + "\"phone\": \"999123456\", \"email\": \"john@example.com\", \"country\": { \"id\": \"GB\", \"name\": \"United Kingdom\" },"
                + "\"customFields\": [ { \"id\": 73, \"name\": \"Secure ID\", \"value\": \"ABC\", \"createdAt\": \"2007-12-27T13:04:20+0000\" } ] }"
                + "] }";

            var testClient = Common.CreateClient<ContactsResult>(content, null, null);
            client = new Client(testClient);

            var contacts = client.SearchContacts(page, limit, shared, ids, listId, query);

            Assert.IsTrue(contacts.Success);
            Assert.NotNull(contacts.Contacts);
            Assert.AreEqual("John", contacts.Contacts[0].FirstName);
            Assert.AreEqual("Doe", contacts.Contacts[0].LastName);
            Assert.AreEqual("john@example.com", contacts.Contacts[0].Email);
            Assert.AreEqual("999123456", contacts.Contacts[0].Phone);
            Assert.AreEqual(null, contacts.Contacts[0].CompanyName);
            Assert.AreEqual(ids[0], contacts.Contacts[0].Id);
            Assert.AreEqual("GB", contacts.Contacts[0].Country.Id);
            Assert.AreEqual("United Kingdom", contacts.Contacts[0].Country.Name);
            Assert.AreEqual(1, contacts.Contacts.Count);
            Assert.AreEqual(page, contacts.Page);
            Assert.AreEqual(limit, contacts.Limit);
            Assert.AreEqual(1, contacts.PageCount);
            Assert.IsNotNull(contacts.Contacts[0].CustomFields);
            Assert.AreEqual(1, contacts.Contacts[0].CustomFields.Count);
        }

        [Test]
        public void ShouldUpdateSetCustomFieldValue()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.SetCustomFieldValue(customFieldId, contactId, customFieldValue);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("customfields/{id}/update", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(customFieldId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "contactId").Value);
            Assert.AreEqual(customFieldValue, savedRequest.Parameters.Find(x => x.Name == "value").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.SetCustomFieldValue(customFieldId, contactId, customFieldValue);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
        }

        [Test]
        public void ShouldGetListsWhichContactBelongsTo()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactListsResult());
            var client = mockClient.Object;

            client.GetListsWhichContactBelongsTo(contactId, page, limit);

            mockClient.Verify(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("contacts/{id}/lists", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(contactId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                + "{ \"id\": \"31337\", \"name\": \"Partners\", \"description\": \"\","
                + "\"membersCount\": 1, \"shared\": false } ] }";

            var testClient = Common.CreateClient<ContactListsResult>(content, null, null);
            client = new Client(testClient);

            var results = client.GetListsWhichContactBelongsTo(contactId, page, limit);

            Assert.IsTrue(results.Success);
            Assert.AreEqual(contactId, results.ContactLists[0].Id);
            Assert.AreEqual("Partners", results.ContactLists[0].Name);
            Assert.AreEqual("", results.ContactLists[0].Description);
            Assert.AreEqual(1, results.ContactLists[0].MembersCount);
            Assert.AreEqual(false, results.ContactLists[0].Shared);
        }
    }
}
