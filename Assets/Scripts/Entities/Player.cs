using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Input;
using Asteroids.Pooling;

namespace Asteroids.Entities
{
    /// <summary> Main Player class </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        public float VelocityMax = 20f;
        public float ForceMultiplier = 3f;
        public float RotationSpeed = 20f;

        [Header("Laser"), Space]
        public float LaserForce = 4;
        public ForceMode2D Mode;
        public float MaxTimer = 2f;
        float laserCountdown;

        [Header("FX"), Space]
        public ParticleSystem Thruster;

        Rigidbody2D body;
        bool isAddingForce;
        float rotation;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            laserCountdown = Mathf.Clamp(laserCountdown - Time.deltaTime, 0, MaxTimer);

            isAddingForce = CPInputManager.Instance.GetButton("Forward");
            rotation = CPInputManager.Instance.GetAxis("Turn") * RotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(Vector3.forward * rotation);

            if (CPInputManager.Instance.GetButton("Fire") 
                && (laserCountdown <= 0))
            {
                var p = PoolManager.Get<Projectile>("plaser");
                p.transform.position = transform.position;
                p.Shoot(transform.up, LaserForce, Mode);

                laserCountdown = MaxTimer;
            }

            if (isAddingForce)
                Thruster.Emit(1);
        }

        private void FixedUpdate()
        {
            if (isAddingForce)
                body.AddForce(transform.up * ForceMultiplier, ForceMode2D.Force);

            body.velocity = Vector2.ClampMagnitude(body.velocity, VelocityMax);
        }
    }
}
