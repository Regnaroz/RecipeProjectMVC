namespace RecipeBlogProject.Models
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public  string? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;//0

        public int id { get; set; }
        
    }
}
