using Asteroids.Entities;
using Asteroids.Events;
using Asteroids.Input;
using UnityEngine;

namespace Asteroids
{
    public class PlayerManager : MonoBehaviour,
        IEventListener<GameStateChangedEvent>
    {
        public string MenuButton;
        [Space]
        public Player PlayerTemplate;

        void Awake()
        {
            this.Listen();
        }

        void Update()
        {
            if (CPInputManager.Instance.GetButton(MenuButton))
            {
                EventManager.Trigger(new GameStateChangedEvent(GameState.Paused));
            }
        }

        private void OnDisable()
        {
            this.Unlisten();
        }

        public void OnEvent(GameStateChangedEvent e)
        {
            if (e.CurrentState== GameState.Started)
            {
                var player = Instantiate(PlayerTemplate);
                player.transform.position = Vector2.zero;
            }
        }
    }
}
