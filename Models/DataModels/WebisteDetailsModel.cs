using RecipeBlogProject.Models;

namespace RecipeProject.Models.DataModels
{
    public class WebisteDetailsModel
    {
        public string HomePageHeader {  get; set; }
        public string HomePageFooter {  get; set; }
        public string HomePageContactUs {  get; set; }
        public List<Testimonial> Testimonials {  get; set; }
    }
}
