using System;
using System.Collections.Generic;
using System.Linq;

namespace PoolOfRadiance.Characters
{
    /// <summary>
    /// Manages a party of up to 6 characters
    /// </summary>
    public class Party
    {
        private List<Character> _members = new List<Character>();
        public const int MAX_PARTY_SIZE = 6;
        
        public int Gold { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        public Party()
        {
            _members = new List<Character>();
            Gold = 100; // Starting gold
        }
        
        public bool AddMember(Character character)
        {
            if (_members.Count >= MAX_PARTY_SIZE)
            {
                Console.WriteLine("Party is full!");
                return false;
            }
            
            _members.Add(character);
            return true;
        }
        
        public bool RemoveMember(Character character)
        {
            return _members.Remove(character);
        }
        
        public IReadOnlyList<Character> Members => _members.AsReadOnly();
        
        public int Count => _members.Count;
        
        public bool IsPartyAlive()
        {
            return _members.Any(c => c.IsAlive);
        }
        
        public List<Character> GetAliveMembers()
        {
            return _members.Where(c => c.IsAlive).ToList();
        }
        
        public List<Character> GetDeadMembers()
        {
            return _members.Where(c => !c.IsAlive).ToList();
        }
        
        public void Rest()
        {
            Console.WriteLine("The party rests...");
            foreach (var member in _members)
            {
                if (member.IsAlive)
                {
                    member.Rest();
                }
            }
        }
        
        public void DistributeGold(int amount)
        {
            Gold += amount;
        }
        
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }
        
        public void DistributeExperience(int totalXP)
        {
            if (_members.Count == 0) return;
            
            int xpPerMember = totalXP / _members.Count;
            
            foreach (var member in _members.Where(m => m.IsAlive))
            {
                member.GainExperience(xpPerMember);
            }
        }
        
        public Character? GetLeader()
        {
            
            return _members.FirstOrDefault(c => c.IsAlive);
        }
        
        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void PrintStatus()
        {
            Console.WriteLine("===== PARTY STATUS =====");
            Console.WriteLine($"Gold: {Gold}");
            Console.WriteLine($"Position: ({X}, {Y})");
            Console.WriteLine("fMembers:");
            
            for (int i = 0; i < _members.Count; i++)
            {
                var member = _members[i];
                string status = member.IsAlive ? "ALIVE" : "DEAD";
                Console.WriteLine($"{i + 1}. {member.Name} - Lvl {member.Level} {member.Race} {member.Class} - HP: {member.HitPointsCurrent}/{member.HitPointsMax} - {status}");
            }
            
            Console.WriteLine("========================");
        }
    }
}
