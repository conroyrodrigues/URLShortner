using Shortner.Web.Context.Models;

namespace Shortner.Web.Controller.Transport
{
    public class ResponseModel
    {
        public int UrlShortenerId { get; set; }
        public string OriginalUrl { get; set; }

        public string ShortUrl { get; set; }


        /// <summary>
        /// Used to reduce the sice of the Payload to be sent as the response
        /// </summary>
        /// <param name="urlShortener"></param>
        /// <returns></returns>
        public ResponseModel ToTransport(UrlShortener urlShortener)
        {
            return new ResponseModel
            {
                UrlShortenerId = urlShortener.UrlShortenerId,
                OriginalUrl = urlShortener.OriginalUrl,
                ShortUrl = urlShortener.ShortUrl,
            };
        }
    }
}
