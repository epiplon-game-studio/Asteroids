using System;
using System.Collections;
using Asteroids.Entities;
using Asteroids.Events;
using UnityEngine;

namespace Asteroids
{
    public class Game : IDisposable,
        IEventListener<MeteorDestroyedEvent>,
        IEventListener<SaucerDestroyedEvent>,
        IEventListener<GameStateChangedEvent>,
        IEventListener<PlayerHitEvent>,
        IEventListener<CreateHighscore>
    {
        public static Game current { get; private set; }
        static GameSettings settings;

        public int Points { get; private set; }
        public int Lives { get; private set; }
        public Highscores Highscores { get; private set; }
        public GameState CurrentState { get { return currentState; } }
        private GameState currentState = GameState.NotStarted;

        public Game(int lives)
        {
            Points = 0;
            Lives = lives;

            this.Listen<MeteorDestroyedEvent>();
            this.Listen<SaucerDestroyedEvent>();
            this.Listen<GameStateChangedEvent>();
            this.Listen<PlayerHitEvent>();
            this.Listen<CreateHighscore>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            settings = Resources.Load<GameSettings>("Settings");
            current = new Game(settings.StartingLives);
            current.Highscores = Highscores.Load();

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Load()
        {
            EventManager.Trigger(new GameStateChangedEvent(GameState.NotStarted));
            EventManager.Trigger(new GameEvent(current.Points, current.Lives));
        }
        
        public void Dispose()
        {
            this.Unlisten<MeteorDestroyedEvent>();
            this.Unlisten<SaucerDestroyedEvent>();
            this.Unlisten<GameStateChangedEvent>();
            this.Unlisten<PlayerHitEvent>();
            this.Unlisten<CreateHighscore>();
        }

        public void OnEvent(MeteorDestroyedEvent e)
        {
            Points += settings.AsteroidsPoints * (int) e.meteor.Category;

            EventManager.Trigger(new GameEvent(Points, Lives));
        }
        
        public void OnEvent(SaucerDestroyedEvent e)
        {
            switch (e.saucer.SaucerType)
            {
                case SaucerType.Big:
                    Points += settings.BigSaucerPoints;
                    break;
                case SaucerType.Small:
                    Points += settings.SmallSaucerPoints;
                    break;
            }
        }

        public void OnEvent(GameStateChangedEvent e)
        {
            currentState = e.CurrentState;
            switch (currentState)
            {
                case GameState.NotStarted:
                    Lives = settings.StartingLives;
                    Points = 0;
                    Time.timeScale = 1;
                    EventManager.Trigger(new GameEvent(current.Points, current.Lives));
                    break;
                case GameState.Paused:
                    Time.timeScale = 0;
                    break;
                case GameState.Started:
                case GameState.Resumed:
                    Time.timeScale = 1;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0.5f;
                    break;
                case GameState.Highscores:
                    Highscores.Save();
                    break;
            }
        }

        public void OnEvent(CreateHighscore e)
        {
            Highscores.New(e.Name, Points);
            EventManager.Trigger(new GameStateChangedEvent(GameState.Highscores));
        }

        public void OnEvent(PlayerHitEvent e)
        {
            Lives = Mathf.Clamp(Lives - 1, 0, 250);
            EventManager.Trigger(new GameEvent(Points, Lives));
            if(Lives == 0)
                EventManager.Trigger(new GameStateChangedEvent(GameState.GameOver));
        }
    }

    public enum GameState
    {
        NotStarted,
        Started,
        Paused,
        Resumed,
        GameOver,
        Highscores
    }

}
