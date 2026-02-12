using System;

namespace PoolOfRadiance.Characters
{
    /// <summary>
    /// Character ability scores following AD&D 1st Edition rules
    /// </summary>
    public class CharacterStats
    {
        private int _strength;
        private int _strengthPercentile; // For fighters with 18/xx strength
        private int _intelligence;
        private int _wisdom;
        private int _dexterity;
        private int _constitution;
        private int _charisma;
        
        public int Strength
        {
            get => _strength;
            set => _strength = Math.Clamp(value, 3, 18);
        }
        
        public int StrengthPercentile
        {
            get => _strengthPercentile;
            set => _strengthPercentile = Math.Clamp(value, 0, 100);
        }
        
        public int Intelligence
        {
            get => _intelligence;
            set => _intelligence = Math.Clamp(value, 3, 18);
        }
        
        public int Wisdom
        {
            get => _wisdom;
            set => _wisdom = Math.Clamp(value, 3, 18);
        }
        
        public int Dexterity
        {
            get => _dexterity;
            set => _dexterity = Math.Clamp(value, 3, 18);
        }
        
        public int Constitution
        {
            get => _constitution;
            set => _constitution = Math.Clamp(value, 3, 18);
        }
        
        public int Charisma
        {
            get => _charisma;
            set => _charisma = Math.Clamp(value, 3, 18);
        }
        
        public CharacterStats()
        {
            // Initialize with average stats
            _strength = 10;
            _intelligence = 10;
            _wisdom = 10;
            _dexterity = 10;
            _constitution = 10;
            _charisma = 10;
            _strengthPercentile = 0;
        }
        
        /// <summary>
        /// Get the to-hit modifier from Strength
        /// </summary>
        public int GetToHitModifier()
        {
            if (Strength < 3) return -3;
            if (Strength <= 5) return -2;
            if (Strength <= 7) return -1;
            if (Strength <= 15) return 0;
            if (Strength == 16) return 0;
            if (Strength == 17) return 1;
            if (Strength == 18)
            {
                if (StrengthPercentile == 0) return 1;
                if (StrengthPercentile <= 50) return 1;
                if (StrengthPercentile <= 75) return 2;
                if (StrengthPercentile <= 90) return 2;
                if (StrengthPercentile <= 99) return 2;
                return 3; // 18/00
            }
            return 0;
        }
        
        /// <summary>
        /// Get the damage modifier from Strength
        /// </summary>
        public int GetDamageModifier()
        {
            if (Strength < 3) return -2;
            if (Strength <= 5) return -1;
            if (Strength <= 7) return -1;
            if (Strength <= 15) return 0;
            if (Strength == 16) return 1;
            if (Strength == 17) return 1;
            if (Strength == 18)
            {
                if (StrengthPercentile == 0) return 2;
                if (StrengthPercentile <= 50) return 3;
                if (StrengthPercentile <= 75) return 3;
                if (StrengthPercentile <= 90) return 4;
                if (StrengthPercentile <= 99) return 5;
                return 6; // 18/00
            }
            return 0;
        }
        
        /// <summary>
        /// Get the armor class modifier from Dexterity
        /// </summary>
        public int GetArmorClassModifier()
        {
            if (Dexterity <= 3) return -4;
            if (Dexterity == 4) return -3;
            if (Dexterity == 5) return -2;
            if (Dexterity == 6) return -1;
            if (Dexterity <= 14) return 0;
            if (Dexterity == 15) return 1;
            if (Dexterity == 16) return 2;
            if (Dexterity == 17) return 3;
            if (Dexterity >= 18) return 4;
            return 0;
        }
        
        /// <summary>
        /// Get the HP modifier from Constitution
        /// </summary>
        public int GetHPModifier()
        {
            if (Constitution <= 3) return -2;
            if (Constitution <= 6) return -1;
            if (Constitution <= 14) return 0;
            if (Constitution == 15) return 1;
            if (Constitution == 16) return 2;
            if (Constitution == 17) return 3; // +3 for fighters, +2 for others
            if (Constitution >= 18) return 4; // +4 for fighters, +2 for others
            return 0;
        }
        
        /// <summary>
        /// Get max number of spells learnable for magic-users (from INT)
        /// </summary>
        public int GetMaxSpellsPerLevel()
        {
            if (Intelligence <= 8) return 0;
            if (Intelligence == 9) return 4;
            if (Intelligence == 10) return 5;
            if (Intelligence == 11) return 6;
            if (Intelligence == 12) return 7;
            if (Intelligence == 13) return 9;
            if (Intelligence == 14) return 11;
            if (Intelligence == 15) return 13;
            if (Intelligence == 16) return 15;
            if (Intelligence == 17) return 18;
            if (Intelligence >= 18) return 20; // All spells
            return 0;
        }
        
        /// <summary>
        /// Chance to learn a spell for magic-users (from INT)
        /// </summary>
        public int GetSpellLearnChance()
        {
            if (Intelligence <= 8) return 0;
            if (Intelligence == 9) return 35;
            if (Intelligence == 10) return 40;
            if (Intelligence == 11) return 45;
            if (Intelligence == 12) return 50;
            if (Intelligence == 13) return 55;
            if (Intelligence == 14) return 60;
            if (Intelligence == 15) return 65;
            if (Intelligence == 16) return 70;
            if (Intelligence == 17) return 75;
            if (Intelligence >= 18) return 85;
            return 0;
        }
        
        /// <summary>
        /// Bonus cleric spells from Wisdom
        /// </summary>
        public int GetBonusClericSpells(int spellLevel)
        {
            if (Wisdom < 13) return 0;
            if (spellLevel > 6) return 0;
            
            if (Wisdom == 13 && spellLevel == 1) return 1;
            if (Wisdom == 14 && spellLevel <= 1) return 1;
            if (Wisdom == 15 && spellLevel <= 2) return 1;
            if (Wisdom == 16 && spellLevel <= 2) return 1;
            if (Wisdom == 17 && spellLevel <= 3) return 1;
            if (Wisdom >= 18 && spellLevel <= 4) return 1;
            
            return 0;
        }
        
        /// <summary>
        /// Reaction adjustment from Charisma
        /// </summary>
        public int GetReactionModifier()
        {
            if (Charisma <= 3) return -5;
            if (Charisma <= 5) return -3;
            if (Charisma <= 7) return -2;
            if (Charisma == 8) return -1;
            if (Charisma <= 12) return 0;
            if (Charisma == 13) return 1;
            if (Charisma <= 15) return 2;
            if (Charisma == 16) return 3;
            if (Charisma == 17) return 4;
            if (Charisma >= 18) return 5;
            return 0;
        }
        
        public override string ToString()
        {
            string strDisplay = Strength.ToString();
            if (Strength == 18 && StrengthPercentile > 0)
            {
                strDisplay = StrengthPercentile == 100 ? "18/00" : $"18/{StrengthPercentile:00}";
            }
            
            return $"STR: {strDisplay}, INT: {Intelligence}, WIS: {Wisdom}, " +
                   $"DEX: {Dexterity}, CON: {Constitution}, CHA: {Charisma}";
        }
    }
}
