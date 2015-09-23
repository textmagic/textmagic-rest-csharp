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

namespace TextmagicRest
{
    public partial class Client
    {
        /// <summary>
        /// Get a single template.
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns></returns>
        public Template GetTemplate(int id)
        {
            Require.Argument("id", id);

            var request = new RestRequest();
            request.Resource = "templates/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<Template>(request);
        }

        /// <summary>
        /// Get all user templates.
        /// </summary>
        /// <returns></returns>
        public TemplatesResult GetTemplates()
        {
            return GetTemplates(null);
        }

        /// <summary>
        /// Get all user templates.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <returns></returns>
        public TemplatesResult GetTemplates(int? page)
        {
            return GetTemplates(page, null);
        }

        /// <summary>
        /// Get all user templates.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <returns></returns>
        public TemplatesResult GetTemplates(int? page, int? limit)
        {
            var request = new RestRequest();
            request.Resource = "templates";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());

            return Execute<TemplatesResult>(request);
        }

        /// <summary>
        /// Find user templates by given parameters.
        /// </summary>
        /// <param name="page">Fetch specified results page</param>
        /// <param name="limit">How many results to return</param>
        /// <param name="ids">Find template by ID</param>
        /// <param name="name">Find template by name</param>
        /// <param name="content">Find template by content</param>
        /// <returns></returns>
        public TemplatesResult SearchTemplates(int? page, int? limit, int[] ids, string name, string content)
        {
            var request = new RestRequest();
            request.Resource = "templates/search";
            if (page.HasValue) request.AddQueryParameter("page", page.ToString());
            if (limit.HasValue) request.AddQueryParameter("limit", limit.ToString());
            if (ids != null && ids.Length > 0) request.AddQueryParameter("ids", string.Join(",", ids));
            request.AddQueryParameter("name", name);
            request.AddQueryParameter("content", content);

            return Execute<TemplatesResult>(request);
        }

        /// <summary>
        /// Delete a single template.
        /// </summary>
        /// <param name="id">Template ID</param>
        /// <returns></returns>
        public DeleteResult DeleteTemplate(int id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "templates/{id}";
            request.AddUrlSegment("id", id.ToString());

            return Execute<DeleteResult>(request);
        }

        /// <summary>
        /// Delete a single template.
        /// </summary>
        /// <param name="template">Template object</param>
        /// <returns></returns>
        public DeleteResult DeleteTemplate(Template template)
        {
            return DeleteTemplate(template.Id);
        }

        /// <summary>
        /// Create a new template from the submitted data.
        /// </summary>
        /// <param name="name">Template name</param>
        /// <param name="content">Template text. May contain tags inside braces</param>
        /// <returns></returns>
        public LinkResult CreateTemplate(string name, string content)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "templates";
            request.AddParameter("name", name);
            request.AddParameter("body", content);

            return Execute<LinkResult>(request);
        }

        /// <summary>
        /// Update existing template.
        /// </summary>
        /// <param name="template">Template object</param>
        /// <returns></returns>
        public LinkResult UpdateTemplate(Template template)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "templates/{id}";
            request.AddUrlSegment("id", template.Id.ToString());
            request.AddParameter("name", template.Name);
            request.AddParameter("body", template.Content);

            return Execute<LinkResult>(request);
        }
    }
}
