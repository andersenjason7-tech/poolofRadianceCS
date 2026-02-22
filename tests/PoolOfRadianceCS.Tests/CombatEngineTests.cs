using Xunit;
using PoolOfRadiance.Combat;
using PoolOfRadiance.Characters;

namespace PoolOfRadianceCS.Tests;

public class CombatEngineTests
{
    [Fact]
    public void Constructor_InitializesCombatEngine()
    {
        var engine = new CombatEngine();

        Assert.False(engine.IsActive);
        Assert.NotNull(engine.GetPlayerCombatants());
        Assert.NotNull(engine.GetEnemies());
        Assert.Empty(engine.GetPlayerCombatants());
        Assert.Empty(engine.GetEnemies());
    }

    [Fact]
    public void StartCombat_AddsPlayerAndEnemyCombatants()
    {
        var engine = new CombatEngine();
        
        var player = new Character("Hero", CharacterRace.Human, CharacterClass.Fighter);
        player.HitPointsMax = 20;
        player.HitPointsCurrent = 20;
        
        var party = new Party();
        party.AddMember(player);
        
        var enemy = new Character("Goblin", CharacterRace.Human, CharacterClass.Thief);
        enemy.HitPointsMax = 8;
        enemy.HitPointsCurrent = 8;

        engine.StartCombat(party, new List<Character> { enemy });

        Assert.True(engine.IsActive);
        Assert.Single(engine.GetPlayerCombatants());
        Assert.Single(engine.GetEnemies());
    }

    [Fact]
    public void GetPlayerCombatants_OnlyIncludesAlivePlayers()
    {
        var engine = new CombatEngine();
        
        var player1 = new Character("Hero1", CharacterRace.Human, CharacterClass.Fighter);
        player1.HitPointsMax = 20;
        player1.HitPointsCurrent = 20;
        
        var player2 = new Character("Hero2", CharacterRace.Human, CharacterClass.Cleric);
        player2.HitPointsMax = 15;
        player2.HitPointsCurrent = 0; // Dead
        
        var party = new Party();
        party.AddMember(player1);
        party.AddMember(player2);
        
        var enemy = new Character("Goblin", CharacterRace.Human, CharacterClass.Thief);
        enemy.HitPointsMax = 8;
        enemy.HitPointsCurrent = 8;

        engine.StartCombat(party, new List<Character> { enemy });

        Assert.Single(engine.GetPlayerCombatants());
        Assert.Equal("Hero1", engine.GetPlayerCombatants()[0].Character.Name);
    }

    [Fact]
    public void GetEnemies_ReturnsOnlyAliveEnemies()
    {
        var engine = new CombatEngine();
        
        var player = new Character("Hero", CharacterRace.Human, CharacterClass.Fighter);
        player.HitPointsMax = 20;
        player.HitPointsCurrent = 20;
        
        var party = new Party();
        party.AddMember(player);
        
        var enemy1 = new Character("Goblin1", CharacterRace.Human, CharacterClass.Thief);
        enemy1.HitPointsMax = 8;
        enemy1.HitPointsCurrent = 8;
        
        var enemy2 = new Character("Goblin2", CharacterRace.Human, CharacterClass.Thief);
        enemy2.HitPointsMax = 8;
        enemy2.HitPointsCurrent = 0; // Dead

        engine.StartCombat(party, new List<Character> { enemy1, enemy2 });

        Assert.Single(engine.GetEnemies());
        Assert.Equal("Goblin1", engine.GetEnemies()[0].Character.Name);
    }
}

public class CombatantTests
{
    [Fact]
    public void Constructor_SetsCharacterAndPlayerControlledFlag()
    {
        var character = new Character("Test", CharacterRace.Human, CharacterClass.Fighter);
        var combatant = new Combatant(character, true);

        Assert.Equal(character, combatant.Character);
        Assert.True(combatant.IsPlayerControlled);
        Assert.Equal(0, combatant.Initiative);
        Assert.False(combatant.HasActed);
    }

    [Fact]
    public void Constructor_CanCreateEnemy()
    {
        var character = new Character("Orc", CharacterRace.Human, CharacterClass.Fighter);
        var combatant = new Combatant(character, false);

        Assert.False(combatant.IsPlayerControlled);
    }

    [Fact]
    public void Position_CanBeSet()
    {
        var character = new Character("Test", CharacterRace.Human, CharacterClass.Fighter);
        var combatant = new Combatant(character, true);

        combatant.X = 5;
        combatant.Y = 7;

        Assert.Equal(5, combatant.X);
        Assert.Equal(7, combatant.Y);
    }
}

public class CombatGridTests
{
    [Fact]
    public void Constructor_CreatesGridWithCorrectDimensions()
    {
        var grid = new CombatGrid(20, 20);

        Assert.Equal(20, grid.Width);
        Assert.Equal(20, grid.Height);
    }

    [Fact]
    public void CanMoveTo_ReturnsFalseForOutOfBounds()
    {
        var grid = new CombatGrid(20, 20);

        Assert.False(grid.CanMoveTo(-1, 5));
        Assert.False(grid.CanMoveTo(5, -1));
        Assert.False(grid.CanMoveTo(20, 5));
        Assert.False(grid.CanMoveTo(5, 20));
    }

    [Fact]
    public void CanMoveTo_ReturnsTrueForValidEmptyCell()
    {
        var grid = new CombatGrid(20, 20);

        Assert.True(grid.CanMoveTo(5, 5));
        Assert.True(grid.CanMoveTo(0, 0));
        Assert.True(grid.CanMoveTo(19, 19));
    }

    [Fact]
    public void CanMoveTo_ReturnsFalseForOccupiedCell()
    {
        var grid = new CombatGrid(20, 20);
        grid.SetOccupied(5, 5, true);

        Assert.False(grid.CanMoveTo(5, 5));
    }

    [Fact]
    public void SetOccupied_MarksAndUnmarksCells()
    {
        var grid = new CombatGrid(20, 20);

        grid.SetOccupied(3, 3, true);
        Assert.False(grid.CanMoveTo(3, 3));

        grid.SetOccupied(3, 3, false);
        Assert.True(grid.CanMoveTo(3, 3));
    }
}
