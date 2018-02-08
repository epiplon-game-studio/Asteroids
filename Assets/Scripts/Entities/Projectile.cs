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
            Invoke("Disable", Lifetime);
        }
        
        private void FixedUpdate()
        {
            var pos2d = new Vector2(transform.position.x, transform.position.y);
            body.MovePosition(pos2d + (direction * speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                Disable();
        }

        private void Disable()
        {
            direction = Vector2.zero;
            speed = 0f;

            CancelInvoke("Disable");
            gameObject.SetActive(false);
        }
    }

    public enum ProjectileCategory { Friendly, Hostile }
}
