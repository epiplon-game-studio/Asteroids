using System.Collections;
using System.Collections.Generic;
using Asteroids.Entities;
using Asteroids.Events;
using UnityEngine;

namespace Asteroids
{
    public class Game :
        IEventListener<MeteorDestroyedEvent>
    {
        public static Game current { get; private set; }

        public int Points { get; private set; }
        public int Lives { get; private set; }

        static GameSettings settings;

        public Game(int lives)
        {
            Points = 0;
            Lives = lives;

            this.Listen<MeteorDestroyedEvent>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            var highscores = new Highscores();
            settings = Resources.Load<GameSettings>("Settings");

            current = new Game(settings.StartingLives);
        }

        public void OnEvent(MeteorDestroyedEvent e)
        {
            Points += settings.AsteroidsPoints * (int) e.meteor.Category;

            EventManager.Trigger(new GameEvent(Points, Lives));
        }
    }
}
