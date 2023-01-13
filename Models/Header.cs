using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baeun_Project.Models
{
    public class Header:Base
    {
        [Required]
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string ButtonText { get; set; }
        [Required]
        public string ButtonUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
