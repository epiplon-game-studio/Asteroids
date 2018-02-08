using Asteroids.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.UI
{
    public class UIGame : MonoBehaviour,
        IEventListener<GameEvent>
    {
        public Text Points;
        public HorizontalLayoutGroup LivesGroup;
        public RectTransform LiveTemplate;

        private void Awake()
        {
            this.Listen();
        }

        public void OnEvent(GameEvent e)
        {
            Points.text = e.Points.ToString("00000");
        }
    }
}
