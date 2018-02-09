using Asteroids.Entities;
using Asteroids.Events;
using Asteroids.General;
using Asteroids.Pooling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public class EnemyManager : MonoBehaviour,
        IEventListener<MeteorDestroyedEvent>,
        IEventListener<SaucerDestroyedEvent>,
        IEventListener<GameStateChangedEvent>
    {
        public static EnemyManager Instance;

        [HideInInspector]
        public SaucerDirection Direction;

        [Header("Pooling Keys")]
        public string bigMeteorKey;
        public string mediumMeteorKey;
        public string tinyMeteorKey;
        [Space]
        public string smallSaucer;
        public string bigSaucer;

        [Header("Meteor Options")]
        public int startingMeteors = 4;
        public int maxMeteors = 10;
        public int meteorIncrement = 1;
        public string explosionFXKey = "explosion";
        public float minPlayerDistance = 5f;
        int meteorsCount;

        [Header("Saucer Options")]
        public float saucerDelaySeconds;
        public Vector2 saucerSpawnOffset;
        public string saucerHitFx = "saucerhit";
        [Tooltip("When player reaches this score, game becomes harder")]
        public int HardModeScore = 40000;

        [Header("Debug")]
        public bool debugSpawnSaucer;
        public bool debugSpawnMeteor;

        List<Meteor> currentMeteors;
        Coroutine meteorCoroutine;
        Coroutine saucerCoroutine;

        private void Awake()
        {
            this.Listen<MeteorDestroyedEvent>();
            this.Listen<SaucerDestroyedEvent>();
            this.Listen<GameStateChangedEvent>();
            Instance = this;
        }

        void Start()
        {
            meteorsCount = startingMeteors;

            Direction = SaucerDirection.Left;

        }

        void OnDestroy()
        {
            this.Unlisten<MeteorDestroyedEvent>();
            this.Unlisten<SaucerDestroyedEvent>();
            this.Unlisten<GameStateChangedEvent>();
        }

        #region Spawn Routines

        /// <summary> Spawn a new saucer </summary>
        IEnumerator SaucerSpawn()
        {
            yield return new WaitForSecondsRealtime(saucerDelaySeconds);

#if UNITY_EDITOR
            if (!debugSpawnSaucer)
                yield return null;
#endif

            var saucer = SelectSaucer();
            Debug.Log("Spawning saucer...");

            // set saucer direction 
            saucer.Direction = Direction;

            // choose a random position in Y axis to start
            var randomHeight = Random.Range(
                ScreenBounds.MinPoint.y - saucerSpawnOffset.y,
                ScreenBounds.MaxPoint.y + saucerSpawnOffset.y);

            switch (Direction)
            {
                case SaucerDirection.Left:
                    saucer.transform.position = new Vector2(ScreenBounds.MaxPoint.x - saucerSpawnOffset.x, randomHeight);
                    Direction = SaucerDirection.Right;  // shift direction
                    break;
                case SaucerDirection.Right:
                    saucer.transform.position = new Vector2(ScreenBounds.MinPoint.x + saucerSpawnOffset.x, randomHeight);
                    Direction = SaucerDirection.Left;  // shift direction
                    break;
            }

        }

        /// <summary>
        /// Choose a saucer type based on player points
        /// </summary>
        /// <returns></returns>
        Saucer SelectSaucer()
        {
            if (Game.current.Points < HardModeScore)
            {
                if ((SaucerType)Random.Range(0, 2) == SaucerType.Big)
                    return PoolManager.Get<Saucer>(bigSaucer);
                else
                    return PoolManager.Get<Saucer>(smallSaucer);
            }

            return PoolManager.Get<Saucer>(bigSaucer);
        }

        IEnumerator MeteorGenerate()
        {
            yield return new WaitForSecondsRealtime(1f);

            if (Player.Instance == null)
                yield break;

#if UNITY_EDITOR
            if (!debugSpawnMeteor)
                yield return null;
#endif
            currentMeteors = new List<Meteor>();
            for (int i = 0; i < meteorsCount; i++)
            {
                var x = Mathf.Cos(Random.Range(0, Mathf.PI * 2)) * minPlayerDistance + i;
                var y = Mathf.Sin(Random.Range(0, Mathf.PI * 2)) * minPlayerDistance + i;
                Vector2 position = new Vector2(x + Player.Instance.transform.position.x, y + Player.Instance.transform.position.y);
                SpawnMeteor(bigMeteorKey, position, 1);
                yield return new WaitForEndOfFrame();
            }

        }

        public void SpawnMeteor(string key, Vector2 position, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var newMeteor = PoolManager.Get<Meteor>(key);
                newMeteor.Spawn(position);
                currentMeteors.Add(newMeteor);
            }
        }

        #endregion

        #region Events

        public void OnEvent(MeteorDestroyedEvent e)
        {
            var fx = PoolManager.Get(explosionFXKey);
            fx.gameObject.SetActive(true);
            fx.transform.position = e.meteor.transform.position;

            switch (e.meteor.Category)
            {
                case MeteorCategory.Big:
                    SpawnMeteor(mediumMeteorKey, e.meteor.transform.position, 2);
                    break;
                case MeteorCategory.Medium:
                    SpawnMeteor(tinyMeteorKey, e.meteor.transform.position, 2);
                    break;
            }

            if (currentMeteors.All(m => !m.gameObject.activeInHierarchy))
            {
                meteorsCount = Mathf.Clamp(meteorsCount + meteorIncrement, 0, maxMeteors);
                meteorCoroutine = StartCoroutine(MeteorGenerate());
            }
        }

        public void OnEvent(SaucerDestroyedEvent e)
        {
            saucerCoroutine = StartCoroutine(SaucerSpawn());

            var fx = PoolManager.Get(saucerHitFx);
            fx.gameObject.SetActive(true);
            fx.transform.position = e.saucer.transform.position;
        }

        public void OnEvent(GameStateChangedEvent e)
        {
            if (e.CurrentState == GameState.Started)
            {
                meteorCoroutine = StartCoroutine(MeteorGenerate());
                saucerCoroutine = StartCoroutine(SaucerSpawn());
            }
            if (e.CurrentState == GameState.GameOver)
            {
                StopAllCoroutines();
                meteorsCount = startingMeteors;
            }
        }

        #endregion
    }
}
