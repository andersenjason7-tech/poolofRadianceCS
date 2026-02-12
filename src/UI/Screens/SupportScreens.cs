using System;
using PoolOfRadiance.Characters;

namespace PoolOfRadiance.UI.Screens
{
    /// <summary>
    /// Party status screen - detailed view of all party members
    /// </summary>
    public class PartyStatusScreen : GameScreen
    {
        private Party _party;
        
        public PartyStatusScreen(ScreenManager screenManager, Party party) 
            : base(screenManager)
        {
            _party = party;
        }
        
        public override string ScreenName => "Party Status";
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            _screenManager.PopScreen();
        }
        
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ PARTY STATUS - DETAILED VIEW                                                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            
            Console.WriteLine($"  Party Gold: {_party.Gold}");
            Console.WriteLine($"  Location: ({_party.X}, {_party.Y})");
            Console.WriteLine();
            
            foreach (var member in _party.Members)
            {
                Console.WriteLine("  " + new string('─', 76));
                Console.ForegroundColor = member.IsAlive ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {member.Name} - Level {member.Level} {member.Race} {member.Class}");
                Console.ResetColor();
                
                Console.WriteLine($"  HP: {member.HitPointsCurrent}/{member.HitPointsMax}  |  " +
                                $"AC: {member.ArmorClass}  |  THAC0: {member.THAC0}  |  " +
                                $"XP: {member.ExperiencePoints}");
                
                Console.WriteLine($"  {member.Stats}");
                
                string status = member.IsAlive ? "Healthy" : "DEAD";
                Console.WriteLine($"  Status: {status}");
                Console.WriteLine();
            }
            
