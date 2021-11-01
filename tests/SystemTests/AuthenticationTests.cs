using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using SimpleWebAPI;

namespace SystemTests
{
    public class AuthenticationTests
    {
        private TestServer _server;
        private HttpClient _client;
        
        [SetUp]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<StartUp>());
            _client = _server.CreateClient();
        }

        [Test]
        public async Task CheckValidCredentialsSucceed()
        {
            // Arrange...
            const string username = "johnmorsley";
            const string password = "length-over-complexity";
            var credentials = $"{username}:{password}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            var encodedCredentials = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedCredentials}");
            
            // Act...
            var httpResponse = await _client.GetAsync("weatherforecast");
            
            // Assert...
            httpResponse.IsSuccessStatusCode.Should().BeTrue();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var response = await httpResponse.Content.ReadAsStringAsync();
            //response.Length.Should().Be(0);
        }
        
        [Test]
        public async Task CheckInvalidCredentialsFail()
        {
            // Arrange...
            const string username = "joebloggs";
            const string password = "foo-bar";
            var credentials = $"{username}:{password}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            var encodedCredentials = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedCredentials}");
            
            // Act...
            var httpResponse = await _client.GetAsync("weatherforecast");
            
            // Assert...
            httpResponse.IsSuccessStatusCode.Should().BeFalse();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var response = await httpResponse.Content.ReadAsStringAsync();
            //response.Length.Should().Be(0);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}