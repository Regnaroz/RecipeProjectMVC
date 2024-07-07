using RecipeBlogProject.Models;

namespace RecipeProject.Models.DataModels
{
    public class CategoryRecepies
    {
        public Category Category { get; set; }
        public List<Recipe> Recipes { get; set; } = new();
    }
}
