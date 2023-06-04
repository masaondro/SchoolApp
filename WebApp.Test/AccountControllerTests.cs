using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Test
{
    [TestFixture]
    public class AccountControllerTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task Get_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();

            // Act
            var response = await _client.GetAsync("/api/account");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenAccountIsNull()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer token");

            // Act
            var response = await _client.GetAsync("/api/account");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Get_ReturnsAccount_WhenAccountIsNotNull()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer token");

            // Act
            var response = await _client.GetAsync("/api/account/1");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task UpdateAccount_ReturnsOk_WhenAccountIsNotNull()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer token");

            // Act
            var response = await _client.PostAsync("/api/account/counter", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task UpdateAccount_ReturnsNotFound_WhenAccountIsNull()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer token");

            // Act
            var response = await _client.PostAsync("/api/account/counter", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
