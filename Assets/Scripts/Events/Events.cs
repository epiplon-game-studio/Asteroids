using Asteroids.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}