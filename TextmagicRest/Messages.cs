using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using TextmagicRest.Model;
using RestSharp.Validation;
using System.Text.RegularExpressions;

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        /// Get a single outgoing message.
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <returns></returns>
        public Message GetMessage(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "messages/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Message>(request);
        }

        /// <summary>
        /// Get all user outbound messages.
        /// </summary>
        /// <returns></returns>
        public MessagesResult GetMessages()
        {
            return GetMessages(null);
        }

        /// <summary>
        /// Get all user outbound messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public MessagesResult GetMessages(int? page)
        {
            return GetMessages(page, null);
        }
        
        /// <summary>
        /// Get all user outbound messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public MessagesResult GetMessages(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "messages";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<MessagesResult>(request);
        }

        /// <summary>
        /// Find outbound messages by given parameters.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="ids">Find message by ID(s)</param>
        /// <param name="sessionId">Find messages by session ID</param>
        /// <param name="query">Find messages by specified search query</param>
        /// <returns></returns>
        public MessagesResult SearchMessages(int? page, int? limit, int[] ids, int? sessionId, string query)
        {
            var request = new RestRequest();
            request.Resource = "messages/search";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (sessionId.HasValue) request.AddQueryParameter("sessionId", limit.ToString());
            if (ids != null && ids.Length > 0) request.AddQueryParameter("ids", string.Join(",", ids));
            if (query != string.Empty) request.AddQueryParameter("query", query);

            return Execute<MessagesResult>(request);
        }

        /// <summary>
        /// Delete a single message.
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <returns></returns>
        public DeleteResult DeleteMessage(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "messages/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete a single message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public DeleteResult DeleteMessage(Message message)
        {
            return DeleteMessage(message.Id);
        }

        /// <summary>
        /// Send a new outbound message.
        /// </summary>
        /// <param name="options">Message sending options</param>
        /// <returns></returns>
        public SendingResult SendMessage(SendingOptions options)
        {
            var request = new RestRequest(Method.POST);
            request = _makeSendingRequest(request, options);

            return Execute<SendingResult>(request);
        }        

        /// <summary>
        /// Convert SendingOptions to RestRequest parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected RestRequest _makeSendingRequest(RestRequest request, SendingOptions options)
        {
            request.Resource = "messages";
            if (!string.IsNullOrEmpty(options.Text)) request.AddParameter("text", options.Text);
            if (options.Phones != null && options.Phones.Length > 0) request.AddParameter("phones", string.Join(",", options.Phones));
            if (options.ContactIds != null && options.ContactIds.Length > 0) request.AddParameter("contacts", string.Join(",", options.ContactIds));
            if (options.ListIds != null && options.ListIds.Length > 0) request.AddParameter("lists", string.Join(",", options.ListIds));
            if (options.CutExtra.HasValue) request.AddParameter("cutExtra", (bool)options.CutExtra ? "1" : "0");
            if (options.PartsCount.HasValue) request.AddParameter("partsCount", options.PartsCount.ToString());
            if (!string.IsNullOrEmpty(options.ReferenceId)) request.AddParameter("referenceId", options.ReferenceId);
            if (!string.IsNullOrEmpty(options.From)) request.AddParameter("from", options.From);
            if (!string.IsNullOrEmpty(options.Rrule)) request.AddParameter("rrule", options.Rrule);
            if (options.SendingTime.HasValue) request.AddParameter("sendingTime", DateTimeToTimestamp((DateTime)options.SendingTime).ToString());
            if (options.TemplateId.HasValue) request.AddParameter("templateId", options.TemplateId.ToString());

            return request;
        }

        /// <summary>
        /// Send a new outbound message.
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="phones">Array of phone number in international E.164 format message will be sent to</param>
        /// <returns></returns>
        public SendingResult SendMessage(string text, string[] phones)
        {
            var options = new SendingOptions
            {
                Text = text,
                Phones = phones
            };

            return SendMessage(options);
        }

        /// <summary>
        /// Send a new outbound message.
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="phone">Destination phone number in international E.164 format</param>
        /// <returns></returns>
        public SendingResult SendMessage(string text, string phone)
        {
            string[] phones = { phone };
            var options = new SendingOptions
            {
                Text = text,
                Phones = phones
            };

            return SendMessage(options);
        }

        /// <summary>
        /// Get bulk message session status.
        /// </summary>
        /// <param name="id">Bulk session ID</param>
        /// <returns></returns>
        public BulkSession GetBulkSessionStatus(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "bulks/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<BulkSession>(request);
        }

        /// <summary>
        /// Get a message session.
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <returns></returns>
        public Session GetSession(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "sessions/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Session>(request);
        }

        /// <summary>
        /// Fetch messages by given session id.
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <returns></returns>
        public MessagesResult GetSessionMessages(int id, int? page, int? limit)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "sessions/{id}/messages";
            request.AddUrlSegment("id", id.ToString());
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<MessagesResult>(request);
        }

        /// <summary>
        /// Delete a message session, together with all nested messages.
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <returns></returns>
        public DeleteResult DeleteSession(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "sessions/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete a message session, together with all nested messages.
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns></returns>
        public DeleteResult DeleteSession(Session session)
        {
            return DeleteSession(session.Id);
        }

        /// <summary>
        /// Get message schedule.
        /// </summary>
        /// <param name="id">Schedule ID</param>
        /// <returns></returns>
        public Schedule GetSchedule(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "schedules/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Schedule>(request);
        }

        /// <summary>
        /// Get all scheduled messages.
        /// </summary>
        /// <returns></returns>
        public SchedulesResult GetSchedules()
        {
            return GetSchedules(null);
        }

        /// <summary>
        /// Get all scheduled messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public SchedulesResult GetSchedules(int? page)
        {
            return GetSchedules(page, null);
        }

        /// <summary>
        /// Get all scheduled messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public SchedulesResult GetSchedules(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "schedules";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<SchedulesResult>(request);
        }

        /// <summary>
        /// Delete a message session, together with all nested messages.
        /// </summary>
        /// <param name="id">Schedule ID</param>
        /// <returns></returns>
        public DeleteResult DeleteSchedule(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "schedules/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete a message session, together with all nested messages.
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public DeleteResult DeleteSchedule(Schedule schedule)
        {
            return DeleteSchedule(schedule.Id);
        }

        /// <summary>
        /// Get a single inbox message.
        /// </summary>
        /// <param name="id">Inbox message ID</param>
        /// <returns></returns>
        public Reply GetReply(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "replies/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Reply>(request);
        }

        /// <summary>
        /// Get all inbox messages.
        /// </summary>
        /// <returns></returns>
        public RepliesResult GetReplies()
        {
            return GetReplies(null, null);
        }

        /// <summary>
        /// Get all inbox messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public RepliesResult GetReplies(int? page)
        {
            return GetReplies(page, null);
        }

        /// <summary>
        /// Get all inbox messages.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public RepliesResult GetReplies(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "replies";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<RepliesResult>(request);
        }

        /// <summary>
        /// Delete the incoming message.
        /// </summary>
        /// <param name="id">Inbound message ID</param>
        /// <returns></returns>
        public DeleteResult DeleteReply(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "replies/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete the incoming message.
        /// </summary>
        /// <param name="reply">Reply object</param>
        /// <returns></returns>
        public DeleteResult DeleteReply(Reply reply)
        {
            return DeleteReply(reply.Id);
        }

        /// <summary>
        /// Find inbound messages by given parameters.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="ids">Find message by ID(s)</param>
        /// <param name="query">Find messages by specified search query</param>
        /// <returns></returns>
        public RepliesResult SearchReplies(int? page, int? limit, int[] ids, string query)
        {
            var request = new RestRequest();
            request.Resource = "replies/search";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (ids != null && ids.Length > 0) request.AddQueryParameter("ids", string.Join(",", ids));
            if (query != string.Empty) request.AddQueryParameter("query", query);

            return Execute<RepliesResult>(request);
        }

        /// <summary>
        /// Check pricing for a new outbound message.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Pricing GetPrice(SendingOptions options)
        {
            var request = new RestRequest(Method.POST);
            request = _makeSendingRequest(request, options);
            request.Method = Method.GET;
            request.Resource = "messages/price";
            request.AddParameter("dummy", "1");

            return Execute<Pricing>(request);
        }

        /// <summary>
        /// Get all user chats.
        /// </summary>
        /// <returns></returns>
        public ChatsResult GetChats()
        {
            return GetChats(null);
        }

        /// <summary>
        /// Get all user chats.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public ChatsResult GetChats(int? page)
        {
            return GetChats(page, null);
        }

        /// <summary>
        /// Get all user chats.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ChatsResult GetChats(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "chats";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<ChatsResult>(request);
        }

        /// <summary>
        /// Fetch messages from chat with specified phone number.
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns></returns>
        public ChatMessagesResult GetChat(string phone)
        {
            return GetChat(phone, null);
        }

        /// <summary>
        /// Fetch messages from chat with specified phone number.
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public ChatMessagesResult GetChat(string phone, int? page)
        {
            return GetChat(phone, page, null);
        }

        /// <summary>
        /// Fetch messages from chat with specified phone number.
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public ChatMessagesResult GetChat(string phone, int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "chats/{phone}";
            request.AddUrlSegment("phone", phone);
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<ChatMessagesResult>(request);
        }

        /// <summary>
        /// Get all bulk sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public BulkSessionsResult GetBulkSessions()
        {
            return GetBulkSessions(null);
        }

        /// <summary>
        /// Get all bulk sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public BulkSessionsResult GetBulkSessions(int? page)
        {
            return GetBulkSessions(page, null);
        }

        /// <summary>
        /// Get all bulk sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public BulkSessionsResult GetBulkSessions(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "bulks";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<BulkSessionsResult>(request);
        }

        /// <summary>
        /// Get all sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public SessionsResult GetSessions()
        {
            return GetSessions(null);
        }

        /// <summary>
        /// Get all sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public SessionsResult GetSessions(int? page)
        {
            return GetSessions(page, null);
        }

        /// <summary>
        /// Get all sending sessions.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public SessionsResult GetSessions(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "sessions";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<SessionsResult>(request);
        }
    }
}
