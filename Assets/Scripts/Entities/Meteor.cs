using UnityEngine;
using Asteroids.General;
using Asteroids.Pooling;
using Asteroids.Events;

namespace Asteroids.Entities
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class Meteor : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        Rigidbody2D body;

        public MeteorCategory Category;
        public LayerMask HostileLayer;

        [Header("Options")]
        public Sprite[] sprites;
        public float startingForce = 30;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();

            if (sprites.Length > 0)
            {
                var s = Random.Range(0, sprites.Length);
                spriteRenderer.sprite = sprites[s];
            }
        }

        public void Spawn(Vector2 position)
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            body.velocity = direction * startingForce * (int) Category;
            transform.position = position;
            gameObject.SetActive(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
            {
                Disable();
                EventManager.Trigger(new MeteorDestroyedEvent(this));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
            {
                Disable();
                EventManager.Trigger(new MeteorDestroyedEvent(this));
            }
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }

    public enum MeteorCategory
    {
        Big = 1,
        Medium = 2,
        Tiny = 4
    }
}
