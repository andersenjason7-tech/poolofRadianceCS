using System;
using System.Collections.Generic;
using PoolOfRadiance.Characters;

namespace PoolOfRadiance
{
    /// <summary>
    /// Manages party composition, metadata, and constraints
    /// Handles character joining/leaving with size validation
    /// Tracks party information like name, quests, alliances, and enemies
    /// </summary>
    public class PartyManagement
    {
        private const int MinPartySize = 2;
        private const int MaxPartySize = 8;

        private Party _party;
        private string _partyName;
        private List<string> _quests;
        private List<string> _alliances;
        private List<string> _enemies;

        public string PartyName
        {
            get => _partyName;
            set => _partyName = value;
        }

        public Party Party => _party;

        public List<string> Quests => _quests;
        public List<string> Alliances => _alliances;
        public List<string> Enemies => _enemies;

        public int PartySize => _party.Count;

        public PartyManagement(string partyName = "Adventuring Party")
        {
            _party = new Party();
            _partyName = partyName;
            _quests = new List<string>();
            _alliances = new List<string>();
            _enemies = new List<string>();
        }

        /// <summary>
        /// Attempt to add a character to the party
        /// </summary>
        /// <returns>True if character was added, false otherwise</returns>
        public bool AddCharacter(Character character)
        {
            if (character == null)
            {
                Console.WriteLine("Error: Cannot add null character to party");
                return false;
            }

            if (PartySize >= MaxPartySize)
            {
                Console.WriteLine($"Error: Party is at maximum size of {MaxPartySize} characters");
                return false;
            }

            _party.AddMember(character);
            Console.WriteLine($"✓ {character.Name} has joined the party");
            return true;
        }

        /// <summary>
        /// Attempt to remove a character from the party
        /// </summary>
        /// <returns>True if character was removed, false otherwise</returns>
        public bool RemoveCharacter(Character character)
        {
            if (character == null)
            {
                Console.WriteLine("Error: Cannot remove null character from party");
                return false;
            }

            if (PartySize <= MinPartySize)
            {
                Console.WriteLine($"Error: Party must have at least {MinPartySize} characters");
                return false;
            }

            // Remove from party if found
            bool removed = _party.RemoveMember(character);
            if (removed)
            {
                Console.WriteLine($"✓ {character.Name} has left the party");
            }
            else
            {
                Console.WriteLine($"Error: {character.Name} is not in the party");
            }

            return removed;
        }

        /// <summary>
        /// Remove a character by name
        /// </summary>
        public bool RemoveCharacterByName(string characterName)
        {
            foreach (var member in _party.Members)
            {
                if (member.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase))
                {
                    return RemoveCharacter(member);
                }
            }

            Console.WriteLine($"Error: Character '{characterName}' not found in party");
            return false;
        }

        /// <summary>
        /// Get all current party members
        /// </summary>
        public List<Character> GetPartyMembers()
        {
            return new List<Character>(_party.Members);
        }

        /// <summary>
        /// Check if the party is at minimum size
        /// </summary>
        public bool IsAtMinimumSize()
        {
            return PartySize == MinPartySize;
        }

        /// <summary>
        /// Check if the party is at maximum size
        /// </summary>
        public bool IsAtMaximumSize()
        {
            return PartySize == MaxPartySize;
        }

        /// <summary>
        /// Add a quest to the party's quest log
        /// </summary>
        public void AddQuest(string questDescription)
        {
            if (!string.IsNullOrWhiteSpace(questDescription))
            {
                _quests.Add(questDescription);
                Console.WriteLine($"✓ Quest added: {questDescription}");
            }
        }

        /// <summary>
        /// Complete a quest by index
        /// </summary>
        public bool CompleteQuest(int questIndex)
        {
            if (questIndex >= 0 && questIndex < _quests.Count)
            {
                string completedQuest = _quests[questIndex];
                _quests.RemoveAt(questIndex);
                Console.WriteLine($"✓ Quest completed: {completedQuest}");
                return true;
            }

            Console.WriteLine("Error: Invalid quest index");
            return false;
        }

        /// <summary>
        /// Add an alliance to the party
        /// </summary>
        public void AddAlliance(string allianceName)
        {
            if (!string.IsNullOrWhiteSpace(allianceName) && !_alliances.Contains(allianceName))
            {
                _alliances.Add(allianceName);
                Console.WriteLine($"✓ Alliance formed with {allianceName}");
            }
        }

        /// <summary>
        /// Remove an alliance
        /// </summary>
        public bool RemoveAlliance(string allianceName)
        {
            if (_alliances.Remove(allianceName))
            {
                Console.WriteLine($"✓ Alliance with {allianceName} has ended");
                return true;
            }

            Console.WriteLine($"Error: {allianceName} is not in alliance list");
            return false;
        }

        /// <summary>
        /// Add an enemy to the party's enemy list
        /// </summary>
        public void AddEnemy(string enemyName)
        {
            if (!string.IsNullOrWhiteSpace(enemyName) && !_enemies.Contains(enemyName))
            {
                _enemies.Add(enemyName);
                Console.WriteLine($"✓ {enemyName} added to enemy list");
            }
        }

        /// <summary>
        /// Remove an enemy
        /// </summary>
        public bool RemoveEnemy(string enemyName)
        {
            if (_enemies.Remove(enemyName))
            {
                Console.WriteLine($"✓ {enemyName} removed from enemy list");
                return true;
            }

            Console.WriteLine($"Error: {enemyName} is not in enemy list");
            return false;
        }

        /// <summary>
        /// Print comprehensive party status including metadata
        /// </summary>
        public void PrintPartyStatus()
        {
            Console.WriteLine($"\n═══════════════════════════════════════════");
            Console.WriteLine($"  PARTY: {_partyName}");
            Console.WriteLine($"═══════════════════════════════════════════");
            Console.WriteLine($"  Members: {PartySize}/{MaxPartySize}");
            Console.WriteLine();

            Console.WriteLine("  Party Members:");
            var members = GetPartyMembers();
            for (int i = 0; i < members.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {members[i].Name} (Level {members[i].Level}, HP: {members[i].HitPointsCurrent}/{members[i].HitPointsMax})");
            }

            if (_quests.Count > 0)
            {
                Console.WriteLine("\n  Active Quests:");
                for (int i = 0; i < _quests.Count; i++)
                {
                    Console.WriteLine($"    {i + 1}. {_quests[i]}");
                }
            }

            if (_alliances.Count > 0)
            {
                Console.WriteLine("\n  Alliances:");
                foreach (var alliance in _alliances)
                {
                    Console.WriteLine($"    • {alliance}");
                }
            }

            if (_enemies.Count > 0)
            {
                Console.WriteLine("\n  Enemies:");
                foreach (var enemy in _enemies)
                {
                    Console.WriteLine($"    • {enemy}");
                }
            }

            Console.WriteLine($"\n═══════════════════════════════════════════\n");
        }
    }
}
