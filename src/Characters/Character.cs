using System;
using System.Collections.Generic;

namespace PoolOfRadiance.Characters
{
    /// <summary>
    /// Represents a character in the game (player or NPC)
    /// Implements AD&D 1st Edition rules
    /// </summary>
    public class Character
    {
        public string Name { get; set; }
        public CharacterRace Race { get; set; }
        public CharacterClass Class { get; set; }
        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        
        // Primary stats (3-18, fighters can have 18/xx STR)
        public CharacterStats Stats { get; set; }
        
        // Derived stats
        public int HitPointsCurrent { get; set; }
        public int HitPointsMax { get; set; }
        public int ArmorClass { get; set; }  // Lower is better (10 to -10)
        public int THAC0 { get; set; }       // To Hit AC 0
        
        // Combat
        public int Initiative { get; set; }
        public bool IsAlive => HitPointsCurrent > 0;
        public bool IsConscious => HitPointsCurrent > 0;
        
        // Inventory
        public Inventory Inventory { get; set; }
        public Equipment Equipment { get; set; }
        
        // Spells (for casters)
        public List<Spell> KnownSpells { get; set; }
        public List<Spell> MemorizedSpells { get; set; }
        
        // Position (for combat)
        public int X { get; set; }
        public int Y { get; set; }
        
        // Status effects
        public CharacterStatus Status { get; set; }
        
        public Character(string name, CharacterRace race, CharacterClass charClass)
        {
            Name = name;
            Race = race;
            Class = charClass;
            Level = 1;
            ExperiencePoints = 0;
            Stats = new CharacterStats();
            Inventory = new Inventory();
            Equipment = new Equipment();
            KnownSpells = new List<Spell>();
            MemorizedSpells = new List<Spell>();
            Status = new CharacterStatus();
            
            // Calculate initial derived stats
            CalculateDerivedStats();
        }
        
        public void RollStats()
        {
            Random rand = new Random();
            
            // Roll 4d6, drop lowest for each stat
            Stats.Strength = RollStat(rand);
            Stats.Intelligence = RollStat(rand);
            Stats.Wisdom = RollStat(rand);
            Stats.Dexterity = RollStat(rand);
            Stats.Constitution = RollStat(rand);
            Stats.Charisma = RollStat(rand);
            
            // Apply racial modifiers
            ApplyRacialModifiers();
        }
        
        private int RollStat(Random rand)
        {
            int[] rolls = new int[4];
            for (int i = 0; i < 4; i++)
            {
                rolls[i] = rand.Next(1, 7); // 1d6
            }
            
            Array.Sort(rolls);
            // Sum the three highest rolls
            return rolls[1] + rolls[2] + rolls[3];
        }
        
        private void ApplyRacialModifiers()
        {
            switch (Race)
            {
                case CharacterRace.Dwarf:
                    Stats.Constitution += 1;
                    Stats.Charisma -= 1;
                    break;
                case CharacterRace.Elf:
                    Stats.Dexterity += 1;
                    Stats.Constitution -= 1;
                    break;
                case CharacterRace.Halfling:
                    Stats.Dexterity += 1;
                    Stats.Strength -= 1;
                    break;
                case CharacterRace.HalfElf:
                    // No modifiers
                    break;
                case CharacterRace.Human:
                    // No modifiers
                    break;
            }
        }
        
        public void CalculateDerivedStats()
        {
            // Calculate THAC0 based on class and level
            THAC0 = CalculateTHAC0();
            
            // Calculate AC (base 10, modified by armor and DEX)
            ArmorClass = CalculateArmorClass();
            
            // HP already set during creation or level up
        }
        
        private int CalculateTHAC0()
        {
            // Base THAC0 by class and level (simplified)
            if (Class == CharacterClass.Fighter ||
                Class == CharacterClass.Paladin ||
                Class == CharacterClass.Ranger)
            {
                return 20 - Level;
            }

            if (Class == CharacterClass.Cleric)
                return 20 - (Level / 2);

            if (Class == CharacterClass.MagicUser)
                return 20 - (Level / 3);

            if (Class == CharacterClass.Thief)
                return 20 - (Level / 2);

            return 20;
        }
        
