using System;
using System.Collections.Generic;

namespace PoolOfRadiance.UI
{
    /// <summary>
    /// Manages different game screens and transitions between them
    /// Similar to the original Pool of Radiance screen system
    /// </summary>
    public class ScreenManager
    {
        private Stack<IGameScreen>? _screenStack;
        private IGameScreen? _currentScreen;
        private bool _isRunning;
        
        public ScreenManager()
        {
            if (_currentScreen != null)
            {
            _screenStack = new Stack<IGameScreen>();
            _isRunning = true;
            }
        }
        
        public void PushScreen(IGameScreen screen)
        {
            if (_screenStack == null)
            {
                _screenStack = new Stack<IGameScreen>();
            }
            if (_currentScreen != null)
            {
                _currentScreen.OnPause();
                _screenStack.Push(_currentScreen);
            }
            
            _currentScreen = screen;
            _currentScreen.OnEnter();
            _isRunning = true;
        }
        
        public void PopScreen()
        {
            if (_screenStack == null)
            {
                _screenStack = new Stack<IGameScreen>();
            }
            if (_currentScreen != null)
            {
                _currentScreen.OnExit();
            }
            
            if (_screenStack.Count > 0)
            {
                _currentScreen = _screenStack.Pop();
                _currentScreen.OnResume();
            }
            else
            {
                _currentScreen = null;
                _isRunning = false;
            }
        }
        
        public void SwitchScreen(IGameScreen newScreen)
        {
            if (_currentScreen != null)
            {
                _currentScreen.OnExit();
            }
            
            _currentScreen = newScreen;
            _currentScreen.OnEnter();
        }
        
        public void Update()
        {
            if (_currentScreen != null)
            {
                _currentScreen.Update();
            }
        }
        
        public void Render()
        {
            if (_currentScreen != null)
            {
                _currentScreen.Render();
            }
        }
        
        public bool IsRunning => _isRunning;
        public IGameScreen CurrentScreen => _currentScreen;
    }
    
    /// <summary>
    /// Interface for all game screens
    /// </summary>
    public interface IGameScreen
    {
        void OnEnter();
        void OnExit();
        void OnPause();
        void OnResume();
        void Update();
        void Render();
        string ScreenName { get; }
    }
    
    /// <summary>
    /// Base class for game screens
    /// </summary>
    public abstract class GameScreen : IGameScreen
    {
        protected ScreenManager _screenManager;
        
        public GameScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnPause() { }
        public virtual void OnResume() { }
        public abstract void Update();
        public abstract void Render();
        public abstract string ScreenName { get; }
    }
}
