using System;

namespace PoolOfRadiance.Core
{
    /// <summary>
    /// Main game class - handles initialization, game loop, and core systems
    /// </summary>
    public class Game
    {
        private bool _isRunning;
        private GameState _currentState;
        private readonly InputManager _inputManager;
        private readonly Graphics.Renderer _renderer;
        private readonly World.WorldManager _worldManager;
        
        public const int SCREEN_WIDTH = 320;
        public const int SCREEN_HEIGHT = 200;
        public const int SCALE_FACTOR = 3; // Scale up for modern displays
        
        public Game()
        {
            _inputManager = new InputManager();
            _renderer = new Graphics.Renderer(SCREEN_WIDTH, SCREEN_HEIGHT, SCALE_FACTOR);
            _worldManager = new World.WorldManager();
            _currentState = GameState.MainMenu;
            _isRunning = false;
        }
        
        public void Initialize()
        {
            Console.WriteLine("Pool of Radiance - C# Remake");
            Console.WriteLine("Initializing game systems...");
            
            // Load game data
            Data.DataLoader.LoadGameData();
            
            // Initialize graphics system
            _renderer.Initialize();
            
            // Load initial assets
            LoadAssets();
            
            Console.WriteLine("Initialization complete!");
        }
        
        private void LoadAssets()
        {
            // Load sprite sheets
            // Load fonts
            // Load UI elements
            Console.WriteLine("Loading assets...");
        }
        
        public void Run()
        {
            _isRunning = true;
            
            // Main game loop
            while (_isRunning)
            {
                Update();
                Render();
                
                // Simple frame limiting (replace with proper timing)
                System.Threading.Thread.Sleep(16); // ~60 FPS
            }
            
            Cleanup();
        }
        
        private void Update()
        {
            // Process input
            _inputManager.Update();
            
            // Check for quit
            if (_inputManager.IsQuitRequested())
            {
                _isRunning = false;
                return;
            }
            
            // Update based on current game state
            switch (_currentState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu();
                    break;
                case GameState.CharacterCreation:
                    UpdateCharacterCreation();
                    break;
                case GameState.Exploration:
                    UpdateExploration();
                    break;
                case GameState.Combat:
                    UpdateCombat();
                    break;
                case GameState.Dialog:
                    UpdateDialog();
                    break;
            }
        }
        
        private void UpdateMainMenu()
        {
            // Handle main menu logic
        }
        
        private void UpdateCharacterCreation()
        {
            // Handle character creation
        }
        
        private void UpdateExploration()
        {
            // Handle world exploration
            _worldManager.Update();
        }
        
        private void UpdateCombat()
        {
            // Handle combat
        }
        
        private void UpdateDialog()
        {
            // Handle dialog/conversation
        }
        
        private void Render()
        {
            _renderer.Clear();
            
            // Render based on current game state
            switch (_currentState)
            {
                case GameState.MainMenu:
                    RenderMainMenu();
                    break;
                case GameState.CharacterCreation:
                    RenderCharacterCreation();
                    break;
                case GameState.Exploration:
                    RenderExploration();
                    break;
                case GameState.Combat:
                    RenderCombat();
                    break;
                case GameState.Dialog:
                    RenderDialog();
                    break;
            }
            
            _renderer.Present();
        }
        
        private void RenderMainMenu()
        {
            // Render main menu
            _renderer.DrawText("POOL OF RADIANCE", 80, 50);
            _renderer.DrawText("1. New Game", 100, 100);
            _renderer.DrawText("2. Load Game", 100, 120);
            _renderer.DrawText("3. Exit", 100, 140);
        }
        
        private void RenderCharacterCreation()
        {
            // Render character creation screen
        }
        
        private void RenderExploration()
        {
            // Render the world
            _worldManager.Render(_renderer);
        }
        
        private void RenderCombat()
        {
            // Render combat screen
        }
        
        private void RenderDialog()
        {
            // Render dialog
        }
        
        private void Cleanup()
        {
            Console.WriteLine("Shutting down...");
            _renderer.Cleanup();
        }
        
        public void ChangeState(GameState newState)
        {
            _currentState = newState;
        }
    }
    
    public enum GameState
    {
        MainMenu,
        CharacterCreation,
        Exploration,
        Combat,
        Dialog,
        Inventory,
        Shopping
    }
}
