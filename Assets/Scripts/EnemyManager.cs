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
        IEventListener<SaucerDestroyedEvent>
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
            Instance = this;
        }

        void Start()
        {
            meteorsCount = startingMeteors;

            Direction = SaucerDirection.Left;
            meteorCoroutine = StartCoroutine(MeteorGenerate());
            saucerCoroutine = StartCoroutine(SaucerSpawn());
        }

        #region Spawn Routines

        IEnumerator SaucerSpawn()
        {
            yield return new WaitForSecondsRealtime(saucerDelaySeconds);
#if UNITY_EDITOR
            if (debugSpawnSaucer)
            {
                var saucer = PoolManager.Get<Saucer>(smallSaucer);
                saucer.Direction = Direction;

                switch (Direction)
                {
                    case SaucerDirection.Left:
                        saucer.transform.position = new Vector2(ScreenBounds.MaxPoint.x - 1, 0f);
                        Direction = SaucerDirection.Right;
                        break;
                    case SaucerDirection.Right:
                        saucer.transform.position = new Vector2(ScreenBounds.MinPoint.x + 1, 0f);
                        Direction = SaucerDirection.Left;
                        break;
                }
            }
#else
                    var saucer = PoolManager.Get<Saucer>(smallSaucer);
                    saucer.Direction = Direction;

                    switch (Direction)
                    {
                        case SaucerDirection.Left:
                            saucer.transform.position = new Vector2(ScreenBounds.MaxPoint.x - 1, 0f);
                            Direction = SaucerDirection.Right;
                            break;
                        case SaucerDirection.Right:
                            saucer.transform.position = new Vector2(ScreenBounds.MinPoint.x + 1, 0f);
                            Direction = SaucerDirection.Left;
                            break;
                    }
#endif

        }

        IEnumerator MeteorGenerate()
        {
            yield return new WaitForSecondsRealtime(1f);

#if UNITY_EDITOR
            if (debugSpawnMeteor)
            {
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
#else
            currentMeteors = new List<Meteor>();
            for (int i = 0; i < meteorsCount; i++)
            {
                var x = Mathf.Cos(Random.Range(0, Mathf.PI * 2)) * minPlayerDistance + i;
                var y = Mathf.Sin(Random.Range(0, Mathf.PI * 2)) * minPlayerDistance + i;
                Vector2 position = new Vector2(x + Player.Instance.transform.position.x, y + Player.Instance.transform.position.y);
                SpawnMeteor(bigMeteorKey, position, 1);
                yield return new WaitForEndOfFrame();
            }
#endif
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

        #endregion
    }




}
