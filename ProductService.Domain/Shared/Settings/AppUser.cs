namespace ProductService.Domain.Shared.Settings
{
    public class AppUser
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
