using Asteroids.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Entities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        public ProjectileCategory Category;
        public LayerMask PlayerLayer;
        public LayerMask EnemyLayer;
        public float Lifetime = 1f;

        Rigidbody2D body;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void Shoot(Vector2 direction, float force, ForceMode2D mode)
        {
            body.AddForce(direction * force, mode);
            Invoke("Disable", Lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (Category)
            {
                case ProjectileCategory.Friendly:
                    if(Utility.CompareLayer(collision.gameObject, EnemyLayer))
                        gameObject.SetActive(false);
                    break;
                case ProjectileCategory.Hostile:
                    if (Utility.CompareLayer(collision.gameObject, PlayerLayer))
                        gameObject.SetActive(false);
                    break;
            }
        }

        private void Disable()
        {
            body.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    public enum ProjectileCategory { Friendly, Hostile }
}
