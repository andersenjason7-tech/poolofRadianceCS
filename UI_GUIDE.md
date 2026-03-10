# Pool of Radiance UI System - User Guide

## Overview

The game now features a complete screen-based UI system similar to the original 1988 Pool of Radiance! You can explore towns, venture into dungeons, and engage in tactical turn-based combat.

## Game Modes

When you start the game, you'll see two options:

### 1. New Game (Full UI Experience)
This is the complete game experience with:
- Town exploration
- Dungeon/sewer exploration with random encounters
- Tactical combat
- Shops, temples, and taverns
- Party management

### 2. Quick Combat Demo
Jump straight into a combat encounter to test the tactical combat system.

## Screen Types

### Town Screen
**Where you are:** The town of Phlan
**What you can do:**
- Visit the Training Hall
- Go to the Temple for healing
- Shop for equipment
- Visit the Tavern for rumors
- Explore the Sewers (random encounters!)
- View party status
- Rest your party

**Controls:**
- `1-9`: Select location
- `8`: View detailed party status
- `9`: Rest party (heal 1 HP per member)

### Exploration Screen (Sewers/Dungeons)
**Where you are:** Inside dangerous areas
**What you see:**
- Top-down map view
- Your party position (@)
- Walls (#), Floors (.), Doors (+)
- Party status at bottom

**Controls:**
- `Arrow Keys` or `WASD`: Move your party
- `C`: View character status
- `Q` or `ESC`: Exit area

**Gameplay:**
- Navigate through the dungeon
- Avoid or fight random encounters (10% chance per step)
- Find the exit door (+) at the top of the map
- Each step might trigger an encounter!

### Combat Screen
**Where you are:** Engaged in tactical combat
**What you see:**
- Battlefield visualization with enemy and party positions
- Individual character HP bars
- Enemy status
- Combat log showing all actions
- Turn order

**How Combat Works:**
1. **Initiative** - All combatants roll for turn order
2. **Player Turns:**
   - `1`: Attack
   - `2`: Delay turn
   - `3`: Pass/Do nothing
3. **Target Selection:**
   - `↑↓`: Select enemy target
   - `ENTER`: Confirm attack
   - `ESC`: Cancel
4. **Enemy Turns:** Automated
5. **Victory/Defeat:** Automatic when one side is eliminated

**Combat Display:**
```
  ╔═══════════════════════════════╗
  ║ BATTLEFIELD                   ║
  ╠═══════════════════════════════╣
  │ ENEMIES:  [G]  G   G          │
  │                               │
  │        [BATTLEFIELD]          │
  │                               │
  │ PARTY:     A   T   E   R      │
  └───────────────────────────────┘
```
- **Letters represent characters:** First letter of their name
- **[Brackets]** indicate selected target
- **Colors:** Green = Party, Red = Enemies, Yellow = Active turn

### Party Status Screen
**Where you are:** Detailed party information
**What you see:**
- Complete stats for all party members
- HP, AC, THAC0, Experience
- Ability scores (STR, INT, WIS, DEX, CON, CHA)
- Character status (Alive/Dead)
- **Inventory** (gold and carried items per character)

**Controls:**
- `Any key`: Return to previous screen

### Temple Screen
**Services:**
- Heal All Party Members: 50 gold
- Rest: Free (heals 1 HP per member)

### Shop Screen
**Available Items:**
- Longsword: 15 gold
- Chain Mail: 75 gold
- Healing Potion: 10 gold

### Tavern Screen
**Features:**
- Listen to rumors about quests and locations
- Gather information about the world

## Game Flow Example

1. **Start in Town**
   ```
   Choose option 1 from main menu
   → Character creation
   → Enter town of Phlan
   ```

2. **Explore Town**
   ```
   Town Screen
   → Visit Temple (option 2) to heal
   → Visit Shop (option 3) to buy equipment
   → Visit Tavern (option 4) for rumors
   ```

3. **Enter Dungeon**
   ```
   Town Screen → Choose "Explore Sewers" (option 6)
   → Exploration Screen appears
   → Navigate using arrow keys
   ```

4. **Random Encounter**
   ```
   While exploring, encounter triggers!
   → Combat Screen appears
   → Fight monsters
   → Gain XP and gold on victory
   → Return to exploration
   ```

5. **Return to Town**
   ```
   Find exit or press Q
   → Back to Town Screen
   → Heal at temple
   → Shop for better gear
   → Repeat!
   ```

## Character Classes

Your party includes:
- **Aldric** (Fighter): High HP, strong attacks
- **Thalia** (Cleric): Healing, moderate combat
- **Eldrin** (Magic-User): Low HP, powerful spells (coming soon)
- **Raven** (Thief): Good DEX, sneaky skills (coming soon)

## AD&D 1st Edition Combat

### THAC0 (To Hit Armor Class 0)
- Lower is better
- Fighters have best THAC0
- Improves as you level up

### Armor Class (AC)
- Range: 10 (no armor) to -10 (best)
- Lower is better
- Modified by armor and DEX

### Hit Calculation
```
Attack Roll (1d20) + Bonuses >= THAC0 - Target AC
```

### Experience Points
- Gain XP from defeating enemies
- Level up at Training Hall when you have enough XP
- Levels increase HP, THAC0, and abilities

## Tips for Success

1. **Heal before exploring** - Visit the temple before venturing into sewers
2. **Watch your HP** - Combat is deadly; retreat if low on health
3. **Use the terrain** - Dungeons have limited exits; plan your route
4. **Save your gold** - You'll need it for healing and equipment
5. **Check party status often** - Press 'C' during exploration or '8' in town
6. **Random encounters are dangerous** - Be prepared to fight at any time

## Future Features (Coming Soon)

- Spell casting system
- More dungeon types
- Quest system
- Equipment actually affecting stats
- Character leveling at Training Hall
- Save/Load game
- More enemy types
- Boss encounters
- Full story campaign

## Technical Notes

### Screen Stack System
The game uses a screen management system where screens can be pushed and popped:
- **Push** = Add new screen on top (previous screen pauses)
- **Pop** = Remove current screen (previous screen resumes)

Example:
```
Town → Exploration → Combat → Back to Exploration → Back to Town
```

### File Structure
```
UI/
├── ScreenManager.cs          # Screen stack management
└── Screens/
    ├── TownScreen.cs          # Town exploration
    ├── ExplorationScreen.cs   # Dungeon crawling
    ├── CombatScreen.cs        # Tactical combat
    └── SupportScreens.cs      # Temple, Shop, etc.
```

## Controls Reference

### Universal
- `ESC` or `Q`: Exit current screen (where applicable)
- `C`: View character status (in exploration)

### Town Screen
- `1`: Training Hall
- `2`: Temple
- `3`: Shop
- `4`: Tavern
- `5`: City Hall (coming soon)
- `6`: Explore Sewers
- `7`: Leave Town
- `8`: View Party
- `9`: Rest

### Exploration
- `↑↓←→` or `WASD`: Move party
- `C`: Character status
- `Q` or `ESC`: Exit dungeon

### Combat
- `1`: Attack
- `2`: Delay
- `3`: Pass
- `↑↓`: Select target
- `ENTER`: Confirm action
- `ESC`: Cancel

## Troubleshooting

**Console size issues?**
- The game works best with a console window of at least 120x40 characters
- If text looks cramped, maximize your console window

**Can't see colors?**
- Make sure your terminal supports ANSI colors
- Windows 10+ Command Prompt and PowerShell support colors by default

**Game running slowly?**
- Normal - the game includes deliberate delays for readability
- Combat pauses briefly between actions

## Example Play Session

```
1. Start game → Choose "New Game"
2. Party created → Enter Phlan
3. Town screen → Press 2 (Temple)
4. Temple → Press 1 (Heal for 50 gold)
5. Back to town → Press 6 (Explore Sewers)
6. Exploration → Move around with arrow keys
7. Encounter triggers! → Combat screen
8. Combat → Press 1 to attack
9. Select target with ↑↓ → Press ENTER
10. Win combat → Gain 150 XP and 30 gold!
11. Back to exploration → Find exit
12. Back to town → Visit shop with new gold
13. Repeat!
```

Enjoy your adventure in Pool of Radiance!
