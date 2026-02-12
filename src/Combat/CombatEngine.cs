using System;
using System.Collections.Generic;
using System.Linq;
using PoolOfRadiance.Characters;

namespace PoolOfRadiance.Combat
{
    /// <summary>
    /// Manages turn-based tactical combat encounters
    /// </summary>
    public class CombatEngine
    {
        private List<Combatant> _combatants;
        private Queue<Combatant> _turnOrder;
        private Combatant? _currentCombatant;
        private CombatState _state;
        private CombatGrid _grid;
        
        public bool IsActive { get; private set; }
        
        public CombatEngine()
        {
            _combatants = new List<Combatant>();
            _turnOrder = new Queue<Combatant>();
            _state = CombatState.NotStarted;
            _grid = new CombatGrid(20, 20); // 20x20 combat grid
        }
        
        public void StartCombat(Party playerParty, List<Character> enemies)
        {
            Console.WriteLine("===== COMBAT BEGINS =====");
            
            _combatants.Clear();
            _turnOrder.Clear();
            
            // Add player characters
            foreach (var member in playerParty.Members.Where(m => m.IsAlive))
            {
                _combatants.Add(new Combatant(member, true));
            }
            
            // Add enemies
            foreach (var enemy in enemies)
            {
                _combatants.Add(new Combatant(enemy, false));
            }
            
            // Roll initiative and create turn order
            RollInitiative();
            
            IsActive = true;
            _state = CombatState.InProgress;
            
            StartNextTurn();
        }
        
        private void RollInitiative()
        {
            Random rand = new Random();
            
            foreach (var combatant in _combatants)
            {
                // Roll 1d10 + DEX modifier
                int roll = rand.Next(1, 11);
                combatant.Initiative = roll + combatant.Character.Stats.Dexterity;
            }
            
            // Sort by initiative (highest first)
            var ordered = _combatants.OrderByDescending(c => c.Initiative).ToList();
            
            foreach (var combatant in ordered)
            {
                _turnOrder.Enqueue(combatant);
                Console.WriteLine($"{combatant.Character.Name} rolled {combatant.Initiative} initiative");
            }
        }
        
        public void StartNextTurn()
        {

_currentCombatant = _turnOrder.Dequeue();

// Safety check (shouldn't be null, but good practice)
if (_currentCombatant == null) return;

            if (_turnOrder.Count == 0)
            {
                // Round complete, check for combat end
                if (IsCombatOver())
                {
                    EndCombat();
                    return;
                }
                
                // Start new round
                RollInitiative();
            }
            
            _currentCombatant = _turnOrder.Dequeue();
            
            Console.WriteLine($"{_currentCombatant.Character.Name}'s turn!");
            
            if (_currentCombatant.IsPlayerControlled)
            {
                _state = CombatState.PlayerTurn;
                // Wait for player input
            }
            else
            {
                _state = CombatState.EnemyTurn;
                ExecuteEnemyTurn();
            }
        }
        
        private void ExecuteEnemyTurn()
        {

            if (_currentCombatant == null) return; // Safety check

            if (_currentCombatant == null) return; // Safety check

            // Simple AI: attack nearest player character
            var playerCombatants = _combatants.Where(c => c.IsPlayerControlled && c.Character.IsAlive).ToList();
            
            if (playerCombatants.Count > 0)
            {
                var target = playerCombatants[0]; // Simplified - just pick first
                
                Console.WriteLine($"{_currentCombatant.Character.Name} attacks {target.Character.Name}!");
                
                if (_currentCombatant.Character.AttackTarget(target.Character))
                {
                    Console.WriteLine($"Hit! {target.Character.Name} takes damage! HP: {target.Character.HitPointsCurrent}/{target.Character.HitPointsMax}");
                    
                    if (!target.Character.IsAlive)
                    {
                        Console.WriteLine($"{target.Character.Name} has fallen!");
                    }
                }
                else
                {
                    Console.WriteLine("Miss!");
                }
            }
            
            // Wait a moment for readability
            System.Threading.Thread.Sleep(1000);
            
            StartNextTurn();
        }
        
