using Xunit;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Items;

namespace PoolOfRadianceCS.Tests
{
    public class CharacterSupportTests
    {
        [Fact]
        public void Inventory_AddRemoveItemsAndGold_WorksCorrectly()
        {
            var inv = new Inventory();
            Assert.Empty(inv.Items);
            Assert.Equal(0, inv.Gold);

            // add gold
            inv.AddGold(50);
            Assert.Equal(50, inv.Gold);
            Assert.True(inv.RemoveGold(20));
            Assert.Equal(30, inv.Gold);
            Assert.False(inv.RemoveGold(100));
            Assert.Equal(30, inv.Gold);

            // add items until full
            for (int i = 0; i < Inventory.MAX_ITEMS; i++)
            {
                var item = new Item($"Item{i}", ItemType.Misc);
                Assert.True(inv.AddItem(item));
            }

            // further adds should fail
            Assert.False(inv.AddItem(new Item("Overflow", ItemType.Misc)));
            Assert.Equal(Inventory.MAX_ITEMS, inv.Items.Count);

            // remove an item
            var removed = inv.Items[0];
            Assert.True(inv.RemoveItem(removed));
            Assert.Equal(Inventory.MAX_ITEMS - 1, inv.Items.Count);
        }

        [Fact]
        public void Equipment_ACBonus_ComputesBasedOnArmorAndShield()
        {
            var eq = new Equipment();
            Assert.Equal(0, eq.GetArmorClassBonus());

            eq.Armor = new Armor("Chainmail") { ArmorClassBonus = 5 };
            Assert.Equal(5, eq.GetArmorClassBonus());

            eq.Shield = new Item("Wooden Shield", ItemType.Shield);
            Assert.Equal(6, eq.GetArmorClassBonus()); // +1 for shield

            // removing armor resets bonus
            eq.Armor = null;
            Assert.Equal(1, eq.GetArmorClassBonus());
        }

        [Fact]
        public void CharacterStatus_HasAnyEffect_DetectsStates()
        {
            var status = new CharacterStatus();
            Assert.False(status.HasAnyEffect());

            status.IsPoisoned = true;
            Assert.True(status.HasAnyEffect());

            status.IsPoisoned = false;
            status.IsSleeping = true;
            Assert.True(status.HasAnyEffect());
        }

        [Fact]
        public void Spell_Constructor_InitializesFields()
        {
            var spell = new Spell("Fireball", 3, SpellSchool.Evocation);
            Assert.Equal("Fireball", spell.Name);
            Assert.Equal(3, spell.Level);
            Assert.Equal(SpellSchool.Evocation, spell.School);
            Assert.Equal("default", spell.DamageOrEffect);
            Assert.Equal("default", spell.Description);
        }
    }
}
