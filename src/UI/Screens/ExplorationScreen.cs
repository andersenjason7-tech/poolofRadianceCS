using System;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Combat;

namespace PoolOfRadiance.UI.Screens
{
    /// <summary>
    /// Exploration screen - navigate through dungeons/sewers/wilderness
    /// Shows a simple map view and handles random encounters
    /// </summary>
    public class ExplorationScreen : GameScreen
    {
        private Party _party;
        private string _areaName;
        private int _mapWidth = 20;
        private int _mapHeight = 15;
        private char[,] _map = new char[20, 15];
        private int _partyX;
        private int _partyY;
        private Random _random;
        private int _steps;
        
        public ExplorationScreen(ScreenManager screenManager, Party party, string areaName) 
            : base(screenManager)
        {
            _party = party;
            _areaName = areaName;
            _random = new Random();
            GenerateSimpleMap();
        }
        
        public override string ScreenName => $"Exploring {_areaName}";
        
        public override void OnEnter()
        {
            Console.Clear();
            Console.WriteLine($"\n=== Entering {_areaName} ===\n");
            System.Threading.Thread.Sleep(1000);
        }
        
        private void GenerateSimpleMap()
        {
            _map = new char[_mapWidth, _mapHeight];
            
            // Fill with floor tiles
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight - 1)
                    {
                        _map[x, y] = '#'; // Wall
                    }
                    else
                    {
                        _map[x, y] = '.'; // Floor
                    }
                }
            }
            
            // Add some random obstacles
            for (int i = 0; i < 15; i++)
            {
                int x = _random.Next(2, _mapWidth - 2);
                int y = _random.Next(2, _mapHeight - 2);
                _map[x, y] = '#';
            }
            
            // Place some doors
            _map[10, 0] = '+'; // Exit at top
            _map[5, 7] = '+';  // Door
            _map[15, 7] = '+'; // Door
            
            // Starting position
            _partyX = _mapWidth / 2;
            _partyY = _mapHeight - 2;
            _map[_partyX, _partyY] = '@';
            
            _steps = 0;
        }
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            
            int newX = _partyX;
            int newY = _partyY;
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    newY--;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    newY++;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    newX--;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    newX++;
                    break;
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    // Leave exploration
                    _screenManager.PopScreen();
                    return;
                case ConsoleKey.C:
                    // View character status
                    _screenManager.PushScreen(new PartyStatusScreen(_screenManager, _party));
                    return;
            }
            
            // Check if move is valid
            if (newX >= 0 && newX < _mapWidth && newY >= 0 && newY < _mapHeight)
            {
                char targetTile = _map[newX, newY];
                
                if (targetTile == '.' || targetTile == '+' || targetTile == '@')
                {
                    // Valid move
                    _map[_partyX, _partyY] = '.';
                    _partyX = newX;
                    _partyY = newY;
                    _map[_partyX, _partyY] = '@';
                    _steps++;
                    
                    // Check for exit
                    if (targetTile == '+' && newY == 0)
                    {
                        Console.SetCursorPosition(0, 22);
                        Console.WriteLine("\nYou found the exit!");
                        System.Threading.Thread.Sleep(1500);
                        _screenManager.PopScreen();
                        return;
                    }
                    
                    // Random encounter check (10% chance per step)
                    if (_random.Next(100) < 10)
                    {
                        TriggerEncounter();
                    }
                }
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            
            // Header
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ {_areaName,-76} ║");
            Console.WriteLine($"╚════════════════════════════════════════════════════════════════════════════════╝");
            
            // Render map
            Console.SetCursorPosition(0, 4);
            for (int y = 0; y < _mapHeight; y++)
            {
                Console.Write("  ");
                for (int x = 0; x < _mapWidth; x++)
                {
                    char tile = _map[x, y];
                    
                    // Color coding
                    if (tile == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(tile);
                        Console.ResetColor();
                    }
                    else if (tile == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(tile);
                        Console.ResetColor();
                    }
                    else if (tile == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(tile);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(tile);
                    }
                }
                Console.WriteLine();
            }
            
            // Party status
            Console.WriteLine("\n  ╔════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║ PARTY STATUS                                                               ║");
            Console.WriteLine("  ╠════════════════════════════════════════════════════════════════════════════╣");
            
            foreach (var member in _party.Members)
            {
                string status = member.IsAlive ? "OK" : "DEAD";
                Console.ForegroundColor = member.IsAlive ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($"  ║ {member.Name,-12}");
                Console.ResetColor();
                Console.WriteLine($" Lv{member.Level}  HP: {member.HitPointsCurrent,3}/{member.HitPointsMax,3}  [{status}]                        ║");
            }
            
            Console.WriteLine("  ╚════════════════════════════════════════════════════════════════════════════╝");
            
            // Legend and controls
            Console.WriteLine("\n  Legend: @ = Party  # = Wall  . = Floor  + = Door/Exit");
            Console.WriteLine("  Controls: Arrow Keys/WASD = Move  |  C = Party Status  |  Q/ESC = Exit");
            Console.WriteLine($"  Steps: {_steps}");
        }
        
        private void TriggerEncounter()
        {
            Console.SetCursorPosition(0, 22);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  *** ENCOUNTER! Monsters attack! ***");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
            
            // Create random enemies
            var enemies = GenerateEnemies();
            
            // Switch to combat screen
            _screenManager.PushScreen(new CombatScreen(_screenManager, _party, enemies, _areaName));
        }
        
        private System.Collections.Generic.List<Character> GenerateEnemies()
        {
            var enemies = new System.Collections.Generic.List<Character>();
            
            int numEnemies = _random.Next(1, 5); // 1-4 enemies
            
            for (int i = 0; i < numEnemies; i++)
            {
                string[] enemyTypes = { "Goblin", "Kobold", "Skeleton", "Giant Rat", "Orc" };
                string enemyType = enemyTypes[_random.Next(enemyTypes.Length)];
                
                var enemy = new Character($"{enemyType} {i + 1}", CharacterRace.Human, CharacterClass.Fighter);
                enemy.Level = 1;
                enemy.Stats.Strength = _random.Next(8, 15);
                enemy.Stats.Dexterity = _random.Next(8, 15);
                enemy.Stats.Constitution = _random.Next(8, 15);
                enemy.HitPointsMax = _random.Next(4, 10);
                enemy.HitPointsCurrent = enemy.HitPointsMax;
                enemy.CalculateDerivedStats();
                
                enemies.Add(enemy);
            }
            
            return enemies;
        }
    }
}
