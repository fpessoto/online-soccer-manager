using Xunit;

namespace OnlineSoccerManager.Api.Test
{
    public class UsersControllerTest
    {
        [Fact]
        public void Signup_WithValidInformations_ShouldReturn200()
        {
            Assert.Equal(200, 0);
        }


        [Fact]
        public void Signup_WithExistentEmail_ShouldReturn400()
        {
            Assert.Equal(400, 0);
        }


    }
}