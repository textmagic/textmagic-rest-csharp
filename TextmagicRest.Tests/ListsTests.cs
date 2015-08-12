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
    public class ListsTests
    {
        private Mock<Client> mockClient;

        private const int listId = 106847;
        private const string listName = "apitestlist";
        private const string listDescription = "apitestlist description";
        private const int listMembersCount = 1;
        private const bool listIsShared = false;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetSingleList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactList>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactList());
            var client = mockClient.Object;

            client.GetList(listId);

            mockClient.Verify(trc => trc.Execute<ContactList>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{ \"id\": \"106847\", \"name\": \"apitestlist\", \"description\": \"apitestlist description\", \"membersCount\": 1, \"shared\": false }";

            var testClient = Common.CreateClient<ContactList>(content, null, null);
            client = new Client(testClient);

            var list = client.GetList(51335);

            Assert.IsTrue(list.Success);
            Assert.AreEqual(listId, list.Id);
            Assert.AreEqual(listName, list.Name);
            Assert.AreEqual(listDescription, list.Description);
            Assert.AreEqual(listMembersCount, list.MembersCount);
            Assert.AreEqual(listIsShared, list.Shared);
        }

        [Test]
        public void ShouldGetAllLists()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactListsResult());
            var client = mockClient.Object;

            client.GetLists(2, 3);

            mockClient.Verify(trc => trc.Execute<ContactListsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 3, \"resources\": ["
                + "{ \"id\": \"106847\", \"name\": \"apitestlist\", \"description\": \"apitestlist description\", \"membersCount\": 1, \"shared\": false }"
                + "{ \"id\": \"106848\", \"name\": \"apitestlist 2\", \"description\": \"apitestlist description\", \"membersCount\": 10, \"shared\": true }"
                + "{ \"id\": \"106849\", \"name\": \"apitestlist 3\", \"description\": \"apitestlist description\", \"membersCount\": 31, \"shared\": true }"
                + "] }";

            var testClient = Common.CreateClient<ContactListsResult>(content, null, null);
            client = new Client(testClient);

            var lists = client.GetLists(2, 3);

            Assert.IsTrue(lists.Success);
            Assert.NotNull(lists.ContactLists);
            Assert.AreEqual(3, lists.ContactLists.Count);
            Assert.AreEqual(page, lists.Page);
            Assert.AreEqual(limit, lists.Limit);
            Assert.AreEqual(3, lists.PageCount);            
            Assert.AreEqual(10, lists.ContactLists[1].MembersCount);
            Assert.IsTrue(lists.ContactLists[2].Shared);
        }

        [Test]
        public void ShouldCreateList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var link = client.CreateList(listName, listDescription, listIsShared);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(listName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual(listDescription, savedRequest.Parameters.Find(x => x.Name == "description").Value);
            Assert.AreEqual("0", savedRequest.Parameters.Find(x => x.Name == "shared").Value);
        }

        [Test]
        public void ShouldUpdateList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var list = new ContactList()
            {
                Id = listId,
                Name = listName,
                Description = listDescription,
                MembersCount = listMembersCount,
                Shared = listIsShared,
            };
            var link = client.UpdateList(list);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(listName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual(listDescription, savedRequest.Parameters.Find(x => x.Name == "description").Value);
            Assert.AreEqual("0", savedRequest.Parameters.Find(x => x.Name == "shared").Value);
        }

        [Test]
        public void ShouldDeleteList()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var list = new ContactList() { Id = listId };

            var result = client.DeleteList(list);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
        }

        [Test]
        public void ShouldGetContactsInList()
        {
            var page = 2;
            var limit = 3;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new ContactsResult());
            var client = mockClient.Object;

            client.GetListContacts(listId, page, limit);

            mockClient.Verify(trc => trc.Execute<ContactsResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
        }

        [Test]
        public void ShouldAddContactsToList()
        {
            var contactId1 = 12345;
            var contactId2 = 23456;
            var contactId3 = 34567;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var contact1 = new Contact() { Id = contactId1 };
            var contact2 = new Contact() { Id = contactId2 };
            var contact3 = new Contact() { Id = contactId3 };
            int[] contactIds = { contactId1, contactId2, contactId3 };
            var contacts = new System.Collections.Generic.List<Contact> { contact1, contact2, contact3 };
            var list = new ContactList() { Id = listId };
            var link = client.AddContactsToList(list, contacts);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(string.Join(",", contactIds), savedRequest.Parameters.Find(x => x.Name == "contacts").Value);
        }

        [Test]
        public void ShouldDeleteContactsFromList()
        {
            var contactId1 = 12345;
            var contactId2 = 23456;
            var contactId3 = 34567;

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var contact1 = new Contact() { Id = contactId1 };
            var contact2 = new Contact() { Id = contactId2 };
            var contact3 = new Contact() { Id = contactId3 };
            int[] contactIds = { contactId1, contactId2, contactId3 };
            var contacts = new System.Collections.Generic.List<Contact> { contact1, contact2, contact3 };
            var list = new ContactList() { Id = listId };

            var result = client.DeleteContactsFromList(list, contacts);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("lists/{id}/contacts", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(listId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(string.Join(",", contactIds), savedRequest.Parameters.Find(x => x.Name == "contacts").Value);
        }
    }
}
