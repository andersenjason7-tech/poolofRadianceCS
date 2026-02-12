# Pool of Radiance - C# Remake

A C# remake of the classic 1988 SSI Gold Box game "Pool of Radiance", inspired by the [yorkish/pool-of-radiance](https://github.com/yorkish/pool-of-radiance) C++ project.

## Overview

This project aims to recreate the legendary AD&D (Advanced Dungeons & Dragons) 1st Edition computer RPG "Pool of Radiance" using modern C# and .NET technologies.

### Features (Planned)

- ✅ AD&D 1st Edition character creation and rules
- ✅ Turn-based tactical combat system
- ✅ Party management (up to 6 characters)
- ✅ Character classes: Fighter, Ranger, Paladin, Cleric, Magic-User, Thief
- ✅ Character races: Human, Elf, Dwarf, Halfling, Half-Elf, Gnome
- ✅ Complete stat system (STR, INT, WIS, DEX, CON, CHA)
- ⬜ Original graphics extraction and rendering
- ⬜ World exploration and navigation
- ⬜ Spell casting system
- ⬜ Items and equipment
- ⬜ NPCs and dialog
- ⬜ Save/Load system
- ⬜ Complete story and quests

## Current Status

🚧 **Early Development** - Currently implemented:
- Core character system with AD&D stats
- Party management
- Basic combat engine
- Character creation and leveling
- Console-based demo

## Technology Stack

- **Language**: C# 12 / .NET 8
- **Graphics** (planned): MonoGame or Raylib-cs
- **Data Format**: XML for game data
- **Platform**: Cross-platform (Windows, Linux, macOS)

## Project Structure

```
PoolOfRadianceCS/
├── src/
│   ├── Core/              # Core game engine
│   ├── Graphics/          # Rendering and graphics
│   ├── Combat/            # Combat system
│   ├── Characters/        # Character management
│   ├── World/             # World and maps
│   ├── Items/             # Items and inventory
│   ├── UI/                # User interface
│   └── Data/              # Data loading
├── assets/                # Game assets
└── tools/                 # Asset extraction tools
```

## Building and Running

### Prerequisites

- .NET 8 SDK or later
- (Optional) Original Pool of Radiance game files for asset extraction

### Build

```bash
# Clone the repository
git clone https://github.com/yourusername/PoolOfRadianceCS
cd PoolOfRadianceCS

# Build the project
dotnet build

# Run the demo
dotnet run
```

### Running the Demo

The current demo showcases:
- Character creation with randomized AD&D stats
- Party formation
- Turn-based combat simulation
- Experience and leveling system

```bash
dotnet run
```

## AD&D 1st Edition Rules

This remake faithfully implements AD&D 1st Edition rules:

### Character Stats
- **Strength (STR)**: Physical power, affects melee damage and hit chance
- **Intelligence (INT)**: Mental acuity, affects magic-user spells
- **Wisdom (WIS)**: Willpower and perception, affects cleric spells
- **Dexterity (DEX)**: Agility and reflexes, affects AC and initiative
- **Constitution (CON)**: Health and stamina, affects HP
- **Charisma (CHA)**: Leadership and personality, affects reactions

### Combat System
- **THAC0** (To Hit Armor Class 0): Lower is better
- **Armor Class (AC)**: 10 to -10, lower is better
- **Initiative**: DEX-based turn order
- **Saving Throws**: Resist spells and special attacks

### Character Classes
- **Fighter**: Warriors and soldiers (d10 HP)
- **Ranger**: Wilderness fighters (d10 HP)
- **Paladin**: Holy warriors (d10 HP)
- **Cleric**: Divine spellcasters (d8 HP)
- **Magic-User**: Arcane spellcasters (d4 HP)
- **Thief**: Rogues and scouts (d6 HP)

### Races
- **Human**: No modifiers, can be any class
- **Elf**: +1 DEX, -1 CON
- **Dwarf**: +1 CON, -1 CHA
- **Halfling**: +1 DEX, -1 STR
- **Half-Elf**: No modifiers
- **Gnome**: Varied modifiers

## Code Examples

### Creating a Character

```csharp
var fighter = new Character("Aldric", CharacterRace.Human, CharacterClass.Fighter);
fighter.RollStats();
fighter.HitPointsMax = 10;
fighter.HitPointsCurrent = 10;
```

### Creating a Party

```csharp
Party party = new Party();
party.AddMember(fighter);
party.AddMember(cleric);
party.AddMember(wizard);
```

### Starting Combat

```csharp
var combat = new CombatEngine();
combat.StartCombat(playerParty, enemyList);
```

## Legal Notice

This is a **remake** project, not a port. The original Pool of Radiance is copyright SSI/TSR/Wizards of the Coast. This project:

- Does NOT include original game assets, data, or story content
- Requires users to own the original game for asset extraction
- Is for educational and personal use only
- Implements game mechanics (which are not copyrightable)

If you want to use original graphics:
1. Purchase Pool of Radiance from [GOG.com](https://www.gog.com/) or Steam
2. Use extraction tools (like Gold Box Companion) to extract graphics
3. Place extracted assets in the `assets/` directory

## Roadmap

### Phase 1: Foundation ✅ (Current)
- [x] Project structure
- [x] Character system
- [x] Combat basics
- [x] Party management

### Phase 2: Graphics (Next)
- [ ] MonoGame/Raylib integration
- [ ] Asset loading
- [ ] Tile rendering
- [ ] Sprite rendering
- [ ] UI rendering

### Phase 3: World
- [ ] Map system
- [ ] Movement
- [ ] Encounters
- [ ] 3D dungeon view

### Phase 4: Complete Systems
- [ ] Full combat
- [ ] Spell system
- [ ] Items and equipment
- [ ] Dialog system
- [ ] Save/Load

### Phase 5: Content
- [ ] Story implementation
- [ ] All monsters
- [ ] All items
- [ ] All spells
- [ ] All locations

## Contributing

Contributions are welcome! Areas that need help:
- Graphics programming (MonoGame/Raylib)
- Asset extraction tools
- UI/UX design
- Testing and bug reports
- Documentation

## Resources

- [Original Pool of Radiance Manual](http://www.weekendwastemonster.net/crpgs/pool/pool.html)
- [AD&D 1st Edition Rules](https://en.wikipedia.org/wiki/Editions_of_Dungeons_%26_Dragons#Advanced_Dungeons_&_Dragons)
- [Gold Box Companion](https://gbc.zorbus.net/)
- [yorkish C++ Project](https://github.com/yorkish/pool-of-radiance)

## Acknowledgments

- Original game by Strategic Simulations, Inc. (SSI) and TSR
- Inspired by the yorkish C++ remake project
- Thanks to the Gold Box preservation community
- MonoGame and Raylib communities

## License

This project is released under the MIT License. See LICENSE file for details.

Note: This license applies only to the code in this repository. Original Pool of Radiance assets and content remain property of their respective copyright holders.

---

**Disclaimer**: This is a fan project and is not affiliated with or endorsed by SSI, TSR, Wizards of the Coast, or any other rights holders.
