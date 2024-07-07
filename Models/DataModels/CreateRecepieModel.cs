using RecipeBlogProject.Models;

namespace RecipeProject.Models.DataModels
{
    public class CreateRecepieModel
    {
        public Recipe Recipe { get; set; }
        public List<Category> CategoryList { get; set; } = new List<Category>();
        public int SelectedCategoryId { get; set; }
    }
}
