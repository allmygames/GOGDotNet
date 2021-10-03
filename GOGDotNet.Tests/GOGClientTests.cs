using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
            Assert.AreEqual(response.Item1, Models.ProfileState.Verified);

            var games = response.Item2.ToList();
            var discoElysium = games.FirstOrDefault(g => g.Id == 1771589310);

            Assert.IsNotNull(discoElysium);
            Assert.IsTrue(discoElysium.AchievementSupport);
            Assert.IsTrue(discoElysium.Playtime > 0);
            Assert.IsTrue(discoElysium.AchievementsPercentage > 0);
            Assert.IsTrue(discoElysium.LastSession > DateTime.MinValue);

            var sevenBillionHumans = games.FirstOrDefault(g => g.Id == 2056114425);
            Assert.IsNotNull(sevenBillionHumans);
            Assert.IsFalse(sevenBillionHumans.AchievementSupport);
            Assert.IsNull(sevenBillionHumans.AchievementsPercentage);
        }
    }
}
