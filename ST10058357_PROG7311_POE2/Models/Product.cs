using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10058357_PROG7311_POE2.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Column("UserId")]
        [ForeignKey("UserId")]
        public int FarmerId { get; set; }


        [Column("CategoryId")]
        [ForeignKey("CategoryId")]
        [Display(Name="Product Category")]
        public int CategoryId { get; set; }

        [Column("SubCategoryId")]
        [ForeignKey("SubCategoryId")]
        [Display(Name = "Product Sub Category")]
        public int SubCategoryId { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateOnly ProductionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price (Rands)")]
        public decimal Price { get; set; }

        
        [Display(Name = "Product Image")]
        public string? ImagePath { get; set; } 



        public Product()
        {

        }

        public Product(int productId, int farmerId, int categoryId,
            int subCategoryId, string name, string description,
            DateOnly productionDate, decimal price, string imagePath)
        {
            ProductId = productId;
            FarmerId = farmerId;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            Name = name;
            Description = description;
            ProductionDate = productionDate;
            Price = price;
            ImagePath = imagePath;
        }
    }
}
