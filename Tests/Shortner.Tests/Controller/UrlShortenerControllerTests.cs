using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shortner.Web.Context;
using Shortner.Web.Controller;
using Shortner.Web.Controller.Transport;
using System.Diagnostics;

namespace Shortner.Tests.Controller
{
    public class UrlShortenerControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DataStoreContext _context;
        private readonly Mock<ILogger<UrlShortenerController>> _mockLogger;
        private readonly UrlShortenerController _controller;
        public UrlShortenerControllerTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _mockLogger = new Mock<ILogger<UrlShortenerController>>();
            // Initialize the controller
            _controller = new UrlShortenerController(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsListOfShortenedUrls()
        {
            //Arrange
            var expectedResult = 2;

            // Act
            var result = await _controller.Get();
            var returnedValue = (result.Result as OkObjectResult).Value as IEnumerable<ResponseModel>;
            _context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedResult, returnedValue.Count());  // We seeded two records
        }

        [Fact]
        public async Task Post_InvalidUrl_ReturnsBadRequest()
        {
            //Arrange
            var expectedReult = "Invalid URL.";

            // Act
            var result = await _controller.Post(new RequestModel { OriginalUrl = "invalid-url" });
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            _context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedReult, badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ValidUrl_ReturnsOk()
        {
            // Arrange
            var validUrl = "https://validurl.com";

            // Act
            var result = await _controller.Post(new RequestModel { OriginalUrl = validUrl });
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseModel = (result.Result as OkObjectResult).Value as ResponseModel;
            _context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(validUrl, responseModel.OriginalUrl);
        }

        [Fact]
        public async Task Post_DbUpdateException_ReturnsInternalServerError()
        {
            // Arrange
            var validUrl = "https://example.com";

            // Act
            var result = await _controller.Post(new RequestModel { OriginalUrl = validUrl });
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            _context.ChangeTracker.Clear();

            // Assert
            Assert.Contains($"Failed generating short url for {validUrl}", statusCodeResult.Value.ToString());
        }
    }
}
