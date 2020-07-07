using System.Collections.Generic;
using RestSharp;
using RestSharp.Validation;
using TextmagicRest.Model;

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        ///     Get a single list.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <returns></returns>
        public ContactList GetList(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "lists/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<ContactList>(request);
        }

        /// <summary>
        ///     Get all user lists.
        /// </summary>
        /// <returns></returns>
        public ContactListsResult GetLists()
        {
            return GetLists(null);
        }

        /// <summary>
        ///     Get all user lists.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public ContactListsResult GetLists(int? page)
        {
            return GetLists(page, null);
        }

        /// <summary>
        ///     Get all user lists.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ContactListsResult GetLists(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "lists";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<ContactListsResult>(request);
        }

        /// <summary>
        ///     Find contact lists by given parameters.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="ids">Find lists by ID(s)</param>
        /// <param name="query">Find lists by specified search query</param>
        /// <returns></returns>
        public ContactListsResult SearchLists(int? page, int? limit, int[] ids, string query)
        {
            var request = new RestRequest();
            request.Resource = "lists/search";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (ids != null && ids.Length > 0) request.AddQueryParameter("ids", string.Join(",", ids));
            if (query != string.Empty) request.AddQueryParameter("query", query);

            return Execute<ContactListsResult>(request);
        }

        /// <summary>
        ///     Create a new list from the submitted data.
        /// </summary>
        /// <param name="name">List name</param>
        /// <returns></returns>
        public LinkResult CreateList(string name)
        {
            return CreateList(name, null);
        }

        /// <summary>
        ///     Create a new list from the submitted data.
        /// </summary>
        /// <param name="name">List name</param>
        /// <param name="shared">(Optional) Should this list be shared with sub-accounts</param>
        /// <returns></returns>
        public LinkResult CreateList(string name, bool? shared)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "lists";
            request.AddParameter("name", name);
            if (shared.HasValue) request.AddParameter("shared", (bool) shared ? "1" : "0");

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Update existing list.
        /// </summary>
        /// <param name="list">ContactList object</param>
        /// <returns></returns>
        public LinkResult UpdateList(ContactList list)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "lists/{id}";
            request.AddUrlSegment("id", list.Id.ToString());
            request.AddParameter("name", list.Name);
            request.AddParameter("shared", list.Shared ? "1" : "0");

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Delete a single list.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <returns></returns>
        public DeleteResult DeleteList(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "lists/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        ///     Delete a single list.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <returns></returns>
        public DeleteResult DeleteList(ContactList list)
        {
            return DeleteList(list.Id);
        }

        /// <summary>
        ///     Fetch user contacts by given list id.
        /// </summary>
        /// <param name="list">ContactList object</param>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ContactsResult GetListContacts(ContactList list, int? page, int? limit)
        {
            return GetListContacts(list.Id, page, limit);
        }

        /// <summary>
        ///     Fetch user contacts by given list id.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ContactsResult GetListContacts(int id, int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "lists/{id}/contacts";
            request.AddUrlSegment("id", id.ToString());
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<ContactsResult>(request);
        }

        /// <summary>
        ///     Assign contacts to the specified list.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <param name="contactIds">Contact IDs</param>
        /// <returns></returns>
        public LinkResult AddContactsToList(int id, int[] contactIds)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "lists/{id}/contacts";
            request.AddUrlSegment("id", id.ToString());
            if (contactIds != null && contactIds.Length > 0)
                request.AddParameter("contacts", string.Join(",", contactIds));

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Assign contacts to the specified list.
        /// </summary>
        /// <param name="list">ContactList object</param>
        /// <param name="contacts">Contacts</param>
        /// <returns></returns>
        public LinkResult AddContactsToList(ContactList list, List<Contact> contacts)
        {
            var contactIds = new List<int>();
            foreach (var contact in contacts) contactIds.Add(contact.Id);

            return AddContactsToList(list.Id, contactIds.ToArray());
        }

        /// <summary>
        ///     Unassign contacts from the specified list.
        /// </summary>
        /// <param name="id">List ID</param>
        /// <param name="contactIds">Contact IDs</param>
        /// <returns></returns>
        public DeleteResult DeleteContactsFromList(int id, int[] contactIds)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "lists/{id}/contacts";
            request.AddUrlSegment("id", id.ToString());
            if (contactIds != null && contactIds.Length > 0)
                request.AddParameter("contacts", string.Join(",", contactIds));

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        ///     Unassign contacts from the specified list.
        /// </summary>
        /// <param name="list">ContactList object</param>
        /// <param name="contacts">Contacts</param>
        /// <returns></returns>
        public DeleteResult DeleteContactsFromList(ContactList list, List<Contact> contacts)
        {
            var contactIds = new List<int>();
            foreach (var contact in contacts) contactIds.Add(contact.Id);

            return DeleteContactsFromList(list.Id, contactIds.ToArray());
        }
    }
}