            Console.WriteLine("  " + new string('─', 76));
            Console.WriteLine("\n  Press any key to return...");
        }
    }
    
    /// <summary>
    /// Temple screen - healing and resurrection services
    /// </summary>
    public class TempleScreen : GameScreen
    {
        private Party _party;
        
        public TempleScreen(ScreenManager screenManager, Party party) 
            : base(screenManager)
        {
            _party = party;
        }
        
        public override string ScreenName => "Temple";
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            
            switch (key.KeyChar)
            {
                case '1': // Heal all
                    HealParty();
                    break;
                case '2': // Rest
                    _party.Rest();
                    Console.WriteLine("\n  The party rests and recovers...");
                    System.Threading.Thread.Sleep(1500);
                    break;
                case '3': // Leave
                    _screenManager.PopScreen();
                    break;
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ TEMPLE OF HEALING                                                              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  'Welcome to the temple. How may we serve you?'");
            Console.WriteLine();
            Console.WriteLine("  Services:");
            Console.WriteLine("  [1] Heal All Party Members (50 gold)");
            Console.WriteLine("  [2] Rest (Free)");
            Console.WriteLine("  [3] Leave");
            Console.WriteLine();
            Console.WriteLine($"  Party Gold: {_party.Gold}");
        }
        
        private void HealParty()
        {
            if (_party.Gold >= 50)
            {
                _party.SpendGold(50);
                
                foreach (var member in _party.Members)
                {
                    if (member.IsAlive)
                    {
                        member.HitPointsCurrent = member.HitPointsMax;
                    }
                }
                
                Console.WriteLine("\n  The priests channel divine energy. Your party is fully healed!");
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("\n  'I'm sorry, you don't have enough gold for that service.'");
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
    
    /// <summary>
    /// Shop screen - buy and sell items
    /// </summary>
    public class ShopScreen : GameScreen
    {
        private Party _party;
        
        public ShopScreen(ScreenManager screenManager, Party party) 
            : base(screenManager)
        {
            _party = party;
        }
        
        public override string ScreenName => "Shop";
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            
            switch (key.KeyChar)
            {
                case '1': // Buy sword
                    BuyItem("Longsword", 15);
                    break;
                case '2': // Buy armor
                    BuyItem("Chain Mail", 75);
                    break;
                case '3': // Buy potion
                    BuyItem("Healing Potion", 10);
                    break;
                case '4': // Leave
                    _screenManager.PopScreen();
                    break;
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ GENERAL STORE                                                                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  'Welcome adventurers! Take a look at my wares.'");
            Console.WriteLine();
            Console.WriteLine("  For Sale:");
            Console.WriteLine("  [1] Longsword        - 15 gold");
            Console.WriteLine("  [2] Chain Mail       - 75 gold");
            Console.WriteLine("  [3] Healing Potion   - 10 gold");
            Console.WriteLine("  [4] Leave Shop");
            Console.WriteLine();
            Console.WriteLine($"  Party Gold: {_party.Gold}");
        }
        
        private void BuyItem(string itemName, int cost)
        {
            if (_party.Gold >= cost)
            {
                _party.SpendGold(cost);
                Console.WriteLine($"\n  You purchased {itemName} for {cost} gold!");
                // TODO: Actually add item to inventory
                System.Threading.Thread.Sleep(1500);
            }
            else
            {
                Console.WriteLine("\n  'Sorry, you can't afford that.'");
                System.Threading.Thread.Sleep(1500);
            }
        }
    }
    
    /// <summary>
    /// Tavern screen - rumors and quests
    /// </summary>
    public class TavernScreen : GameScreen
    {
        private Party _party;
        private string[] _rumors = new[]
        {
            "I heard the sewers are crawling with monsters these days...",
            "The old temple in the ruins holds great treasure, they say.",
            "Beware the kobolds in the eastern caves!",
            "A powerful wizard lives in the tower to the north.",
            "Strange lights have been seen in the graveyard at night."
        };
        
        public TavernScreen(ScreenManager screenManager, Party party) 
            : base(screenManager)
        {
            _party = party;
        }
        
        public override string ScreenName => "Tavern";
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            
            switch (key.KeyChar)
            {
                case '1': // Listen to rumors
                    var random = new Random();
                    var rumor = _rumors[random.Next(_rumors.Length)];
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine($"\n  Patron: '{rumor}'");
                    System.Threading.Thread.Sleep(3000);
                    break;
                case '2': // Leave
                    _screenManager.PopScreen();
                    break;
            }
        }
        
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ THE PRANCING PONY TAVERN                                                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  The tavern is warm and crowded. Adventurers and merchants fill the room.");
            Console.WriteLine();
            Console.WriteLine("  [1] Listen to Rumors");
            Console.WriteLine("  [2] Leave Tavern");
            Console.WriteLine();
        }
    }
    
    /// <summary>
    /// Training Hall - level up characters
    /// </summary>
    public class TrainingHallScreen : GameScreen
    {
        private Party _party;
        
        public TrainingHallScreen(ScreenManager screenManager, Party party) 
            : base(screenManager)
        {
            _party = party;
        }
        
        public override string ScreenName => "Training Hall";
        
        public override void Update()
        {
            var key = Console.ReadKey(true);
            _screenManager.PopScreen();
        }
        
        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ TRAINING HALL                                                                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  'Training is essential for growth. Show me your experience.'");
            Console.WriteLine();
            
            bool anyCanLevel = false;
            
            foreach (var member in _party.Members)
            {
                int xpNeeded = member.Level * 2000; // Simplified
                
                if (member.ExperiencePoints >= xpNeeded && member.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  {member.Name} can level up! (Level {member.Level} -> {member.Level + 1})");
                    Console.ResetColor();
                    anyCanLevel = true;
                }
                else
                {
                    Console.WriteLine($"  {member.Name}: {member.ExperiencePoints}/{xpNeeded} XP (Level {member.Level})");
                }
            }
            
            Console.WriteLine();
            
            if (!anyCanLevel)
            {
                Console.WriteLine("  'Come back when you have more experience.'");
            }
            else
            {
                Console.WriteLine("  [Characters would level up here - feature coming soon]");
            }
            
            Console.WriteLine("\n  Press any key to leave...");
        }
    }
}
