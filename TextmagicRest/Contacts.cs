using System;
using RestSharp;
using RestSharp.Validation;
using TextmagicRest.Model;

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        ///     Get a single contact.
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns></returns>
        public Contact GetContact(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "contacts/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Contact>(request);
        }

        /// <summary>
        ///     Get all user contacts
        /// </summary>
        /// <returns></returns>
        public ContactsResult GetContacts()
        {
            return GetContacts(null, null);
        }

        /// <summary>
        ///     Get all user contacts.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public ContactsResult GetContacts(int? page)
        {
            return GetContacts(page, null);
        }

        /// <summary>
        ///     Get all user contacts.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ContactsResult GetContacts(int? page, int? limit)
        {
            return GetContacts(page, limit, null);
        }

        /// <summary>
        ///     Get all user contacts.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="includeShared">Should shared contacts be included</param>
        /// <returns></returns>
        public ContactsResult GetContacts(int? page, int? limit, bool? includeShared)
        {
            var request = new RestRequest();
            request.Resource = "contacts";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (includeShared.HasValue) request.AddQueryParameter("shared", (bool) includeShared ? "1" : "0");

            return Execute<ContactsResult>(request);
        }

        /// <summary>
        ///     Find user contacts by given parameters.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="includeShared">Should shared contacts be included</param>
        /// <param name="ids">Find contact by ID(s)</param>
        /// <param name="listId">Find contact by List ID</param>
        /// <param name="query">Find contact by specified search query</param>
        /// <returns></returns>
        public ContactsResult SearchContacts(int? page, int? limit, bool? includeShared, int[] ids, int? listId,
            string query)
        {
            var request = new RestRequest();
            request.Resource = "contacts/search";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (includeShared.HasValue) request.AddQueryParameter("shared", Convert.ToInt32(includeShared).ToString());
            if (ids != null && ids.Length > 0) request.AddQueryParameter("ids", string.Join(",", ids));
            if (listId != null) request.AddQueryParameter("listId", listId.ToString());
            if (query != string.Empty) request.AddQueryParameter("query", query);

            return Execute<ContactsResult>(request);
        }

        /// <summary>
        ///     Create a new contact from the submitted data.
        /// </summary>
        /// <param name="phone">Contact phone number in E.164 format</param>
        /// <param name="listIds">Array of List IDs this contact will be assigned to</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <returns></returns>
        public LinkResult CreateContact(string phone, int[] listIds, string firstName, string lastName)
        {
            return CreateContact(phone, listIds, firstName, lastName, string.Empty, string.Empty);
        }

        /// <summary>
        ///     Create a new contact from the submitted data.
        /// </summary>
        /// <param name="phone">Contact phone number in E.164 format</param>
        /// <param name="listIds">Array of List IDs this contact will be assigned to</param>
        /// <param name="firstName">(Optional) First name</param>
        /// <param name="lastName">(Optional) Last name</param>
        /// <param name="companyName">(Optional) Company name</param>
        /// <param name="email">(Optional) Email</param>
        /// <returns></returns>
        public LinkResult CreateContact(string phone, int[] listIds, string firstName, string lastName,
            string companyName, string email)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "contacts";
            request.AddParameter("phone", phone);
            request.AddParameter("lists", string.Join(",", listIds));
            request.AddParameter("firstName", firstName);
            request.AddParameter("lastName", lastName);
            request.AddParameter("companyName", companyName);
            request.AddParameter("email", email);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Update existing contact.
        /// </summary>
        /// <param name="contact">Contact object</param>
        /// <param name="listIds">List IDs</param>
        /// <returns></returns>
        public LinkResult UpdateContact(Contact contact, int[] listIds)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "contacts/{id}";
            request.AddUrlSegment("id", contact.Id.ToString());
            request.AddParameter("phone", contact.Phone);
            request.AddParameter("lists", string.Join(",", listIds));
            request.AddParameter("firstName", contact.FirstName);
            request.AddParameter("lastName", contact.LastName);
            request.AddParameter("companyName", contact.CompanyName);
            request.AddParameter("email", contact.Email);

            var result = Execute<LinkResult>(request);

            if (contact.CustomFields != null)
                foreach (var customField in contact.CustomFields)
                    SetCustomFieldValue(customField.Id, contact.Id, customField.Value);

            return result;
        }

        /// <summary>
        ///     Delete a single contact.
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns></returns>
        public DeleteResult DeleteContact(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "contacts/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        ///     Delete a single contact.
        /// </summary>
        /// <param name="contact">Contact object</param>
        /// <returns></returns>
        public DeleteResult DeleteContact(Contact contact)
        {
            return DeleteContact(contact.Id);
        }

        /// <summary>
        ///     Get all contact unsubscribed from your communication.
        /// </summary>
        /// <returns></returns>
        public UnsubscribedContactsResult GetUnsubscribedContacts()
        {
            return GetUnsubscribedContacts(null);
        }

        /// <summary>
        ///     Get all contact unsubscribed from your communication.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public UnsubscribedContactsResult GetUnsubscribedContacts(int? page)
        {
            return GetUnsubscribedContacts(page, null);
        }

        /// <summary>
        ///     Get all contact unsubscribed from your communication.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public UnsubscribedContactsResult GetUnsubscribedContacts(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "unsubscribers";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<UnsubscribedContactsResult>(request);
        }

        /// <summary>
        ///     Get a single unsubscribed contact.
        /// </summary>
        /// <param name="id">Unsubscribed contact ID</param>
        /// <returns></returns>
        public UnsubscribedContact GetUnsubscribedContact(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "unsubscribers/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<UnsubscribedContact>(request);
        }

        /// <summary>
        ///     Unsubscribe contact from your communication.
        /// </summary>
        /// <param name="phone">Contact phone number (may not be in your contact list)</param>
        /// <returns></returns>
        public LinkResult UnsubscribeContact(string phone)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "unsubscribers";
            request.AddParameter("phone", phone);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Unsubscribe contact from your communication.
        /// </summary>
        /// <param name="phone">Contact object</param>
        /// <returns></returns>
        public LinkResult UnsubscribeContact(Contact contact)
        {
            return UnsubscribeContact(contact.Phone);
        }

        /// <summary>
        ///     Get all available contact custom fields.
        /// </summary>
        /// <returns></returns>
        public CustomFieldsResult GetCustomFields()
        {
            return GetCustomFields(null);
        }

        /// <summary>
        ///     Get all available contact custom fields.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public CustomFieldsResult GetCustomFields(int? page)
        {
            return GetCustomFields(page, null);
        }

        /// <summary>
        ///     Get all available contact custom fields.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public CustomFieldsResult GetCustomFields(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "customfields";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<CustomFieldsResult>(request);
        }

        /// <summary>
        ///     Get a single custom field.
        /// </summary>
        /// <param name="id">Custom field ID</param>
        /// <returns></returns>
        public CustomField GetCustomField(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "customfields/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<CustomField>(request);
        }

        /// <summary>
        ///     Create a new custom field.
        /// </summary>
        /// <param name="name">Custom field name</param>
        /// <returns></returns>
        public LinkResult CreateCustomField(string name)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "customfields";
            request.AddParameter("name", name);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Update existing custom field.
        /// </summary>
        /// <param name="customField">CustomField update</param>
        /// <returns></returns>
        public LinkResult UpdateCustomField(CustomField customField)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "customfields/{id}";
            request.AddUrlSegment("id", customField.Id.ToString());
            request.AddParameter("name", customField.Name);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Delete a single custom field.
        /// </summary>
        /// <param name="id">Custom field ID</param>
        /// <returns></returns>
        public DeleteResult DeleteCustomField(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "customfields/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        ///     Delete a single custom field.
        /// </summary>
        /// <param name="id">Custom field ID</param>
        /// <returns></returns>
        public DeleteResult DeleteCustomField(CustomField customField)
        {
            return DeleteCustomField(customField.Id);
        }

        /// <summary>
        ///     Update/Set contact's custom field value.
        /// </summary>
        /// <param name="id">Contact's custom field ID</param>
        /// <param name="contactId">Contact ID</param>
        /// <param name="value">Contact's custom field value</param>
        /// <returns></returns>
        public virtual LinkResult SetCustomFieldValue(int id, int contactId, string value)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "customfields/{id}/update";
            request.AddUrlSegment("id", id.ToString());
            request.AddParameter("contactId", contactId.ToString());
            request.AddParameter("value", value);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        ///     Get lists which contact belongs to.
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ContactListsResult GetListsWhichContactBelongsTo(int id, int? page, int? limit)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "contacts/{id}/lists";
            request.AddUrlSegment("id", id.ToString());
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<ContactListsResult>(request);
        }
    }
}