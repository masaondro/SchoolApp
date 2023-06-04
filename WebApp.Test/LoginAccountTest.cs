using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Test
{
    [TestFixture]
    public class LoginAccountTest
    {
        private Mock<IAccountDatabase> _mockDb;
        private LoginController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDb = new Mock<IAccountDatabase>();
            _controller = new LoginController(_mockDb.Object);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            string userName = "alice@mailinator.com";
            Account account = new Account
            {
                ExternalId = "alice",
                UserName = "alice@mailinator.com",
                Role = "Admin",
                InternalId = 1
            };
            _mockDb.Setup(x => x.FindByUserNameAsync(userName)).ReturnsAsync(account);

            // Act
            var result = await _controller.Login(userName);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsNotFound()
        {
            // Arrange
            string userName = "unknownuser";
            _mockDb.Setup(x => x.FindByUserNameAsync(userName)).ReturnsAsync((Account)null);

            // Act
            var result = await _controller.Login(userName);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
