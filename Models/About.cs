using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baeun_Project.Models
{
    public class About: Base
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Office { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
