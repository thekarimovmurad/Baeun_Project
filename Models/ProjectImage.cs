namespace Baeun_Project.Models
{
    public class ProjectImage: Base
    {
        public string Image { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
