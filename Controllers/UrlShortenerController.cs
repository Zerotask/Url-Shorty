using Microsoft.AspNetCore.Mvc;
using UrlShorty.Requests;
using UrlShorty.Services;

namespace UrlShorty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IRedisService _redisService;
        private readonly IShortenerService _shortenerService;

        public UrlShortenerController(IRedisService redisService, IShortenerService shortenerService)
        {
            _redisService = redisService;
            _shortenerService = shortenerService;
        }

        /// <summary>
        /// Pass a long URL, store it in Redis and return the short URL.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetShortUrl(UrlShorten data)
        {
            if (string.IsNullOrEmpty(data.Url))
            {
                return BadRequest();
            }

            var shortUrl = _shortenerService.Shorten(data.Url);
            if (_redisService.Set(shortUrl, data.Url))
            {
                return Ok(shortUrl);
            }

            return BadRequest();
        }

        /// <summary>
        /// Pass a short URL, lookup in Redis for an entry and return the long URL.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        [HttpGet("{shortUrl:minlength(1)}")]
        [ResponseCache(Duration = 600)]
        public IActionResult GetLongUrl(string shortUrl)
        {
            var longUrl = _redisService.Get(shortUrl);

            if (string.IsNullOrEmpty(longUrl))
            {
                return NotFound($"No entry found for key {shortUrl}");
            }

            return Ok(longUrl);
        }
    }
}