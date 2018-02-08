using Asteroids.Entities;

namespace Asteroids.Events
{
    public struct GameEvent
    {
        public int Points;
        public int Lives;

        public GameEvent(int points, int lives)
        {
            this.Points = points;
            this.Lives = lives;
        }
    }

    public struct GameStateChangedEvent
    {
        public GameState CurrentState;

        public GameStateChangedEvent(GameState gameState)
        {
            CurrentState = gameState;
        }
    }

    public struct MeteorDestroyedEvent
    {
        public Meteor meteor;

        public MeteorDestroyedEvent(Meteor meteor)
        {
            this.meteor = meteor;
        }
    }

    public struct SaucerDestroyedEvent
    {
        public Saucer saucer;

        public SaucerDestroyedEvent(Saucer saucer)
        {
            this.saucer = saucer;
        }
    }

    public struct PlayerHitEvent { }

    public struct CreateHighscore
    {
        public string Name;

        public CreateHighscore(string name)
        {
            Name = name;
        }
    }
}