using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class Category:BaseEntity
    {
        [MinLength(1),MaxLength(30)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
