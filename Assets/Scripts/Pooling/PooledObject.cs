using UnityEngine;

namespace Asteroids.Pooling
{
    public class PooledObject : MonoBehaviour
    {
        [HideInInspector]
        public string key;

        void OnDisable()
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Object " + gameObject.name + " was not instantiated from Pool Manager!");
                return;
            }

            PoolManager.Insert(this);
        }
    }

}
