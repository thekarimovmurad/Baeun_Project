using Baeun_Project.Models;
using System.Collections.Generic;

namespace Baeun_Project.ViewModels
{
    public class HomeViewModel
    {
        public List<Header> Headers { get; set; }
        public About Abouts { get; set; }
        public List<Project> Projects { get; set; }
    }
}
