using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public static class Game
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            var highscores = new Highscores();
        }
    }

}
