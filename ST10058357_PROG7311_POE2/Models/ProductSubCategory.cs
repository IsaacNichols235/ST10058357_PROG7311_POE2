using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10058357_PROG7311_POE2.Models
{
    public class ProductSubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Column("CategoryId")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public int? ProductCategoryCategoryId { get; set; }

        public string Name { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ProductSubCategory()
        {
            
        }


        public ProductSubCategory(int subCategoryId, int categoryId, string name)
        {
            SubCategoryId = subCategoryId;
            CategoryId = categoryId;
            Name = name;
        }

      

    }
}
