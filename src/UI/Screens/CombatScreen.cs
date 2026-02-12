using System;
using System.Collections.Generic;
using System.Linq;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Combat;

namespace PoolOfRadiance.UI.Screens
{
    /// <summary>
    /// Tactical combat screen - shows character-by-character combat
    /// Similar to Pool of Radiance's tactical combat view
    /// </summary>
    public class CombatScreen : GameScreen
    {
        private Party _party;
        private List<Character> _enemies;
        private CombatEngine _combat;
        private string _location;
        private List<Combatant> _turnOrder = new List<Combatant>();
        private int _currentTurn;
        private Combatant? _activeCombatant;
        private int _selectedTarget;
        private CombatPhase _phase;
        private List<string> _combatLog;
        
        private enum CombatPhase
        {
            InitiativeRoll,
            SelectAction,
            SelectTarget,
            ExecuteAction,
            CheckVictory,
            Victory,
            Defeat
        }
        
        public CombatScreen(ScreenManager screenManager, Party party, List<Character> enemies, string location) 
            : base(screenManager)
        {
            _party = party;
            _enemies = enemies;
            _location = location;
            _combat = new CombatEngine();
            _combatLog = new List<string>();
            _selectedTarget = 0;
            _phase = CombatPhase.InitiativeRoll;
            
        }
        
        public override string ScreenName => "Combat";
        
        public override void OnEnter()
        {
            Console.Clear();
            AddLog($"Combat begins in {_location}!");
            
            // Start combat
            _combat.StartCombat(_party, _enemies);
            _turnOrder = new List<Combatant>();
            
            // Get all combatants and sort by initiative
            var allCombatants = _combat.GetPlayerCombatants();
            allCombatants.AddRange(_combat.GetEnemies());
            _turnOrder = allCombatants.OrderByDescending(c => c.Initiative).ToList();
            
            foreach (var c in _turnOrder)
            {
                AddLog($"{c.Character.Name} rolled initiative: {c.Initiative}");
            }
            
            _currentTurn = 0;
            _phase = CombatPhase.SelectAction;
            
            System.Threading.Thread.Sleep(2000);
        }
        
        public override void Update()
        {
            if (_phase == CombatPhase.Victory || _phase == CombatPhase.Defeat)
            {
                var key = Console.ReadKey(true);
                _screenManager.PopScreen();
                return;
            }
            
            // Get current combatant
            if (_currentTurn >= _turnOrder.Count)
            {
                // Round complete
                _currentTurn = 0;
                AddLog("--- New Round ---");
            }
            
            _activeCombatant = _turnOrder[_currentTurn];
            
            // Skip dead combatants
            if (!_activeCombatant.Character.IsAlive)
            {
                _currentTurn++;
                return;
            }
            
            if (_activeCombatant.IsPlayerControlled)
            {
                HandlePlayerTurn();
            }
            else
            {
                HandleEnemyTurn();
            }
        }
        
        private void HandlePlayerTurn()
        {
            if (_activeCombatant == null) return; // Safety check

            if (_phase == CombatPhase.SelectAction)
            {
                var key = Console.ReadKey(true);
                
                switch (key.KeyChar)
                {
                    case '1': // Attack
                        _phase = CombatPhase.SelectTarget;
                        _selectedTarget = 0;
                        break;
                    case '2': // Delay
                        AddLog($"{_activeCombatant.Character.Name} delays their turn.");
                        NextTurn();
                        break;
                    case '3': // Pass
                        AddLog($"{_activeCombatant.Character.Name} passes.");
                        NextTurn();
                        break;
                }
            }
            else if (_phase == CombatPhase.SelectTarget)
            {
                var enemies = _combat.GetEnemies();
                if (enemies.Count == 0)
                {
                    _phase = CombatPhase.Victory;
                    return;
                }
                
                var key = Console.ReadKey(true);
                
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedTarget = Math.Max(0, _selectedTarget - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedTarget = Math.Min(enemies.Count - 1, _selectedTarget + 1);
                        break;
                    case ConsoleKey.Enter:
                        // Execute attack
                        ExecuteAttack(enemies[_selectedTarget]);
                        break;
                    case ConsoleKey.Escape:
                        _phase = CombatPhase.SelectAction;
                        break;
                }
            }
        }
        
