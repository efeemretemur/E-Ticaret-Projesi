namespace E_Ticaret_Projesi.Configuration
{
    public sealed class AdminSeedOptions
    {
        public const string SectionName = "SeedAdmin";

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = "Demo Admin";

        public string Password { get; set; } = string.Empty;

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);
    }
}
