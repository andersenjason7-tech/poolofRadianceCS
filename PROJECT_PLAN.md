# Pool of Radiance C# Clone - Project Plan

## Overview
A C# remake of the classic 1988 SSI Gold Box game "Pool of Radiance" inspired by the yorkish C++ project.

## Technology Stack
- **Language**: C# 12 / .NET 8
- **Graphics Framework**: MonoGame (cross-platform, XNA successor)
- **Alternative**: Raylib-cs (simpler, lighter)
- **UI**: Custom rendering using sprite sheets
- **Data Format**: XML for game data (maps, characters, items, etc.)

## Project Structure
```
PoolOfRadianceCS/
├── src/
│   ├── Core/              # Core game engine
│   │   ├── Game.cs
│   │   ├── GameState.cs
│   │   └── InputManager.cs
│   ├── Graphics/          # Rendering and graphics
│   │   ├── SpriteManager.cs
│   │   ├── Renderer.cs
│   │   └── TileRenderer.cs
│   ├── Combat/            # Combat system
│   │   ├── CombatEngine.cs
│   │   ├── TurnManager.cs
│   │   └── AI.cs
│   ├── Characters/        # Character management
│   │   ├── Character.cs
│   │   ├── Party.cs
│   │   ├── CharacterClass.cs
│   │   └── Stats.cs
│   ├── World/             # World and maps
│   │   ├── Map.cs
│   │   ├── Tile.cs
│   │   ├── Location.cs
│   │   └── WorldManager.cs
│   ├── Items/             # Items and inventory
│   │   ├── Item.cs
│   │   ├── Inventory.cs
│   │   └── Equipment.cs
│   ├── UI/                # User interface
│   │   ├── Menu.cs
│   │   ├── Dialog.cs
│   │   ├── CharacterSheet.cs
│   │   └── CombatUI.cs
│   └── Data/              # Data loading
│       ├── DataLoader.cs
│       └── AssetManager.cs
├── assets/                # Game assets
│   ├── graphics/          # Original game graphics (extracted)
│   ├── data/              # XML game data
│   └── audio/             # Sound effects and music
└── tools/                 # Asset extraction tools
    └── GfxExtractor.cs
```

## Core Systems to Implement

### Phase 1: Foundation (Weeks 1-2)
- [x] Project setup
- [ ] MonoGame initialization
- [ ] Asset loading system
- [ ] Graphics extraction from original game files
- [ ] Basic rendering (tiles, sprites)
- [ ] Input handling

### Phase 2: Character System (Weeks 3-4)
- [ ] Character data structures (AD&D 1st Edition rules)
- [ ] Character classes (Fighter, Cleric, Magic-User, Thief, etc.)
- [ ] Races (Human, Elf, Dwarf, Halfling, etc.)
- [ ] Stats (STR, INT, WIS, DEX, CON, CHA)
- [ ] Character creation wizard
- [ ] Party management

### Phase 3: World & Navigation (Weeks 5-6)
- [ ] Map data structure
- [ ] Tile-based world rendering
- [ ] Party movement
- [ ] Area transitions
- [ ] NPCs and encounters
- [ ] 3D dungeon view (pseudo-3D)

### Phase 4: Combat System (Weeks 7-9)
- [ ] Turn-based tactical combat
- [ ] Combat grid system
- [ ] Movement and positioning
- [ ] Attack mechanics (THAC0, AC, etc.)
- [ ] Spell casting
- [ ] Enemy AI
- [ ] Combat UI

### Phase 5: Items & Equipment (Weeks 10-11)
- [ ] Item system
- [ ] Inventory management
- [ ] Equipment slots
- [ ] Shops and trading
- [ ] Treasure and loot

### Phase 6: Game Flow (Weeks 12-14)
- [ ] Save/Load system
- [ ] Journal and quest log
- [ ] Dialog system
- [ ] Story events
- [ ] Main menu
- [ ] Game over/victory conditions

### Phase 7: Polish (Weeks 15-16)
- [ ] Sound effects
- [ ] Music
- [ ] UI improvements
- [ ] Bug fixes
- [ ] Performance optimization
- [ ] Documentation

## AD&D 1st Edition Rules Implementation

### Character Stats
- Strength (STR): 3-18 (fighters can have 18/xx)
- Intelligence (INT): 3-18
- Wisdom (WIS): 3-18
- Dexterity (DEX): 3-18
- Constitution (CON): 3-18
- Charisma (CHA): 3-18

### Character Classes
- Fighter
- Ranger
- Paladin
- Cleric
- Magic-User
- Thief
- Multi-class combinations

### Combat Mechanics
- THAC0 (To Hit Armor Class 0)
- Armor Class (AC): 10 to -10 (lower is better)
- Initiative (DEX based)
- Weapon damage by type
- Saving throws

### Spells
- Magic-User spells (levels 1-5)
- Cleric spells (levels 1-4)
- Spell memorization
- Spell components (simplified)

## Original Graphics Extraction

The original Pool of Radiance uses the following file formats:
- **ECL files**: Compressed event data
- **GEO files**: Geographical/map data
- **PIC files**: Pictures and graphics
- **DAX files**: Data archives

We'll need to:
1. Extract graphics from original game files (legally obtained from GOG/Steam)
2. Convert to modern formats (PNG)
3. Organize into sprite sheets

## Data Format Example

### Character XML
```xml
<character>
  <name>Aldric</name>
  <race>Human</race>
  <class>Fighter</class>
  <level>1</level>
  <stats>
    <str>16</str>
    <int>10</int>
    <wis>12</wis>
    <dex>14</dex>
    <con>15</con>
    <cha>11</cha>
  </stats>
  <hp current="12" max="12"/>
  <ac>4</ac>
  <thac0>20</thac0>
</character>
```

## Legal Considerations
- This is a **remake**, not a port
- Original graphics require owning the original game
- No game data/story content will be included in the repository
- Users must provide their own copy of the original game for asset extraction

## References
- AD&D 1st Edition Player's Handbook
- AD&D 1st Edition Dungeon Master's Guide
- Pool of Radiance Manual
- Pool of Radiance Cluebook
- Gold Box Companion documentation
- MonoGame documentation

## Next Steps
1. Set up MonoGame project
2. Create basic window and rendering loop
3. Implement asset loading
4. Begin character data structures
