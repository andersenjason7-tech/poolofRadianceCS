using System;
using PoolOfRadiance.Core;
using PoolOfRadiance.Characters;
using PoolOfRadiance.Combat;
using PoolOfRadiance.UI;
using PoolOfRadiance.UI.Screens;

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

        // Random name lists for character generation
        static readonly string[] FighterNames = { "Aldric", "Barrack", "Corin", "Darius", "Evan", "Gareth", "Hector", "Ironus" };
        static readonly string[] ClericNames = { "Thalia", "Bella", "Cassandra", "Diana", "Elara", "Faye", "Grace", "Helena" };
        static readonly string[] WizardNames = { "Eldrin", "Alastor", "Cedric", "Draven", "Eamon", "Fennimore", "Gregori", "Hugo" };
        static readonly string[] ThiefNames = { "Raven", "Ash", "Blade", "Cipher", "Drake", "Echo", "Felix", "Gideon" };

        static void Main(string[] args)
        {
            // Set console size for better display
            try
            {
                if (!Console.IsOutputRedirected && !Console.IsInputRedirected)
                {
                    Console.SetWindowSize(Math.Min(windowWidth, Console.LargestWindowWidth),
                                         Math.Min(windowLength, Console.LargestWindowHeight));
                    Console.SetBufferSize(120, 300);
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

            // Create a sample party
            Party party = CreateParty();

            Console.WriteLine("\nYour party is ready to begin their adventure!");
            Console.WriteLine("\nPress any key to enter the town of Phlan...");
            Console.ReadKey(true);

            // Start the screen-based UI system
            var screenManager = new ScreenManager();
            Console.ReadKey(true);

            // Start at the town screen
            screenManager.PushScreen(new TownScreen(screenManager, party, "Phlan"));
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

        static string GetRandomName(string[] nameList)
        {
            var random = new Random();
            return nameList[random.Next(nameList.Length)];
        }

        static int GetRandomHitPoints(int minHP, int maxHP)
        {
            var random = new Random();
            return random.Next(minHP, maxHP + 1);
        }

        static Party CreateParty()
        {
            Party party = new Party();

            // Create fighter with random name and hit points
            var fighter = new Character(GetRandomName(FighterNames), CharacterRace.Human, CharacterClass.Fighter);
            fighter.RollStats();
            int fighterHP = GetRandomHitPoints(10, 14);
            fighter.HitPointsMax = fighterHP;
            fighter.HitPointsCurrent = fighterHP;
            party.AddMember(fighter);
            Console.WriteLine($"✓ {fighter.Name} the Fighter joins your party (HP: {fighter.HitPointsMax})");

            // Create cleric with random name and hit points
            var cleric = new Character(GetRandomName(ClericNames), CharacterRace.Human, CharacterClass.Cleric);
            cleric.RollStats();
            int clericHP = GetRandomHitPoints(6, 9);
            cleric.HitPointsMax = clericHP;
            cleric.HitPointsCurrent = clericHP;
            party.AddMember(cleric);
            Console.WriteLine($"✓ {cleric.Name} the Cleric joins your party (HP: {cleric.HitPointsMax})");

            // Create magic-user with random name and hit points
            var wizard = new Character(GetRandomName(WizardNames), CharacterRace.Elf, CharacterClass.MagicUser);
            wizard.RollStats();
            int wizardHP = GetRandomHitPoints(4, 7);
            wizard.HitPointsMax = wizardHP;
            wizard.HitPointsCurrent = wizardHP;
            party.AddMember(wizard);
            Console.WriteLine($"✓ {wizard.Name} the Magic-User joins your party (HP: {wizard.HitPointsMax})");

            // Create thief with random name and hit points
            var thief = new Character(GetRandomName(ThiefNames), CharacterRace.Halfling, CharacterClass.Thief);
            thief.RollStats();
            int thiefHP = GetRandomHitPoints(6, 9);
            thief.HitPointsMax = thiefHP;
            thief.HitPointsCurrent = thiefHP;
            party.AddMember(thief);
            Console.WriteLine($"✓ {thief.Name} the Thief joins your party (HP: {thief.HitPointsMax})");

            // Give them some starting gold
            party.DistributeGold(100);

            return party;
        }

        static void RunQuickDemo()
        {
            Console.Clear();
            Console.WriteLine("=== QUICK COMBAT DEMO ===\n");

            // Create a sample party
            Party party = CreateParty();

            Console.WriteLine("\n\nCreating enemies...");

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
            screenManager.PushScreen(new CombatScreen(screenManager, party, enemies, "Demo Arena"));

            while (screenManager.IsRunning)
            {
                screenManager.Render();
                screenManager.Update();
            }

            Console.Clear();
            Console.WriteLine("\n\n=== COMBAT COMPLETE ===");
            party.PrintStatus();

            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
