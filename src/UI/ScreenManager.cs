using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PoolOfRadiance.UI
{
    /// <summary>
    /// Manages different game screens and transitions between them
    /// Similar to the original Pool of Radiance screen system
    /// </summary>
    public class ScreenManager
    {
        private Stack<IGameScreen>? _screenStack;
        private bool _isRunning;
        
        public ScreenManager()
        {
            if (CurrentScreen != null)
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
            if (CurrentScreen != null)
            {
                CurrentScreen.OnPause();
                _screenStack.Push(CurrentScreen);
            }
            
            CurrentScreen = screen;
            CurrentScreen.OnEnter();
            _isRunning = true;
        }
        
        public void PopScreen()
        {
            if (_screenStack == null)
            {
                _screenStack = new Stack<IGameScreen>();
            }
            if (CurrentScreen != null)
            {
                CurrentScreen.OnExit();
            }
            
            if (_screenStack.Count > 0)
            {
                CurrentScreen = _screenStack.Pop();
                CurrentScreen.OnResume();
            }
            else
            {
                CurrentScreen = null;
                _isRunning = false;
            }
        }
        
        public void SwitchScreen(IGameScreen newScreen)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.OnExit();
            }
            
            CurrentScreen = newScreen;
            CurrentScreen.OnEnter();
        }
        
        public void Update()
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Update();
            }
        }
        
        public void Render()
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Render();
            }
        }
        
        public bool IsRunning => _isRunning;

        [AllowNull]
        public IGameScreen CurrentScreen { get; private set; }
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
