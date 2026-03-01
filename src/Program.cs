using System;
using PoolOfRadiance.Core;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Combat;
using PoolOfRadiance.UI;
using PoolOfRadiance.UI.Screens;
using System.Runtime.InteropServices;

namespace PoolOfRadiance
{
    /// <summary>
    /// Main entry point for Pool of Radiance C# remake
    /// Now with full screen-based UI system!
    /// </summary>
    class Program
    {
        const int windowWidth = 120;
        const int windowLength = 40;

        static void Main(string[] args)
        {
            // Set console size for better display
            try
            {
                if (!Console.IsOutputRedirected && !Console.IsInputRedirected)
                {
                    
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        try
                        {
                            Console.SetWindowSize(Math.Min(windowWidth, Console.LargestWindowWidth),
                                         Math.Min(windowLength, Console.LargestWindowHeight));
                            Console.SetBufferSize(120, 300);
                        }
                        catch (Exception)
                        {
                            // Fail silently if buffer size can't be set
                        }
                    }
                }
            }

            catch
            {
                // If we can't set size, just continue
            }

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                                                ║");
            Console.WriteLine("║                        POOL OF RADIANCE - C# REMAKE                            ║");
            Console.WriteLine("║                     Based on the classic 1988 SSI game                         ║");
            Console.WriteLine("║                                                                                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  Select Mode:");
            Console.WriteLine("  [1] New Game (Full UI Experience)");
            Console.WriteLine("  [2] Quick Combat Demo");
            Console.WriteLine("  [3] Exit");
            Console.WriteLine();
            Console.Write("  Choice: ");

            var choice = Console.ReadKey(true);

            switch (choice.KeyChar)
            {
                case '1':
                    RunFullGame();
                    break;
                case '2':
                    RunQuickDemo();
                    break;
                case '3':
                    return;
            }
        }

        static void RunFullGame()
        {


            Console.Clear();
            Console.WriteLine("\n=== CHARACTER CREATION ===\n");

            // Create a party using PartyManagement
            var partyManager = new PartyManagement("Phlan Adventurers");

            // let user choose how the party is created
            Console.WriteLine("\nChoose party creation method:");
            Console.WriteLine("[1] Default party");
            Console.WriteLine("[2] Custom party (build your own character list)");
            char partyChoice;
            do
            {
                partyChoice = Console.ReadKey(true).KeyChar;
            } while (partyChoice != '1' && partyChoice != '2');

            if (partyChoice == '1')
            {
                partyManager.CreateDefaultParty();
            }
            else
            {
                CreateCustomParty(partyManager);
            }

            Console.WriteLine("\nYour party is ready to begin their adventure!");
            Console.WriteLine("\nPress any key to enter the town of Phlan...");
            Console.ReadKey(true);

            // Start the screen-based UI system
            var screenManager = new ScreenManager();
            Console.ReadKey(true);

            // Start at the town screen
            screenManager.PushScreen(new TownScreen(screenManager, partyManager.Party, "Phlan"));
            Console.ReadKey(true);

            // Main game loop
            while (screenManager.IsRunning)
            {
                screenManager.Render();
                screenManager.Update();
            }

            Console.Clear();
            Console.WriteLine("\n\nThank you for playing Pool of Radiance!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void CreateCustomParty(PartyManagement partyManager)
        {
            Console.WriteLine("\n=== CUSTOM PARTY CREATION ===\n");

            // ask how many characters
            int count;
            do
            {
                Console.Write("How many characters in your party (2-8)? ");
                if (!int.TryParse(Console.ReadLine(), out count)) count = 0;
            } while (count < 2 || count > 8);

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nCreating character {i + 1} of {count}");
                Console.Write("Name: ");
                var name = Console.ReadLine() ?? "Unnamed";

                // choose race
                Console.WriteLine("Choose race:");
                var races = Enum.GetValues(typeof(PoolOfRadiance.Characters.CharacterRace));
                for (int r = 0; r < races.Length; r++)
                {
                    Console.WriteLine($"[{r + 1}] {races.GetValue(r)}");
                }
                int raceIndex;
                do
                {
                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out raceIndex)) raceIndex = 0;
                } while (raceIndex < 1 || raceIndex > races.Length);
                var race = (PoolOfRadiance.Characters.CharacterRace)races.GetValue(raceIndex - 1)!;

                // choose class
                Console.WriteLine("Choose class:");
                var classes = PoolOfRadiance.Characters.CharacterClass.Values;
                int cnum = 1;
                foreach (var cls in classes)
                {
                    Console.WriteLine($"[{cnum}] {cls}");
                    cnum++;
                }
                int classIndex;
                do
                {
                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out classIndex)) classIndex = 0;
                } while (classIndex < 1 || classIndex > cnum - 1);
                var charClass = System.Linq.Enumerable.ElementAt(classes, classIndex - 1);

                var character = new PoolOfRadiance.Characters.Character(name, race, charClass);
                character.RollStats();
                // set some default HP based on class hit die
                character.HitPointsMax = 5 + character.Stats.GetHPModifier();
                if (character.HitPointsMax < 1) character.HitPointsMax = 1;
                character.HitPointsCurrent = character.HitPointsMax;
                partyManager.AddCharacter(character);
            }

            Console.WriteLine("\nYour custom party is ready!\n");
        }

        static void RunQuickDemo()
        {
            Console.Clear();
            Console.WriteLine("=== QUICK COMBAT DEMO ===\n");

            // Create a party using PartyManagement
            var partyManager = new PartyManagement("Combat Demo Party");
            partyManager.CreateDefaultParty();

            Console.WriteLine("\nCreating enemies...");

            // Create some enemies
            var enemies = new System.Collections.Generic.List<Character>();

            for (int i = 0; i < 3; i++)
            {
                var goblin = new Character($"Goblin {i + 1}", CharacterRace.Human, CharacterClass.Fighter);
                goblin.Stats.Strength = 12;
                goblin.Stats.Dexterity = 13;
                goblin.HitPointsMax = 7;
                goblin.HitPointsCurrent = 7;
                goblin.Level = 1;
                goblin.CalculateDerivedStats();
                enemies.Add(goblin);
                Console.WriteLine($"✓ {goblin.Name} appears!");
            }

            Console.WriteLine("\n\nPress any key to begin combat...");
            Console.ReadKey(true);

            // Start combat with screen system
            var screenManager = new ScreenManager();
            screenManager.PushScreen(new CombatScreen(screenManager, partyManager.Party, enemies, "Demo Arena"));

            while (screenManager.IsRunning)
            {
                screenManager.Render();
                screenManager.Update();
            }

            Console.Clear();
            Console.WriteLine("\n\n=== COMBAT COMPLETE ===");
            partyManager.Party.PrintStatus();

            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
