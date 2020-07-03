namespace AccionaCovid.Identity.Infrastructure
{
    public class AdminUserSettings
    {
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public bool LockoutEnabled { get; set; } = true;
        public string UserName { get; set; } = "AdminCovAcc";
        public string Password { get; set; }
    }
}
