using OnlineSoccerManager.Domain.Base;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Domain.Users
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //public Team? Team { get; set; }

        public void Create(string email, string password, string username, string role)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            Username = username;
            Role = role;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        //public void AssignTeam(Team team)
        //{
        //    Team = team;
        //}
    }
}
