using System.Collections.Generic;

namespace PoolOfRadiance.Characters
{
    public class Inventory
    {
        private List<Items.Item> _items;
        private int _gold;
        public const int MAX_ITEMS = 20;
        
        public Inventory()
        {
            _items = new List<Items.Item>();
            _gold = 0;
        }
        
        public bool AddItem(Items.Item item)
        {
            if (_items.Count >= MAX_ITEMS)
                return false;
                
            _items.Add(item);
            return true;
        }
        
        public bool RemoveItem(Items.Item item)
        {
            return _items.Remove(item);
        }
        
        public void AddGold(int amount)
        {
            _gold += amount;
        }
        
        public bool RemoveGold(int amount)
        {
            if (_gold >= amount)
            {
                _gold -= amount;
                return true;
            }
            return false;
        }
        
        public int Gold => _gold;
        public IReadOnlyList<Items.Item> Items => _items.AsReadOnly();
    }
    
    public class Equipment
    {
        public Items.Weapon? MainHand { get; set; }
        public Items.Armor? Armor { get; set; }
        public Items.Item? Shield { get; set; }
        public Items.Item? Helmet { get; set; }
        public Items.Item? Boots { get; set; }
        public Items.Item? Cloak { get; set; }
        public Items.Item? Ring1 { get; set; }
        public Items.Item? Ring2 { get; set; }
        public Items.Item? Amulet { get; set; }
        
        public int GetArmorClassBonus()
        {
            int bonus = 0;
            
            if (Armor != null)
                bonus += ((Items.Armor)Armor).ArmorClassBonus;
                
            if (Shield != null)
                bonus += 1; // Shields give +1 AC bonus
                
            return bonus;
        }
    }
    
    public class CharacterStatus
    {
        public bool IsParalyzed { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsStoned { get; set; }
        public bool IsSleeping { get; set; }
        public bool IsConfused { get; set; }
        public bool IsBlind { get; set; }
        
        public CharacterStatus()
        {
            IsParalyzed = false;
            IsPoisoned = false;
            IsStoned = false;
            IsSleeping = false;
            IsConfused = false;
            IsBlind = false;
        }
        
        public bool HasAnyEffect()
        {
            return IsParalyzed || IsPoisoned || IsStoned || 
                   IsSleeping || IsConfused || IsBlind;
        }
    }
    
    public class Spell
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public SpellSchool School { get; set; }
        public string Description { get; set; }
        public int Range { get; set; }
        public int Duration { get; set; }
        public string DamageOrEffect { get; set; }
        
        public Spell(string name, int level, SpellSchool school)
        {
            Name = name;
            Level = level;
            School = school;
            DamageOrEffect = "default";
            Description ="default";
        }
    }
    
    public enum SpellSchool
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    }
}

namespace PoolOfRadiance.Items
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
        public ItemType Type { get; set; }
        
        public Item(string name, ItemType type)
        {
            Name = name;
            Type = type;
            Description = "default";
        }
    }
    
    public class Weapon : Item
    {
        public int DamageDice { get; set; }
        public int DamageSides { get; set; }
        public int ToHitBonus { get; set; }
        public int DamageBonus { get; set; }
        public WeaponType WeaponType { get; set; }
        
        public Weapon(string name) : base(name, ItemType.Weapon)
        {
        }
    }
    
    public class Armor : Item
    {
        public int ArmorClassBonus { get; set; }
        public ArmorType ArmorType { get; set; }
        
        public Armor(string name) : base(name, ItemType.Armor)
        {
        }
    }
    
    public enum ItemType
    {
        Weapon,
        Armor,
        Shield,
        Potion,
        Scroll,
        Ring,
        Amulet,
        Misc
    }
    
    public enum WeaponType
    {
        Sword,
        Axe,
        Mace,
        Dagger,
        Bow,
        Crossbow,
        Staff,
        Spear
    }
    
    public enum ArmorType
    {
        Leather,
        StuddedLeather,
        ChainMail,
        SplintMail,
        PlateMail,
        FieldPlate,
        FullPlate
    }
}
