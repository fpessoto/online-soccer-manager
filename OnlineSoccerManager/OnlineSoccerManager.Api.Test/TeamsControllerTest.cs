using Xunit;

namespace OnlineSoccerManager.Api.Test
{
    public class TeamsControllerTest
    {
        [Fact]
        public void Get_WhenLoggedIn_ShouldReturnTeamAndPlayerInformations()
        {
            Assert.Equal(200, 0);
        }

        [Fact]
        public void Get_WhenNotLoggedIn_ShouldReturn401()
        {
            var a = new int[] { };
            var b = new int[] { 3, 2, 0, 4, 4 };

            var res = FilterBy(a, b);

            res.Should();

        }

        public static int[] FilterBy(int[] a, int[] b)
        {

        }

    }
}