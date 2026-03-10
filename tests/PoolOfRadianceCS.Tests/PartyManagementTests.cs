using System;
using System.Linq;
using Xunit;
using PoolOfRadiance;
using PoolOfRadiance.Characters;

namespace PoolOfRadianceCS.Tests
{
    public class PartyManagementTests
    {
        [Fact]
        public void CreateDefaultParty_AssignsRandomInventoryGoldToEachCharacter()
        {
            var manager = new PartyManagement();
            manager.CreateDefaultParty();

            var members = manager.Party.Members;
            Assert.NotEmpty(members);

            foreach (var member in members)
            {
                int gold = member.Inventory.Gold;
                Assert.InRange(gold, 50, 150);
            }
        }
    }
}
