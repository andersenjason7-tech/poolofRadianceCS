# Pool of Radiance C# Clone - Quick Start

## What I've Created

A complete C# project structure for recreating Pool of Radiance, inspired by the yorkish C++ project. This includes:

### ✅ Implemented Features

1. **Complete Character System**
   - AD&D 1st Edition stat system (STR, INT, WIS, DEX, CON, CHA)
   - Character classes: Fighter, Ranger, Paladin, Cleric, Magic-User, Thief
   - Character races: Human, Elf, Dwarf, Halfling, Half-Elf, Gnome
   - Stat rolling (4d6 drop lowest)
   - Racial modifiers
   - Exceptional strength (18/xx) for fighters

2. **Party Management**
   - Up to 6 party members
   - Party gold and inventory
   - Group movement
   - Experience distribution

3. **Combat System**
   - Turn-based tactical combat
   - Initiative system (DEX-based)
   - THAC0 attack system
   - Combat grid (20x20)
   - Simple enemy AI
   - Damage calculation
   - Experience rewards

4. **Character Progression**
   - Experience points
   - Level up system
   - HP gain on level up
   - Automatic THAC0 improvement

5. **Items & Equipment**
   - Inventory system
   - Equipment slots
   - Weapons and armor
   - AC calculation from equipment

### 📁 Project Structure

```
PoolOfRadianceCS/
├── src/
│   ├── Core/
│   │   ├── Game.cs              # Main game loop
│   │   └── SystemStubs.cs       # Input, rendering, data systems
│   ├── Characters/
│   │   ├── Character.cs         # Main character class
│   │   ├── CharacterStats.cs    # AD&D ability scores
│   │   ├── CharacterSupport.cs  # Inventory, equipment, spells
│   │   └── Party.cs             # Party management
│   ├── Combat/
│   │   └── CombatEngine.cs      # Turn-based combat
│   └── Program.cs               # Entry point with demo
├── assets/                      # (Empty) For game assets
├── tools/                       # (Empty) For asset tools
├── PoolOfRadianceCS.csproj     # Project file
├── PROJECT_PLAN.md              # Detailed development plan
├── README.md                    # Main documentation
├── DEVELOPMENT.md               # Developer guide
└── build.sh                     # Build script
```

### 🚀 How to Run

**Option 1: If you have .NET installed**
```bash
cd PoolOfRadianceCS
dotnet run
```

**Option 2: Using the build script**
```bash
cd PoolOfRadianceCS
./build.sh
```

This will run a demo that shows:
- Character creation with randomized AD&D stats
- Party formation (4 characters)
- Simulated combat encounter
- Experience gain and leveling

### 📊 Current Demo Output

The demo creates:
- **Aldric** - Human Fighter
- **Thalia** - Human Cleric  
- **Eldrin** - Elf Magic-User
- **Raven** - Halfling Thief

Then simulates combat against:
- 2 Goblins

Shows:
- Character stats and derived values (AC, THAC0)
- Initiative rolls
- Combat rounds with attacks
- Post-combat status
- Experience distribution
- Potential level-ups

### 🎯 Next Steps

To turn this into a complete game:

1. **Graphics** (Next Priority)
   - Integrate MonoGame or Raylib-cs
   - Extract original game graphics
   - Implement sprite/tile rendering
   - Create UI system

2. **World System**
   - Load map data
   - Implement movement
   - Add NPCs and encounters
   - Create 3D dungeon view

3. **Complete Combat**
   - Ranged weapons
   - Spell casting
   - Area effects
   - Status effects
   - Improved AI

4. **Items & Magic**
   - Full item database
   - All AD&D spells
   - Spell memorization
   - Shops and trading

5. **Story & Content**
   - All locations from original game
   - NPCs and dialog
   - Quest system
   - Journal

### 💡 Key Design Decisions

1. **C# over C++**: Modern language, easier development, cross-platform
2. **AD&D Rules**: Faithful to 1st Edition for authenticity
3. **Modular Design**: Easy to extend and test
4. **No Original Assets**: Requires user to own original game
5. **MonoGame Target**: Similar to SDL2 but C#-native

### 🔧 Technologies Used

- **C# 12** / **.NET 8**: Modern, cross-platform
- **MonoGame** (planned): Cross-platform game framework
- **XML**: For game data (maps, items, monsters)

### 📖 Documentation Included

1. **README.md**: Overview, features, build instructions
2. **PROJECT_PLAN.md**: Detailed development roadmap
3. **DEVELOPMENT.md**: Coding standards, architecture, guidelines
4. **This file**: Quick start guide

### ⚖️ Legal Notes

- This is a **remake**, not a port
- No original game code or assets included
- Users must own original game
- Educational/personal use only

### 🤝 Comparison to yorkish Project

**Similar:**
- Goal: Remake Pool of Radiance
- No reverse engineering of code
- Extract original graphics
- AD&D rules implementation

**Different:**
- Language: C# instead of C++
- Framework: MonoGame instead of SDL2
- More modular architecture
- Better documentation
- Active development with clear roadmap

### ⚡ Quick Commands

```bash
# Build
dotnet build

# Run demo
dotnet run

# Clean build
dotnet clean

# Create release build
dotnet build -c Release

# Future: Run tests
dotnet test
```

### 🎮 Playing the Demo

When you run the demo, you'll see:

1. Party creation with randomized stats
2. Character information display
3. Combat encounter begins
4. Initiative rolls
5. Turn-by-turn combat (auto-played for demo)
6. Post-combat status
7. Experience distribution
8. Level-up messages (if applicable)

### 📝 Notes

- Current version is console-only (no graphics yet)
- Combat is auto-played in the demo
- Many systems are stubbed out for future implementation
- Focus on solid core mechanics first, graphics second

### 🐛 Known Limitations

- No graphics (console only for now)
- No save/load
- Limited AI
- No spells yet
- Simplified item system
- Demo auto-plays combat

---

**Ready to start?** Just run `dotnet run` in the PoolOfRadianceCS directory!

For development, see DEVELOPMENT.md for coding standards and architecture details.
