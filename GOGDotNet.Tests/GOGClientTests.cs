using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GOGDotNet.Tests
{
    [TestClass]
    public class GOGClientTests
    {
        private GOGClient client;

        [TestInitialize]
        public void TestInitialize()
        {
            this.client = new GOGClient();
        }

        [TestMethod]
        public async Task GetGameStats()
        {
            const string userID = "MChartier";
            var response = await this.client.GetGamesStatsAsync(userID);
            Assert.IsNotNull(response);
        }
    }
}
