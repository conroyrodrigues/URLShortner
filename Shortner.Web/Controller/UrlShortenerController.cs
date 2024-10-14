using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortner.Web.Context;
using Shortner.Web.Context.Models;
using Shortner.Web.Controller.Transport;

namespace Shortner.Web.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly DataStoreContext _context;
        private readonly ILogger<UrlShortenerController> _logger;

        public UrlShortenerController(DataStoreContext context, ILogger<UrlShortenerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseModel>>> Get()
        {
            var urls = await _context.UrlShorteners.ToListAsync();

            _logger.LogInformation($"Total Number of records retrived : {urls.Count}");

            return Ok(urls.Select(a => new ResponseModel().ToTransport(a)));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Post([FromBody] RequestModel request)
        {
            if (string.IsNullOrEmpty(request.OriginalUrl) || !Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
            {
                _logger.LogError($"{request.OriginalUrl} is not a valid url format");

                return BadRequest("Invalid URL.");
            }

            _logger.LogInformation($"Generating Short Url for {request.OriginalUrl}");

            try
            {
                var urlShortner = new UrlShortener
                {
                    ShortUrl = $"https://{Guid.NewGuid().ToString().Substring(0, 8)}",
                    OriginalUrl = request.OriginalUrl,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                };

                _context.UrlShorteners.Add(urlShortner);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Short Url for {request.OriginalUrl} Generated: {urlShortner}");

                return Ok(new ResponseModel().ToTransport(urlShortner));

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error updating database : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed generating short url for {request.OriginalUrl}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating database : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again.");
            }
        }
    }
}
