namespace Restaurants.Application.User
{
    public record CurrentUser(string Id, 
        string Email, 
        IEnumerable<string> Roles,
        string? Nationality,
        DateOnly? DateOfBirth)
    {
        /// <summary>
        /// Checks if the user has a specific role
        /// </summary>
        /// <param name="role">The role to check for</param>
        /// <returns>True if the user has the specified role, false otherwise</returns>
        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
