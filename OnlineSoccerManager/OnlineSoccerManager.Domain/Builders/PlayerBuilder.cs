using OnlineSoccerManager.Domain.Enums;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;

namespace OnlineSoccerManager.Domain.Builders
{
    public class PlayerBuilder : IPlayerBuilder
    {
        public readonly string[] FIRST_NAMES = new string[] { "Felipe", "Raul", "Cristiano", "James", "John", "Alex", "Ronaldo", "Jim" };
        public readonly string[] LAST_NAMES = new string[] { "Silva", "Santos" };

        public Player Build(PlayerPosition position, Team team)
        {
            var random = new Random();
            var lastCountry = (int)Enum.GetValues(typeof(Country)).Cast<Country>().Last();
            int randomCountry = random.Next(0, lastCountry);

            return new Player
            {
                FirstName = GetRandomFirstName(),
                LastName = GetRandomLastName(),
                CreatedDate = DateTime.Now,
                CurrentValue = 1000000,
                Position = position,
                Age = random.Next(18, 40),
                Country = (Country)randomCountry,
                Team = team,
                Id = Guid.NewGuid(),
                UpdatedDate = DateTime.Now,
            };
        }

        private string GetRandomFirstName()
        {
            var random = new Random();
            return FIRST_NAMES[random.Next(0, FIRST_NAMES.Length - 1)];
        }

        private string GetRandomLastName()
        {
            var random = new Random();
            return LAST_NAMES[random.Next(0, LAST_NAMES.Length - 1)];
        }
    }
}
