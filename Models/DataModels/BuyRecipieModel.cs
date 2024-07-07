using RecipeBlogProject.Models;

namespace RecipeProject.Models.DataModels
{
    public class BuyRecipieModel
    {
        public Systemuser User { get; set; }
        public Recipe Recipe { get; set; }
        public Visacard Visacard { get; set; }
    }
}
