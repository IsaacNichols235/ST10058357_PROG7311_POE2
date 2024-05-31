using System.ComponentModel.DataAnnotations;

namespace ST10058357_PROG7311_POE2.Models
{
    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
        public ICollection<ProductSubCategory> SubCategories { get; set; } = new List<ProductSubCategory>();

        [Display(Name = "Number of Products")]
        public int ProductCount => Products.Count;

        public ProductCategory()
        {

        }




        public ProductCategory(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
            //TODO get list of products
            //TODO get list of subcategories
        }

        /// <summary>
        /// Method to get the list of products associated with a category
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns> 
        /*
        private List<Product> products(int CategoryID)
        {

        }*/


        /// <summary>
        /// Method to get the list of subcategpries associated with a category
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns> 
        /*
        private List<ProductSubCategory> subCategories(int CategoryID)
        {

        }*/
    }
}
