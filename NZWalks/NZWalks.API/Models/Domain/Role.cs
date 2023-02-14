namespace NZWalks.API.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        // Navigation property: not used right now
        public List<UserRole> UserRoles { get; set; }

    }
}
