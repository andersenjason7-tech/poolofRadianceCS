# Development Guide

## Getting Started with Development

### Setting Up Your Environment

1. **Install .NET 8 SDK**
   ```bash
   # Download from https://dotnet.microsoft.com/download
   # Or on Ubuntu/Debian:
   sudo apt install dotnet-sdk-8.0
   ```

2. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/PoolOfRadianceCS
   cd PoolOfRadianceCS
   ```

3. **Open in Your IDE**
   - **Visual Studio 2022**: Open `PoolOfRadianceCS.csproj`
   - **VS Code**: Open folder, install C# extension
   - **Rider**: Open `PoolOfRadianceCS.csproj`

### Project Architecture

#### Core Systems

**Core/**
- `Game.cs`: Main game loop and state management
- `GameState.cs`: Enum for different game states
- `InputManager.cs`: Handles keyboard/mouse input

**Characters/**
- `Character.cs`: Main character class with stats and combat
- `CharacterStats.cs`: AD&D ability scores and modifiers
- `Party.cs`: Manages group of up to 6 characters
- `CharacterSupport.cs`: Supporting classes (Inventory, Equipment, etc.)

**Combat/**
- `CombatEngine.cs`: Turn-based tactical combat system
- Initiative rolling, turn order, AI

**Graphics/** (To be implemented)
- Sprite rendering
- Tile rendering
- UI rendering

**World/** (Partially implemented)
- Map system
- Tile-based world
- Navigation

### Coding Standards

#### Naming Conventions
- **Classes**: PascalCase (e.g., `CombatEngine`)
- **Methods**: PascalCase (e.g., `StartCombat`)
- **Private fields**: _camelCase with underscore (e.g., `_currentState`)
- **Properties**: PascalCase (e.g., `IsActive`)
- **Constants**: UPPER_CASE (e.g., `MAX_PARTY_SIZE`)

#### Code Style
```csharp
// Good
public class Character
{
    private int _hitPoints;
    
    public int HitPoints 
    { 
        get => _hitPoints;
        set => _hitPoints = Math.Max(0, value);
    }
    
    public void TakeDamage(int amount)
    {
        _hitPoints -= amount;
        if (_hitPoints < 0)
            _hitPoints = 0;
    }
}
```

### AD&D Rules Implementation

When implementing game mechanics, consult these resources:
1. AD&D 1st Edition Player's Handbook
2. AD&D 1st Edition Dungeon Master's Guide
3. Original Pool of Radiance manual
4. [AD&D Wiki](http://www.adnd1e.wikidot.com/)

#### Critical Rules to Follow

**Stats (3-18)**
- Roll 4d6, drop lowest
- Apply racial modifiers
- Fighters can have 18/xx Strength

**THAC0 (To Hit AC 0)**
- Decreases as level increases
- Varies by class
- Formula: roll + bonuses >= THAC0 - target AC

**Armor Class**
- 10 (no armor) to -10 (best)
- Lower is better
- Modified by DEX and equipment

**Hit Points**
- Roll hit die at each level
- Add CON modifier
- Minimum 1 HP per level

### Testing

#### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "TestCharacterCreation"
```

#### Writing Tests
```csharp
[TestClass]
public class CharacterTests
{
    [TestMethod]
    public void TestStatRolling()
    {
        var character = new Character("Test", CharacterRace.Human, CharacterClass.Fighter);
        character.RollStats();
        
        Assert.IsTrue(character.Stats.Strength >= 3 && character.Stats.Strength <= 18);
    }
}
```

### Common Tasks

#### Adding a New Character Class
1. Add to `CharacterClass` enum in `Character.cs`
2. Update `CalculateTHAC0()` method
3. Update hit die in `LevelUp()` method
4. Add class-specific abilities

#### Adding a New Spell
1. Create spell in spell data (XML or code)
2. Add to appropriate spell list
3. Implement spell effect in combat system
4. Add to spell casting UI

#### Adding a New Item
1. Define in `Items/` namespace
2. Add to item data files
3. Implement equip/use logic
4. Add to loot tables

### Debugging Tips

#### Common Issues

**Character stats not calculating correctly**
- Check racial modifiers are applied
- Verify stat cap (3-18)
- Check for proper stat modifier methods

**Combat not working**
- Verify initiative is rolled
- Check THAC0 calculations
- Ensure hit detection logic is correct

**Graphics not rendering**
- Check asset paths
- Verify MonoGame/Raylib initialization
- Check coordinate systems

### Next Steps for Development

#### Priority 1: Graphics System
```csharp
// TODO: Implement MonoGame renderer
public class MonoGameRenderer : Renderer
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    public override void Initialize()
    {
        // Initialize MonoGame
    }
}
```

#### Priority 2: World System
- Complete map loading from data files
- Implement collision detection
- Add area transitions
- Create 3D dungeon view

#### Priority 3: Spell System
- Define all 1st-5th level magic-user spells
- Define all 1st-4th level cleric spells
- Implement spell effects
- Add spell memorization system

### Resources

- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [MonoGame Documentation](https://docs.monogame.net/)
- [Raylib C# Documentation](https://github.com/ChrisDill/Raylib-cs)
- [Original Gold Box File Formats](https://github.com/simeonpilgrim/coab/tree/master/docs)

### Getting Help

- Check existing issues on GitHub
- Read the AD&D 1st Edition rules
- Examine the original Pool of Radiance
- Ask in project discussions

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/spell-system`)
3. Make your changes
4. Write/update tests
5. Commit with clear messages
6. Push and create a Pull Request

### Pull Request Guidelines

- Describe what your changes do
- Reference any related issues
- Include tests for new features
- Follow the coding standards
- Update documentation as needed
