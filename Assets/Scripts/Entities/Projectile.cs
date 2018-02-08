using Asteroids.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Entities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        //public ProjectileCategory Category;
        public LayerMask HostileLayer;
        //public LayerMask EnemyLayer;
        public float Lifetime = 1f;
        float currentLife;

        Rigidbody2D body;
        Vector2 direction;
        float speed;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void Shoot(Vector2 direction, float speed)
        {
            this.direction = direction;
            this.speed = speed;
            currentLife = Lifetime;
        }
        
        private void FixedUpdate()
        {
            var pos2d = new Vector2(transform.position.x, transform.position.y);
            body.MovePosition(pos2d + (direction * speed * Time.fixedDeltaTime));
            currentLife = Mathf.Clamp(currentLife - Time.fixedDeltaTime, 0, Lifetime);
            if (currentLife <= 0)
                Disable();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
                return;

            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                Disable();
        }

        private void Disable()
        {
            direction = Vector2.zero;
            speed = 0f;
            gameObject.SetActive(false);
        }
    }

    public enum ProjectileCategory { Friendly, Hostile }
}
