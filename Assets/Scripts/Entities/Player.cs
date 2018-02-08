using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Input;
using Asteroids.Pooling;
using Asteroids.General;
using Asteroids.Events;

namespace Asteroids.Entities
{
    /// <summary> Main Player class </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Player : MonoBehaviour,
        IEventListener<GameEvent>
    {
        public static Player Instance = null;

        [Header("Movement")]
        public float VelocityMax = 20f;
        public float ForceMultiplier = 3f;
        public float RotationSpeed = 20f;

        [Header("Laser"), Space]
        public string laserKey;         // pooling key
        public float LaserForce = 4;
        public float MaxTimer = 2f;
        float laserCountdown;           // delay between shots

        [Header("Options"), Space]
        public float InvulnerabilityTimer = 2f;
        public ParticleSystem Thruster;     // thruster effect
        public string PlayerExplosionFX;    // when the player dies        
        public LayerMask HostileLayer;      // layer of hostile entities

        Rigidbody2D body;
        SpriteRenderer spriteRenderer;
        bool isAddingForce;
        float rotation;                 // spaceship rotation
        float invulnerable;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            this.Listen();
            Instance = this;
        }

        private void Start()
        {
            invulnerable = InvulnerabilityTimer;
        }

        void Update()
        {
            // temp invulnerability countdown
            invulnerable = Mathf.Clamp(invulnerable - Time.deltaTime, 0, InvulnerabilityTimer);

            // laser delay
            laserCountdown = Mathf.Clamp(laserCountdown - Time.deltaTime, 0, MaxTimer);

            // player input
            isAddingForce = CPInputManager.Instance.GetButton("Forward");
            rotation = CPInputManager.Instance.GetAxis("Turn") * RotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(Vector3.forward * rotation);

            // if it's ready to fire
            if (CPInputManager.Instance.GetButton("Fire")
                && (laserCountdown <= 0))
            {
                var p = PoolManager.Get<Projectile>(laserKey);
                p.transform.position = transform.position;
                p.Shoot(transform.up, LaserForce);

                laserCountdown = MaxTimer;
            }

            // emit thruster on player input
            if (isAddingForce)
                Thruster.Emit(1);

            if (invulnerable > 0)
            {
                var a = spriteRenderer.color.a * 100;
                a = Mathf.Repeat(a + 25, 255);
                spriteRenderer.color = new Color(1, 1, 1, a / 100);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }

        private void FixedUpdate()
        {
            if (isAddingForce)
                body.AddForce(transform.up * ForceMultiplier, ForceMode2D.Force);

            body.velocity = Vector2.ClampMagnitude(body.velocity, VelocityMax);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                HitPlayer();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utility.CompareLayer(collision.gameObject, HostileLayer))
                HitPlayer();
        }

        private void HitPlayer()
        {
            body.velocity = Vector2.zero;

            if (invulnerable <= 0)
            {
                invulnerable = InvulnerabilityTimer;
                EventManager.Trigger(new PlayerHitEvent());
            }
        }

        private void OnDestroy()
        {
            this.Unlisten();
        }

        public void OnEvent(GameEvent e)
        {
            if (e.Lives == 0)
            {
                var fx = PoolManager.Get(PlayerExplosionFX);
                fx.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
}
