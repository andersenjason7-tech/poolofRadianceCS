using Xunit;
using PoolOfRadiance.Characters;

namespace PoolOfRadianceCS.Tests;

public class CharacterTests
{
    [Fact]
    public void Constructor_SetsInitialValues()
    {
        var c = new Character("Bob", CharacterRace.Human, CharacterClass.Fighter);

        Assert.Equal("Bob", c.Name);
        Assert.Equal(CharacterRace.Human, c.Race);
        Assert.Equal(CharacterClass.Fighter, c.Class);
        Assert.Equal(1, c.Level);
        Assert.Equal(0, c.ExperiencePoints);

        // Default stats are initialized to 10
        Assert.Equal(10, c.Stats.Strength);
        Assert.Equal(10, c.Stats.Dexterity);
        Assert.Equal(10, c.Stats.Constitution);

        // No HP set by default, so character should not be alive
        Assert.False(c.IsAlive);
    }

    [Fact]
    public void TakeDamage_And_Heal_WorksAndClamps()
    {
        var c = new Character("Alice", CharacterRace.Human, CharacterClass.Cleric);
        c.HitPointsMax = 10;
        c.HitPointsCurrent = 10;

        c.TakeDamage(3);
        Assert.Equal(7, c.HitPointsCurrent);

        c.TakeDamage(20);
        Assert.Equal(0, c.HitPointsCurrent);

        c.Heal(5);
        Assert.Equal(5, c.HitPointsCurrent);

        c.Heal(100);
        Assert.Equal(10, c.HitPointsCurrent);
    }

    [Fact]
    public void GainExperience_LevelsUp_WhenThresholdReached()
    {
        var c = new Character("Eve", CharacterRace.Human, CharacterClass.Thief);
        Assert.Equal(1, c.Level);

        // Level 1 -> needs 2000 XP to reach level 2
        c.GainExperience(2000);
        Assert.True(c.Level >= 2);
        Assert.Equal(2000, c.ExperiencePoints);
    }
}
