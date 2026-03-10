using System;
using System.IO;
using Xunit;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Items;
using PoolOfRadiance.UI.Screens;
using PoolOfRadiance.UI;

namespace PoolOfRadianceCS.Tests
{
    public class SupportScreensTests
    {
        [Fact]
        public void PartyStatusScreen_ShowsCharacterInventories()
        {
            var party = new Party();
            var character = new Character("Test", CharacterRace.Human, CharacterClass.Fighter);
            character.Inventory.AddGold(42);
            character.Inventory.AddItem(new Item("Staff", ItemType.Weapon));
            party.AddMember(character);

            var screen = new PartyStatusScreen(new ScreenManager(), party);

            var sw = new StringWriter();
            var original = Console.Out;
            try
            {
                Console.SetOut(sw);
                screen.Render();
            }
            finally
            {
                Console.SetOut(original);
            }

            var output = sw.ToString();
            Assert.Contains("Inventory", output);
            Assert.Contains("Gold: 42", output);
            Assert.Contains("Staff", output);
        }
    }
}
