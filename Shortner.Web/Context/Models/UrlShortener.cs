using System.ComponentModel.DataAnnotations;

namespace Shortner.Web.Context.Models
{
    public class UrlShortener
    {
        [Key]
        public int UrlShortenerId { get; set; }

        [Required]
        [Url]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortUrl { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}
