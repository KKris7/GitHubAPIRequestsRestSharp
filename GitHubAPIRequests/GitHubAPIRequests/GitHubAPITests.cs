using GitHubAPIRequests;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace GitHubAPIRequests
{
    public class GitHubAPITests
    {
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            client = new RestClient("https://api.github.com");
            request = new RestRequest("/repos/USE YOUR USERNAMEUSE/YOUR REPOSITORY/issues");
            client.Authenticator = new HttpBasicAuthenticator("USE YOUR USERNAME", "USE YOUR TOKEN HERE");

        }

        private async Task<Issue> CreateIssue(string title, string body)
        {
            request.AddBody(new { title, body });
            var response = await client.ExecuteAsync(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;

        }
        private async Task<Issue> EditIssue(string title)
        {

            request.AddBody(new { title });

            var response = await client.ExecuteAsync(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }

        [Test]
        public async Task APIRequestStatusCode()
        {
            var response = await client.ExecuteAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task APIRequestALLIssuesStatusCode()
        {
            var response = await client.ExecuteAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            Assert.That(issues.Count >= 1);

            foreach (var issue in issues)
            {
                Assert.Greater(issue.id,0);
                Assert.Greater(issue.number,0);
                Assert.IsNotEmpty(issue.title);
            }
        }

        [Test]
        public async Task APIRequestToNotExistingIssue()
        {
            var request = new RestRequest("/repos/USE YOUR USERNAMEUSE/YOUR REPOSITORY/issues/77");
            var response = await client.ExecuteAsync(request); 
           Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetIssueByNumber()
        {
            var request = new RestRequest("/repos/USE YOUR USERNAMEUSE/YOUR REPOSITORY/issues/4");
            var response = await client.ExecuteAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var item = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.That(item.id >= 1);
            Assert.AreEqual(item.number,4);
        }


        [Test]
        public async Task TestCreateIssue()
        {
            var title = "New test Issue from RestSharp";
            var body = "Some test body";
            var issue = await CreateIssue(title, body);

            Assert.That(issue.id > 0);
            Assert.That(issue.number > 0);
            Assert.IsNotEmpty(issue.title);
        }

        [Test]
        public async Task Try_CreateIssueWithoutAuthorization()
        {
            var title = "New test Issue from RestSharp";
            var body = "Some test body in the new issue";
            client.Authenticator = new HttpBasicAuthenticator("user", "no token");

            var issue = await CreateIssue(title, body);
            var response = await client.ExecuteAsync(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Try_CreateIssueWithoutTitle()
        {
            var body = "Some test body in the new issue";
            var issue = await CreateIssue("", body);
            var response = await client.ExecuteAsync(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task TestEditIssue()
        {
            string newTitle = "New Edited from RestSharp";
            request = new RestRequest("/repos/USE YOUR USERNAMEUSE/YOUR REPOSITORY/issues/12");
            var issue = await EditIssue(newTitle);
            Assert.That(issue.id > 0);
            Assert.That(issue.number > 0);
            Assert.AreEqual(newTitle, issue.title);
        }

    }
    public class ZipopotamostAPITests
    {

        [TestCase("BG", "1000", "Sofija")]
        [TestCase("ES", "21002", "Huelva")]
        [TestCase("IT", "98167", "Pace")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("DE", "01067", "Dresden")]
        [TestCase("GB", "B1", "Birmingham")]
        [TestCase("US", "99501", "Anchorage")]

        public async Task TestZipopotamost(string countryCode , string zipCode, string expectedLocation)
        {
            //arrange
            var client = new RestClient("https://api.zippopotam.us/");
            var request = new RestRequest(countryCode + "/" + zipCode);
            //act
            var response = await client.ExecuteAsync(request, Method.Get);
            var location = new SystemTextJsonSerializer().Deserialize<Location>(response);
            //assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual(countryCode, location.countryAbbreviation);
            Assert.AreEqual(zipCode, location.postCode);
            StringAssert.Contains(expectedLocation, location.Places[0].PlaceName);

        }
    }
    
}
