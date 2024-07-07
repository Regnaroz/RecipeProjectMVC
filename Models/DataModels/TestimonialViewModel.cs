using System.ComponentModel.DataAnnotations;

namespace RecipeProject.Models.DataModels
{
    public class TestimonialViewModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5 stars.")]
        public int Rating { get; set; }
    }
}
