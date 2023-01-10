using System.ComponentModel.DataAnnotations;

namespace QuizTask.ViewModels
{
    public class UpdateProductVM
    {
        [MinLength(1), MaxLength(50)]
        public string Name { get; set; }
        [Range(0.0, 1000.00)]
        public double SellPrice { get; set; }
        [Range(0.0, 1000.00)]
        public double CostPrice { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        [MinLength(1), MaxLength(5000)]
        public string Desc { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
