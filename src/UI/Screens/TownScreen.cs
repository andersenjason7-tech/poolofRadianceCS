using System;
using PoolOfRadiance.Characters;

namespace PoolOfRadiance.UI.Screens
{
    /// <summary>
    /// Town exploration screen - shows available locations and party status
    /// Similar to the original Pool of Radiance town view
    /// </summary>
    public class TownScreen : GameScreen
    {
        private Party _party;
        private string _townName;
        private string[] _locations;
     //   private int _selectedLocation ;
        
        public TownScreen(ScreenManager screenManager, Party party, string townName) 
            : base(screenManager)
        {
            _party = party;
            _townName = townName;
            _locations = new[]
            {
                "1. Training Hall",
                "2. Temple (Heal & Rest)",
                "3. Shop",
                "4. Tavern (Rumors & Quests)",
                "5. City Hall",
                "6. Explore Sewers",
                "7. Leave Town",
                "8. View Party",
                "9. Camp (Rest)"
            };
          //  _selectedLocation = 0;
        }
        
        public override string ScreenName => "Town Exploration";
        
        public override void OnEnter()
        {
            Console.Clear();
            Console.WriteLine($"\n=== Welcome to {_townName} ===\n");
        }
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            
            switch (key.KeyChar)
            {
                case '1':
                    _screenManager.PushScreen(new TrainingHallScreen(_screenManager, _party));
                    break;
                case '2':
                    _screenManager.PushScreen(new TempleScreen(_screenManager, _party));
                    break;
                case '3':
                    _screenManager.PushScreen(new ShopScreen(_screenManager, _party));
                    break;
                case '4':
                    _screenManager.PushScreen(new TavernScreen(_screenManager, _party));
                    break;
                case '5':
                    Console.WriteLine("\n[City Hall - Quest Board - Coming Soon]\n");
                    System.Threading.Thread.Sleep(1500);
                    break;
                case '6':
                    // Random encounter when exploring sewers
                    _screenManager.PushScreen(new ExplorationScreen(_screenManager, _party, "Sewers"));
                    break;
                case '7':
                    Console.WriteLine("\n[Leaving town...]\n");
                    _screenManager.PopScreen();
                    break;
                case '8':
                    _screenManager.PushScreen(new PartyStatusScreen(_screenManager, _party));
                    break;
                case '9':
                    RestParty();
                    break;
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            
            // Town header
            DrawBox(0, 0, 80, 3);
            Console.SetCursorPosition(2, 1);
            Console.Write($"TOWN: {_townName}");
            Console.SetCursorPosition(50, 1);
            Console.Write($"Party Gold: {_party.Gold}");
            
            // Party status
            DrawBox(0, 4, 40, 12);
            Console.SetCursorPosition(2, 5);
            Console.WriteLine("PARTY STATUS:");
            
            int row = 6;
            foreach (var member in _party.Members)
            {
                Console.SetCursorPosition(2, row);
                string status = member.IsAlive ? "OK" : "DEAD";
                Console.WriteLine($"{member.Name,-12} Lv{member.Level} HP:{member.HitPointsCurrent,3}/{member.HitPointsMax,3} [{status}]");
                row++;
            }
            
            // Available locations
            DrawBox(42, 4, 38, 20);
            Console.SetCursorPosition(44, 5);
            Console.WriteLine("WHERE WILL YOU GO?");
            
            row = 7;
            foreach (var location in _locations)
            {
                Console.SetCursorPosition(44, row);
                Console.WriteLine(location);
                row++;
            }
            
            // Instructions
            DrawBox(0, 17, 40, 7);
            Console.SetCursorPosition(2, 18);
            Console.WriteLine("COMMANDS:");
            Console.SetCursorPosition(2, 19);
            Console.WriteLine("1-9: Select location");
            Console.SetCursorPosition(2, 20);
            Console.WriteLine("8: View full party details");
            Console.SetCursorPosition(2, 21);
            Console.WriteLine("9: Rest party (heal 1 HP)");
        }
        
        private void RestParty()
        {
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("\nThe party rests for the night...");
            _party.Rest();
            System.Threading.Thread.Sleep(1500);
        }
        
        private void DrawBox(int x, int y, int width, int height)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("┌" + new string('─', width - 2) + "┐");
            
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│" + new string(' ', width - 2) + "│");
            }
            
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write("└" + new string('─', width - 2) + "┘");
        }
    }
}
