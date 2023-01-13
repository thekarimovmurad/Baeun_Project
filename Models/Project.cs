using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baeun_Project.Models
{
    public class Project : Base
    {
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Year { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Adress { get; set; }
        public string Image { get; set; }
        public List<ProjectImage> ProjectImages { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile[] ImageFiles { get; set; }
    }
}
