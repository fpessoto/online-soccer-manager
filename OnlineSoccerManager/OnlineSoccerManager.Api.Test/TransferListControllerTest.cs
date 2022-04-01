using Xunit;

namespace OnlineSoccerManager.Api.Test
{
    public class TransferListControllerTest
    {
        [Fact]
        public void Get_WhenLoggedIn_ShouldReturn200WithAllPlayersInTransferMarketList()
        {
            Assert.Equal(200, 0);
        }

        [Fact]
        public void Get_WhenNotLoggedIn_ShouldReturn401()
        {
            Assert.Equal(200, 0);
        }

    }
}