using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RestSharp;
using TextmagicRest.Model;

namespace TextmagicRest.Tests
{
    [TestFixture]
    public class ListsTests
    {
        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        private Mock<Client> mockClient;

        private const int listId = 106847;
        private const string listName = "apitestlist";
        private const string listDescription = "apitestlist description";
        private const int listMembersCount = 1;
        private const bool listIsShared = false;

        [Test]
        public void ShouldAddContactsToList()
        {
            var contactId1 = 12345;
            var contactId2 = 23456;
            var contactId3 = 34567;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var contact1 = new Contact {Id = contactId1};
            var contact2 = new Contact {Id = contactId2};
            var contact3 = new Contact {Id = contactId3};
            int[] contactIds = {contactId1, contactId2, contactId3};
            var contacts = new List<Contact> {contact1, contact2, contact3};
            var list = new ContactList {Id = listId};
            client.AddContactsToList(list, contacts);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(string.Join(",", contactIds),
                savedRequest.Parameters.Find(x => x.Name == "contacts").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/lists/31337/contacts\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.AddContactsToList(list, contacts);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/lists/31337/contacts", link.Href);
        }

        [Test]
        public void ShouldCreateList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.CreateList(listName, listIsShared);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(listName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual("0", savedRequest.Parameters.Find(x => x.Name == "shared").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/lists/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.CreateList(listName, listIsShared);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/lists/31337", link.Href);
        }

        [Test]
        public void ShouldDeleteContactsFromList()
        {
            var contactId1 = 12345;
            var contactId2 = 23456;
            var contactId3 = 34567;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var contact1 = new Contact {Id = contactId1};
            var contact2 = new Contact {Id = contactId2};
            var contact3 = new Contact {Id = contactId3};
            int[] contactIds = {contactId1, contactId2, contactId3};
            var contacts = new List<Contact> {contact1, contact2, contact3};
            var list = new ContactList {Id = listId};

            client.DeleteContactsFromList(list, contacts);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(string.Join(",", contactIds),
                savedRequest.Parameters.Find(x => x.Name == "contacts").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var link = client.DeleteContactsFromList(list, contacts);

            Assert.IsTrue(link.Success);
        }

        [Test]
        public void ShouldDeleteList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var list = new ContactList {Id = listId};

            client.DeleteList(list);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var link = client.DeleteList(list);

            Assert.IsTrue(link.Success);
        }

        [Test]
        public void ShouldGetAllLists()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new ContactListsResult());
            var client = mockClient.Object;

            client.GetLists(page, limit);

            mockClient.Verify(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 3, \"resources\": ["
                          + "{ \"id\": \"106847\", \"name\": \"apitestlist\", \"description\": \"apitestlist description\", \"membersCount\": 1, \"shared\": false }"
                          + "{ \"id\": \"106848\", \"name\": \"apitestlist 2\", \"description\": \"apitestlist description 2\", \"membersCount\": 10, \"shared\": true }"
                          + "{ \"id\": \"106849\", \"name\": \"apitestlist 3\", \"description\": \"apitestlist description 3\", \"membersCount\": 31, \"shared\": true }"
                          + "] }";

            var testClient = Common.CreateClient<ContactListsResult>(content, null, null);
            client = new Client(testClient);

            var lists = client.GetLists(page, limit);

            Assert.IsTrue(lists.Success);
            Assert.NotNull(lists.ContactLists);
            Assert.AreEqual(3, lists.ContactLists.Count);
            Assert.AreEqual(page, lists.Page);
            Assert.AreEqual(limit, lists.Limit);
            Assert.AreEqual(3, lists.PageCount);
            Assert.AreEqual(106848, lists.ContactLists[1].Id);
            Assert.AreEqual("apitestlist 2", lists.ContactLists[1].Name);
            Assert.AreEqual("apitestlist description 2", lists.ContactLists[1].Description);
            Assert.AreEqual(10, lists.ContactLists[1].MembersCount);
            Assert.IsTrue(lists.ContactLists[1].Shared);
            Assert.IsTrue(lists.ContactLists[2].Shared);
            Assert.IsFalse(lists.ContactLists[0].Shared);
        }

        [Test]
        public void ShouldGetContactsInList()
        {
            var page = 2;
            var limit = 3;
            var list = new ContactList {Id = listId};

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.GetListContacts(list.Id, page, limit);

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(list.Id.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
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

            var contacts = client.GetListContacts(list.Id, page, limit);

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
        public void ShouldGetSingleList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactList>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new ContactList());
            var client = mockClient.Object;

            client.GetList(listId);

            mockClient.Verify(trc => trc.Execute<ContactList>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content =
                "{ \"id\": \"106847\", \"name\": \"apitestlist\", \"description\": \"apitestlist description\", \"membersCount\": 1, \"shared\": false }";

            var testClient = Common.CreateClient<ContactList>(content, null, null);
            client = new Client(testClient);

            var list = client.GetList(listId);

            Assert.IsTrue(list.Success);
            Assert.AreEqual(listId, list.Id);
            Assert.AreEqual(listName, list.Name);
            Assert.AreEqual(listDescription, list.Description);
            Assert.AreEqual(listMembersCount, list.MembersCount);
            Assert.AreEqual(listIsShared, list.Shared);
        }

        [Test]
        public void ShouldSearchContactsLists()
        {
            var page = 2;
            var limit = 3;
            int[] ids = {687};
            var query = "my_query";

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new ContactListsResult());
            var client = mockClient.Object;

            client.SearchLists(page, limit, ids, query);

            mockClient.Verify(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/search", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
            Assert.AreEqual(ids[0].ToString(), savedRequest.Parameters.Find(x => x.Name == "ids").Value);
            Assert.AreEqual(query, savedRequest.Parameters.Find(x => x.Name == "query").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                          + "{ \"id\": \"687\", \"name\": \"apitestlist\", \"description\": \"apitestlist description\", \"membersCount\": 1, \"shared\": false }"
                          + "] }";

            var testClient = Common.CreateClient<ContactListsResult>(content, null, null);
            client = new Client(testClient);

            var lists = client.SearchLists(page, limit, ids, query);

            Assert.IsTrue(lists.Success);
            Assert.NotNull(lists.ContactLists);
            Assert.AreEqual(1, lists.ContactLists.Count);
            Assert.AreEqual(page, lists.Page);
            Assert.AreEqual(limit, lists.Limit);
            Assert.AreEqual(1, lists.PageCount);
            Assert.AreEqual(687, lists.ContactLists[0].Id);
            Assert.AreEqual("apitestlist", lists.ContactLists[0].Name);
            Assert.AreEqual("apitestlist description", lists.ContactLists[0].Description);
            Assert.AreEqual(1, lists.ContactLists[0].MembersCount);
            Assert.IsFalse(lists.ContactLists[0].Shared);
        }

        [Test]
        public void ShouldUpdateList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var list = new ContactList
            {
                Id = listId,
                Name = listName,
                Description = listDescription,
                MembersCount = listMembersCount,
                Shared = listIsShared
            };
            client.UpdateList(list);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(listName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual("0", savedRequest.Parameters.Find(x => x.Name == "shared").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/lists/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UpdateList(list);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/lists/31337", link.Href);
        }
    }
}