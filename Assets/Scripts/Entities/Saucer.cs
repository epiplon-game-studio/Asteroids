using Asteroids.Events;
using Asteroids.General;
using Asteroids.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Saucer : MonoBehaviour
    {
        [Header("Laser")]
        public string laserKey;         // pooling key
        public float LaserSpeed = 10;   
        public float MaxTimer = 2f;
        float laserCountdown;           // delay between shots

        [Header("Options")]
        public float Speed = 5f;
        public LayerMask HostileLayer;

        [HideInInspector] public SaucerDirection Direction;
        Rigidbody2D body;

        private void OnEnable()
        {
            laserCountdown = MaxTimer;
        }

        private void OnDisable()
        {
            EventManager.Trigger(new SaucerDestroyedEvent(this));
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            laserCountdown = Mathf.Clamp(laserCountdown - Time.deltaTime, 0, MaxTimer);
            if (laserCountdown <= 0)
            {
                var playerPos = Player.Instance.transform.position;
                var direction = (playerPos - transform.position).normalized;

                var p = PoolManager.Get<Projectile>(laserKey);
                p.transform.position = transform.position;

                if (Game.current.Points < EnemyManager.Instance.HardModeScore)
                {
                    direction = Quaternion.Euler(0f, 0f, 45f) * direction;
                }

                p.Shoot(direction, LaserSpeed);

                laserCountdown = MaxTimer;
            }
            var pos2d = new Vector2(transform.position.x, transform.position.y);
            body.MovePosition(pos2d + (Vector2.right * (int)Direction * Speed * Time.deltaTime));
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                Disable();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                Disable();
        }
    }

    public enum SaucerDirection { Left = -1, Right = 1 }
}
