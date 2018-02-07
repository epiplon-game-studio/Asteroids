using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Entities
{
    public class Meteor : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        [Header("Visuals")]
        public Sprite[] bigSprites;
        public Sprite[] mediumSprites;
        public Sprite[] tinySprites;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();


        }
    }
}