        public void PlayerAttack(Combatant target)
        {
            if (_state != CombatState.PlayerTurn) return;
            if (_currentCombatant == null || !_currentCombatant.IsPlayerControlled) return;
            
            Console.WriteLine($"{_currentCombatant.Character.Name} attacks {target.Character.Name}!");
            
            if (_currentCombatant.Character.AttackTarget(target.Character))
            {
                Console.WriteLine($"Hit! {target.Character.Name} takes damage! HP: {target.Character.HitPointsCurrent}/{target.Character.HitPointsMax}");
                
                if (!target.Character.IsAlive)
                {
                    Console.WriteLine($"{target.Character.Name} has been defeated!");
                }
            }
            else
            {
                Console.WriteLine("Miss!");
            }
            
            _currentCombatant.HasActed = true;
            StartNextTurn();
        }
        
        public void PlayerMove(int x, int y)
        {
            if (_state != CombatState.PlayerTurn) return;
            if (_currentCombatant == null || !_currentCombatant.IsPlayerControlled) return;
            
            // Check if movement is valid
            if (_grid.CanMoveTo(x, y))
            {
                _currentCombatant.X = x;
                _currentCombatant.Y = y;
                Console.WriteLine($"{_currentCombatant.Character.Name} moves to ({x}, {y})");
            }
        }
        
        public void PlayerCastSpell(Spell spell, Combatant target)
        {
            if (_state != CombatState.PlayerTurn) return;
            if (_currentCombatant == null || !_currentCombatant.IsPlayerControlled) return;
            
            Console.WriteLine($"{_currentCombatant.Character.Name} casts {spell.Name}!");
            
            // Apply spell effects (simplified)
            // In real implementation, would have spell-specific logic
            
            _currentCombatant.HasActed = true;
            StartNextTurn();
        }
        
        public void PlayerEndTurn()
        {

      if (_state != CombatState.PlayerTurn) return;
    if (_currentCombatant == null) return; // Safety check
    
    _currentCombatant.HasActed = true;
    StartNextTurn();
        }
        
        private bool IsCombatOver()
        {
            bool anyPlayersAlive = _combatants.Any(c => c.IsPlayerControlled && c.Character.IsAlive);
            bool anyEnemiesAlive = _combatants.Any(c => !c.IsPlayerControlled && c.Character.IsAlive);
            
            return !anyPlayersAlive || !anyEnemiesAlive;
        }
        
        private void EndCombat()
        {
            IsActive = false;
            _state = CombatState.Ended;
            
            bool playerVictory = _combatants.Any(c => c.IsPlayerControlled && c.Character.IsAlive);
            
            if (playerVictory)
            {
                Console.WriteLine("===== VICTORY! =====");
                
                // Calculate experience and treasure
                int totalXP = _combatants.Where(c => !c.IsPlayerControlled && !c.Character.IsAlive)
                                        .Sum(c => c.Character.Level * 100); // Simplified XP calc
                
                int gold = new Random().Next(10, 100);
                
                Console.WriteLine($"Experience gained: {totalXP}");
                Console.WriteLine($"Gold found: {gold}");
            }
            else
            {
                Console.WriteLine("===== DEFEAT =====");
                Console.WriteLine("Game Over");
            }
            
            Console.WriteLine("===================");
        }
        
        public List<Combatant> GetEnemies()
        {
            return _combatants.Where(c => !c.IsPlayerControlled && c.Character.IsAlive).ToList();
        }
        
        public List<Combatant> GetPlayerCombatants()
        {
            return _combatants.Where(c => c.IsPlayerControlled).ToList();
        }
    }
    
    public class Combatant
    {
        public Character Character { get; }
        public bool IsPlayerControlled { get; }
        public int Initiative { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasActed { get; set; }
        
        public Combatant(Character character, bool isPlayer)
        {
            Character = character;
            IsPlayerControlled = isPlayer;
            Initiative = 0;
            HasActed = false;
        }
    }
    
    public class CombatGrid
    {
        private bool[,] _occupiedCells;
        public int Width { get; }
        public int Height { get; }
        
        public CombatGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _occupiedCells = new bool[width, height];
        }
        
        public bool CanMoveTo(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
                
            return !_occupiedCells[x, y];
        }
        
        public void SetOccupied(int x, int y, bool occupied)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                _occupiedCells[x, y] = occupied;
        }
    }
    
    public enum CombatState
    {
        NotStarted,
        InProgress,
        PlayerTurn,
        EnemyTurn,
        Ended
    }
}