        private void HandleEnemyTurn()
        {
            if (_activeCombatant == null) return; // Safety check
            System.Threading.Thread.Sleep(500);
            
            var playerTargets = _combat.GetPlayerCombatants().Where(c => c.Character.IsAlive).ToList();
            if (playerTargets.Count == 0)
            {
                _phase = CombatPhase.Defeat;
                return;
            }
            
            // Simple AI: attack random player
            var target = playerTargets[new Random().Next(playerTargets.Count)];
            ExecuteAttack(target);
        }
        
        private void ExecuteAttack(Combatant target)
        {
            if (_activeCombatant == null) return; // Safety check
            
            AddLog($"{_activeCombatant.Character.Name} attacks {target.Character.Name}!");
            
            bool hit = _activeCombatant.Character.AttackTarget(target.Character);
            
            if (hit)
            {
                AddLog($"  HIT! {target.Character.Name} HP: {target.Character.HitPointsCurrent}/{target.Character.HitPointsMax}");
                
                if (!target.Character.IsAlive)
                {
                    AddLog($"  {target.Character.Name} has been defeated!");
                }
            }
            else
            {
                AddLog($"  MISS!");
            }
            
            System.Threading.Thread.Sleep(1000);
            CheckVictoryConditions();
            NextTurn();
        }
        
        private void CheckVictoryConditions()
        {
            bool anyPlayersAlive = _combat.GetPlayerCombatants().Any(c => c.Character.IsAlive);
            bool anyEnemiesAlive = _combat.GetEnemies().Any(c => c.Character.IsAlive);
            
            if (!anyEnemiesAlive)
            {
                _phase = CombatPhase.Victory;
                AddLog("");
                AddLog("=== VICTORY! ===");
                
                // Award XP and gold
                int xp = _enemies.Count * 50;
                int gold = new Random().Next(10, 50) * _enemies.Count;
                
                _party.DistributeExperience(xp);
                _party.DistributeGold(gold);
                
                AddLog($"Party gains {xp} experience!");
                AddLog($"Party finds {gold} gold!");
            }
            else if (!anyPlayersAlive)
            {
                _phase = CombatPhase.Defeat;
                AddLog("");
                AddLog("=== DEFEAT ===");
                AddLog("Your party has been defeated...");
            }
        }
        
        private void NextTurn()
        {
            _currentTurn++;
            _phase = CombatPhase.SelectAction;
        }
        
        private void AddLog(string message)
        {
            _combatLog.Add(message);
            if (_combatLog.Count > 8)
            {
                _combatLog.RemoveAt(0);
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            
            // Header
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ COMBAT - {_location,-67} ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            
            // Combat grid view
            RenderCombatGrid();
            
            Console.WriteLine();
            
            // Character status panels
            RenderPartyStatus();
            Console.WriteLine();
            RenderEnemyStatus();
            
            Console.WriteLine();
            
            // Combat log
            RenderCombatLog();
            
            Console.WriteLine();
            
            // Active combatant and actions
            if (_activeCombatant != null && _phase != CombatPhase.Victory && _phase != CombatPhase.Defeat)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  >> {_activeCombatant.Character.Name}'s Turn <<");
                Console.ResetColor();
                
                if (_activeCombatant.IsPlayerControlled)
                {
                    if (_phase == CombatPhase.SelectAction)
                    {
                        Console.WriteLine("  Actions: [1] Attack  [2] Delay  [3] Pass");
                    }
                    else if (_phase == CombatPhase.SelectTarget)
                    {
                        Console.WriteLine("  Select target (↑↓) and press ENTER to attack, ESC to cancel");
                    }
                }
            }
            else if (_phase == CombatPhase.Victory || _phase == CombatPhase.Defeat)
            {
                Console.WriteLine("  Press any key to continue...");
            }
        }
        
