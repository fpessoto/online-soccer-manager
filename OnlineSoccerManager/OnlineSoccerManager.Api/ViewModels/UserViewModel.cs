using OnlineSoccerManager.Domain.Users;

namespace OnlineSoccerManager.Api.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Role { get; set; }
    }

    public static class Map
    {
        public static UserViewModel ToViewModel(this User user)
        {
            return new UserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                Role = user.Role,
            };
        }
    }
}
