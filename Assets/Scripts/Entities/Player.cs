using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Input;
using Asteroids.Pooling;
using Asteroids.General;

namespace Asteroids.Entities
{
    /// <summary> Main Player class </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public static Player Instance;

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
        public ParticleSystem Thruster;     // thruster effect
        public LayerMask HostileLayer;      // layer of hostile entities

        Rigidbody2D body;
        bool isAddingForce;
        float rotation;                 // spaceship rotation

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            Instance = this;
        }

        void Update()
        {
            laserCountdown = Mathf.Clamp(laserCountdown - Time.deltaTime, 0, MaxTimer);

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
        }

        private void FixedUpdate()
        {
            if (isAddingForce)
                body.AddForce(transform.up * ForceMultiplier, ForceMode2D.Force);

            body.velocity = Vector2.ClampMagnitude(body.velocity, VelocityMax);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Utility.CompareLayer(collision.otherCollider.gameObject, HostileLayer))
            {
                body.velocity = Vector2.zero;
            }
        }
    }
}
