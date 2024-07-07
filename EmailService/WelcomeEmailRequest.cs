namespace RecipeProject.EmailService
{
    public class WelcomeEmailRequest
    {
        public string UserName { get; internal set; }
        public string ToEmail { get; internal set; }
    }
}