        private int CalculateArmorClass()
        {
            int baseAC = 10;
            
            // DEX modifier
            int dexMod = Stats.GetArmorClassModifier();
            
            // Equipment modifier (will be calculated from equipped items)
            int armorBonus = Equipment.GetArmorClassBonus();
            
            return baseAC - dexMod - armorBonus;
        }
        
        public void TakeDamage(int damage)
        {
            HitPointsCurrent -= damage;
            if (HitPointsCurrent < 0)
                HitPointsCurrent = 0;
        }
        
        public void Heal(int amount)
        {
            HitPointsCurrent += amount;
            if (HitPointsCurrent > HitPointsMax)
                HitPointsCurrent = HitPointsMax;
        }
        
        public void Rest()
        {
            // Heal 1 HP per day of rest
            Heal(1);
            
            // Recover spells
            RecoverSpells();
        }
        
        public void RecoverSpells()
        {
            MemorizedSpells.Clear();
            // Re-memorize spells based on level and INT/WIS
        }
        
        public int GetAttackRoll()
        {
            Random rand = new Random();
            int roll = rand.Next(1, 21); // 1d20
            
            // Add strength modifier for melee
            int strMod = Stats.GetToHitModifier();
            
            return roll + strMod;
        }
        
        public bool AttackTarget(Character target)
        {
            int attackRoll = GetAttackRoll();
            int targetAC = target.ArmorClass;
            
            // Calculate if hit: attack roll + THAC0 >= target AC
            int neededRoll = THAC0 - targetAC;
            
            if (attackRoll >= neededRoll)
            {
                // Hit! Calculate damage
                int damage = CalculateDamage();
                target.TakeDamage(damage);
                return true;
            }
            
            return false;
        }
        
        private int CalculateDamage()
        {
            // Base weapon damage (simplified - would come from equipped weapon)
            Random rand = new Random();
            int baseDamage = rand.Next(1, 9); // 1d8
            
            // Add strength modifier
            int strMod = Stats.GetDamageModifier();
            
            return baseDamage + strMod;
        }
        
        public void GainExperience(int xp)
        {
            ExperiencePoints += xp;
            
            // Check for level up
            CheckLevelUp();
        }
        
        private void CheckLevelUp()
        {
            int xpNeeded = GetXPForNextLevel();
            
            if (ExperiencePoints >= xpNeeded)
            {
                LevelUp();
            }
        }
        
        private int GetXPForNextLevel()
        {
            // Simplified XP table (would be class-specific)
            return Level * 2000;
        }
        
        private void LevelUp()
        {
            Level++;
            
            // Roll for additional HP
            Random rand = new Random();
            int hitDie;
            
            if (Class == CharacterClass.Fighter ||
                Class == CharacterClass.Paladin ||
                Class == CharacterClass.Ranger)
                hitDie = 10;
            else if (Class == CharacterClass.Cleric)
                hitDie = 8;
            else if (Class == CharacterClass.Thief)
                hitDie = 6;
            else if (Class == CharacterClass.MagicUser)
                hitDie = 4;
            else
                hitDie = 6; // default hit die for unexpected classes
            
            int hpGain = rand.Next(1, hitDie + 1) + Stats.GetHPModifier();
            if (hpGain < 1) hpGain = 1; // Always gain at least 1 HP
            
            HitPointsMax += hpGain;
            HitPointsCurrent = HitPointsMax;
            
            // Recalculate derived stats
            CalculateDerivedStats();
            
            Console.WriteLine($"{Name} has reached level {Level}!");
        }
    }
    
    public enum CharacterRace
    {
        Human,
        Elf,
        Dwarf,
        Halfling,
        HalfElf,
        Gnome
    }
    
    // CharacterClass has been moved to its own file and is now a class instead of
    // an enum.  See CharacterClass.cs for details.
}
