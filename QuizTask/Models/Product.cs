using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class Product:BaseEntity
    {
        [MinLength(1),MaxLength(50)]
        public string Name { get; set; }
        [Range(0.0,1000.00)]
        public double SellPrice { get; set; }
        [Range(0.0, 1000.00)]
        public double CostPrice { get; set; }
        [Range(0,100)]
        public int Discount { get; set; }
        [MinLength(1),MaxLength(5000)]
        public string Desc { get; set; }

        public string SKU { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<ProductCategory>? ProductCategories { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
       
    }
}
