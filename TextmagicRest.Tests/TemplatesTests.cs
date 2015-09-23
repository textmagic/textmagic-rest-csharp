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
    public class TemplatesTests
    {
        private Mock<Client> mockClient;

        private const int templateId = 51335;
        private const string templateName = "Template name";
        private const string templateContent = "Template content";
        private DateTime date = new DateTime(2015, 05, 07, 06, 05, 55, 0, DateTimeKind.Utc);

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<Client>(Common.Username, Common.Token);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetSingleTemplate()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Template>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new Template());
            var client = mockClient.Object;

            client.GetTemplate(templateId);

            mockClient.Verify(trc => trc.Execute<Template>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);

            var content = "{ \"id\": \"51335\", \"name\": \"Template name\", \"content\": \"Template content\", \"lastModified\": \"2015-05-07T06:05:55+0000\" }";

            var testClient = Common.CreateClient<Template>(content, null, null);
            client = new Client(testClient);

            var template = client.GetTemplate(templateId);

            Assert.IsTrue(template.Success);
            Assert.AreEqual(templateId, template.Id);
            Assert.AreEqual(templateName, template.Name);
            Assert.AreEqual(templateContent, template.Content);
            Assert.AreEqual(date.ToLocalTime(), template.LastModified);
        }        

        [Test]
        public void ShouldGetAllTemplates()
        {
            var page = 2;
            var limit = 3;
            
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TemplatesResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new TemplatesResult());
            var client = mockClient.Object;

            client.GetTemplates(page, limit);

            mockClient.Verify(trc => trc.Execute<TemplatesResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 5, \"resources\": ["
                + "{ \"id\": \"51335\", \"name\": \"API TEST 0\", \"content\": \"API TEST\", \"lastModified\": \"2015-05-07T06:05:55+0000\" }"
                + "{ \"id\": \"51336\", \"name\": \"API TEST 1\", \"content\": \"API TEST\", \"lastModified\": \"2015-05-07T06:05:55+0000\" }"
                + "{ \"id\": \"51337\", \"name\": \"API TEST 2\", \"content\": \"API TEST\", \"lastModified\": \"2015-05-07T06:05:55+0000\" }"
                + "] }";

            var testClient = Common.CreateClient<TemplatesResult>(content, null, null);
            client = new Client(testClient);

            var templates = client.GetTemplates(page, limit);

            Assert.IsTrue(templates.Success);
            Assert.AreEqual(3, templates.Templates.Count);
            Assert.AreEqual(page, templates.Page);
            Assert.AreEqual(limit, templates.Limit);
            Assert.AreEqual(5, templates.PageCount);

            var firstId = 51335;
            var currentIteration = 0;
            foreach (var template in templates.Templates)
            {
                Assert.AreEqual(firstId + currentIteration, template.Id);
                Assert.AreEqual("API TEST " + currentIteration, template.Name);
                Assert.AreEqual("API TEST", template.Content);
                Assert.AreEqual(date.ToLocalTime(), template.LastModified);
                currentIteration++;
            }
        }

        [Test]
        public void ShouldUpdateTemplate()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            var template = new Template()
            {
                Id = templateId,
                Name = templateName,
                Content = templateContent,
            };
            client.UpdateTemplate(template);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.PUT, savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            Assert.AreEqual(templateId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);
            Assert.AreEqual(templateName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual(templateContent, savedRequest.Parameters.Find(x => x.Name == "body").Value);

            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.UpdateTemplate(template);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
        }

        [Test]
        public void ShouldCreateTemplate()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new LinkResult());
            var client = mockClient.Object;

            client.CreateTemplate(templateName, templateContent);

            mockClient.Verify(trc => trc.Execute<LinkResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates", savedRequest.Resource);
            Assert.AreEqual(Method.POST, savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            Assert.AreEqual(templateName, savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual(templateContent, savedRequest.Parameters.Find(x => x.Name == "body").Value);
            
            var content = "{ \"id\": \"31337\", \"href\": \"/api/v2/contacts/31337\"}";

            var testClient = Common.CreateClient<LinkResult>(content, null, null);
            client = new Client(testClient);

            var link = client.CreateTemplate(templateName, templateContent);

            Assert.IsTrue(link.Success);
            Assert.AreEqual(31337, link.Id);
            Assert.AreEqual("/api/v2/contacts/31337", link.Href);
        }

        [Test]
        public void ShouldDeleteTemplate()
        {
            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new DeleteResult());
            var client = mockClient.Object;

            var template = new Template()
            {
                Id = templateId,
                Name = templateName,
                Content = templateContent,
            };

            client.DeleteTemplate(template);

            mockClient.Verify(trc => trc.Execute<DeleteResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates/{id}", savedRequest.Resource);
            Assert.AreEqual(Method.DELETE, savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            Assert.AreEqual(templateId.ToString(), savedRequest.Parameters.Find(x => x.Name == "id").Value);

            var content = "{}";

            var testClient = Common.CreateClient<DeleteResult>(content, null, null);
            client = new Client(testClient);

            var result = client.DeleteTemplate(template);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldSearchTemplates()
        {
            var page = 2;
            var limit = 3;
            int[] ids = {46};
            string name = "my template";
            string templateContent = "my template content";

            IRestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TemplatesResult>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>((request) => savedRequest = request)
                .Returns(new TemplatesResult());
            var client = mockClient.Object;

            client.SearchTemplates(page, limit, ids, name, templateContent);

            mockClient.Verify(trc => trc.Execute<TemplatesResult>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("templates/search", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            Assert.AreEqual(page.ToString(), savedRequest.Parameters.Find(x => x.Name == "page").Value);
            Assert.AreEqual(limit.ToString(), savedRequest.Parameters.Find(x => x.Name == "limit").Value);
            Assert.AreEqual(name.ToString(), savedRequest.Parameters.Find(x => x.Name == "name").Value);
            Assert.AreEqual(templateContent.ToString(), savedRequest.Parameters.Find(x => x.Name == "content").Value);

            var content = "{ \"page\": 2,  \"limit\": 3, \"pageCount\": 1, \"resources\": ["
                + "{ \"name\": \"my template\", \"id\": \"46\",  \"content\": \"my template content\", \"lastModified\": \"2015-05-07T06:05:55+0000\" }"
                + "] }";

            var testClient = Common.CreateClient<TemplatesResult>(content, null, null);
            client = new Client(testClient);

            var templates = client.SearchTemplates(page, limit, ids, name, templateContent);

            Assert.IsTrue(templates.Success);
            Assert.AreEqual(1, templates.Templates.Count);
            Assert.AreEqual(page, templates.Page);
            Assert.AreEqual(limit, templates.Limit);
            Assert.AreEqual(1, templates.PageCount);

            Assert.AreEqual(ids[0], templates.Templates[0].Id);
            Assert.AreEqual(name, templates.Templates[0].Name);
            Assert.AreEqual(templateContent, templates.Templates[0].Content);
            Assert.AreEqual(date.ToLocalTime(), templates.Templates[0].LastModified);
        }
    }
}
