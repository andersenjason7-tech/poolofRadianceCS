using System.Reflection;
using Xunit;
using PoolOfRadiance.Core;

namespace PoolOfRadianceCS.Tests
{
    public class GameTests
    {
        [Fact]
        public void Constants_AreSetToExpectedValues()
        {
            Assert.Equal(320, Game.SCREEN_WIDTH);
            Assert.Equal(200, Game.SCREEN_HEIGHT);
            Assert.Equal(3, Game.SCALE_FACTOR);
        }

        [Fact]
        public void Constructor_SetsDefaultState()
        {
            var game = new Game();

            var stateField = typeof(Game).GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var currentState = (GameState)stateField.GetValue(game)!;
            Assert.Equal(GameState.MainMenu, currentState);

            var runningField = typeof(Game).GetField("_isRunning", BindingFlags.NonPublic | BindingFlags.Instance);
            var isRunning = (bool)runningField.GetValue(game)!;
            Assert.False(isRunning);
        }

        [Fact]
        public void ChangeState_UpdatesInternalState()
        {
            var game = new Game();
            game.ChangeState(GameState.Exploration);

            var stateField = typeof(Game).GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var currentState = (GameState)stateField.GetValue(game)!;
            Assert.Equal(GameState.Exploration, currentState);
        }

        [Fact]
        public void Update_StopsGameWhenQuitRequested()
        {
            var game = new Game();

            // grab the private input manager and request quit
            var inputField = typeof(Game).GetField("_inputManager", BindingFlags.NonPublic | BindingFlags.Instance);
            var inputManager = (InputManager)inputField.GetValue(game)!;
            inputManager.RequestQuit();

            // invoke the private Update method
            var updateMethod = typeof(Game).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
            updateMethod.Invoke(game, null);

            var runningField = typeof(Game).GetField("_isRunning", BindingFlags.NonPublic | BindingFlags.Instance);
            var isRunning = (bool)runningField.GetValue(game)!;
            Assert.False(isRunning);
        }
    }
}
