using Asteroids.Events;
using Asteroids.General;
using Asteroids.Pooling;
using UnityEngine;

namespace Asteroids.Entities
{
    /// <summary>
    /// Controls the saucers in the game
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Saucer : MonoBehaviour,
        IEventListener<GameStateChangedEvent>
    {
        [Header("Laser")]
        public string laserKey;         // pooling key
        public float LaserSpeed = 10;   
        public float MaxTimer = 2f;
        float laserCountdown;           // delay between shots

        [Header("Options")]
        public SaucerType SaucerType;   // big or small
        public float Speed = 5f;
        public LayerMask HostileLayer;

        [HideInInspector] public SaucerDirection Direction;
        Rigidbody2D body;
        bool isGameOver = false;

        private void OnEnable()
        {
            laserCountdown = MaxTimer;
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            this.Listen();
        }

        private void Update()
        {
            // delay the laser spawn
            laserCountdown = Mathf.Clamp(laserCountdown - Time.deltaTime, 0, MaxTimer);
            if (laserCountdown <= 0)
            {
                if (Player.Instance == null) // no player in the scene (maybe dead)
                    return;

                var playerPos = Player.Instance.transform.position;
                var direction = (playerPos - transform.position).normalized;

                var p = PoolManager.Get<Projectile>(laserKey);
                p.transform.position = transform.position;

                switch (SaucerType)
                {
                    case SaucerType.Big:
                        // shoot at completely random directions
                        direction = Quaternion.Euler(0f, 0f, Random.Range(0, 360)) * direction;
                        break;
                    case SaucerType.Small:
                        // increase accuracy with more player points
                        if (Game.current.Points < EnemyManager.Instance.HardModeScore)
                        {
                            float percentage = Mathf.Clamp(Game.current.Points / EnemyManager.Instance.HardModeScore, 0, 1);
                            var accuracity = Random.Range(0, 45f - (45f * percentage));

                            direction = Quaternion.Euler(0f, 0f, accuracity) * direction;
                        }
                        break;
                    default:
                        break;
                }

                p.Shoot(direction, LaserSpeed);

                laserCountdown = MaxTimer;
            }
            var pos2d = new Vector2(transform.position.x, transform.position.y);
            body.MovePosition(pos2d + (Vector2.right * (int)Direction * Speed * Time.deltaTime));
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

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (!isGameOver)
                EventManager.Trigger(new SaucerDestroyedEvent(this));
        }

        private void OnDestroy()
        {
            this.Unlisten();
        }

        public void OnEvent(GameStateChangedEvent e)
        {
            if (e.CurrentState == GameState.GameOver)
            {
                isGameOver = true;
                Disable();
            }
            if (e.CurrentState == GameState.Started)
                isGameOver = false;
        }
    }

    public enum SaucerType { Big, Small }
    public enum SaucerDirection { Left = -1, Right = 1 }
}