        private void RenderCombatGrid()
        {
            Console.WriteLine("  BATTLEFIELD:");
            Console.WriteLine("  ┌─────────────────────────────────────────┐");
            
            // Simple grid representation
            var players = _combat.GetPlayerCombatants().Where(c => c.Character.IsAlive).ToList();
            var enemies = _combat.GetEnemies().Where(c => c.Character.IsAlive).ToList();
            
            // Show enemies
            Console.Write("  │ ENEMIES:  ");
            for (int i = 0; i < enemies.Count && i < 5; i++)
            {
                if (_phase == CombatPhase.SelectTarget && i == _selectedTarget)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[{enemies[i].Character.Name[0]}] ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($" {enemies[i].Character.Name[0]}  ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine(new string(' ', 30 - (enemies.Count * 4)) + "│");
            
            Console.WriteLine("  │                                         │");
            Console.WriteLine("  │              [BATTLEFIELD]              │");
            Console.WriteLine("  │                                         │");
            
            // Show party
            Console.Write("  │ PARTY:    ");
            for (int i = 0; i < players.Count && i < 5; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" {players[i].Character.Name[0]}  ");
                Console.ResetColor();
            }
            Console.WriteLine(new string(' ', 30 - (players.Count * 4)) + "│");
            
            Console.WriteLine("  └─────────────────────────────────────────┘");
        }
        
        private void RenderPartyStatus()
        {
            Console.WriteLine("  ╔═══════════════════════════════════════╗");
            Console.WriteLine("  ║ PARTY STATUS                          ║");
            Console.WriteLine("  ╠═══════════════════════════════════════╣");
            
            foreach (var c in _combat.GetPlayerCombatants())
            {
                var ch = c.Character;
                string status = ch.IsAlive ? "OK" : "DEAD";
                Console.ForegroundColor = ch.IsAlive ? ConsoleColor.Green : ConsoleColor.DarkGray;
                Console.WriteLine($"  ║ {ch.Name,-12} Lv{ch.Level} HP:{ch.HitPointsCurrent,3}/{ch.HitPointsMax,3} [{status}]  ║");
                Console.ResetColor();
            }
            
            Console.WriteLine("  ╚═══════════════════════════════════════╝");
        }
        
        private void RenderEnemyStatus()
        {
            Console.WriteLine("  ╔═══════════════════════════════════════╗");
            Console.WriteLine("  ║ ENEMY STATUS                          ║");
            Console.WriteLine("  ╠═══════════════════════════════════════╣");
            
            var enemies = _combat.GetEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                var ch = enemies[i].Character;
                string status = ch.IsAlive ? "OK" : "DEAD";
                
                if (_phase == CombatPhase.SelectTarget && i == _selectedTarget)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ║>");
                }
                else
                {
                    Console.Write("  ║ ");
                }
                
                Console.ForegroundColor = ch.IsAlive ? ConsoleColor.Red : ConsoleColor.DarkGray;
                Console.WriteLine($"{ch.Name,-12} Lv{ch.Level} HP:{ch.HitPointsCurrent,3}/{ch.HitPointsMax,3} [{status}]  ║");
                Console.ResetColor();
            }
            
            Console.WriteLine("  ╚═══════════════════════════════════════╝");
        }
        
        private void RenderCombatLog()
        {
            Console.WriteLine("  ╔════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║ COMBAT LOG                                                                 ║");
            Console.WriteLine("  ╠════════════════════════════════════════════════════════════════════════════╣");
            
            foreach (var log in _combatLog)
            {
                Console.WriteLine($"  ║ {log,-74} ║");
            }
            
            // Fill remaining lines
            for (int i = _combatLog.Count; i < 8; i++)
            {
                Console.WriteLine("  ║" + new string(' ', 76) + "║");
            }
            
            Console.WriteLine("  ╚════════════════════════════════════════════════════════════════════════════╝");
        }
    }
}
