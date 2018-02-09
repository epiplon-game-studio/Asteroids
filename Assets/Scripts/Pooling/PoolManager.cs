using Asteroids.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Pooling
{
    /// <summary> Manages objects reusage </summary>
    public static class PoolManager
    {
        static Dictionary<string, PoolEntry> _entries = new Dictionary<string, PoolEntry>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            PoolSettings settings = Resources.Load<PoolSettings>("Pool");
            foreach (var reg in settings.RegistryList)
            {
                var newEntry = new PoolEntry(reg.Template, reg.AutoResize);
                _entries.Add(reg.Key, newEntry);

                for (int i = 0; i < reg.StartCapacity; i++)
                {
                    var instance = Object.Instantiate(reg.Template);
                    instance.key = reg.Key;
                    instance.gameObject.SetActive(false);
                    //newEntry.RuntimeInstances.Push(instance);
                }

            }
        }

        public static PooledObject Get(string key)
        {
            PoolEntry entry;
            if (_entries.TryGetValue(key, out entry))
            {
                if (entry.RuntimeInstances.Count > 0)
                {
                    var pooledObject = entry.RuntimeInstances.Pop();
                    pooledObject.transform.position = Vector3.zero;
                    pooledObject.transform.rotation = Quaternion.identity;
                    pooledObject.gameObject.SetActive(true);
                    return pooledObject;
                }
                else
                {
                    if(entry.AutoResize)
                    {
                        var instance = Object.Instantiate(entry.Template);
                        instance.key = key;
                        instance.gameObject.SetActive(true);
                        return instance;
                    }
                    else
                    {
                        throw new UnityException("Pool " + key + " is empty (no resize).");
                    }
                }
            }
            else
            {
                throw new UnityException("Entry for '" + key + "' does not exist.");
            }
        }

        public static T Get<T>(string key) where T : MonoBehaviour
        {
            PooledObject poolObj = Get(key);
            T objComponent = poolObj.GetComponent<T>();
            return objComponent;
        }

        public static void Insert(PooledObject pooledObject)
        {
            PoolEntry entry;
            if (_entries.TryGetValue(pooledObject.key, out entry))
            {
                entry.RuntimeInstances.Push(pooledObject);
            }
            else
            {
                Debug.LogError("Trying to insert pooled object on non registered entry.");
            }
        }
    }

    public struct PoolEntry
    {
        public PooledObject Template;
        public bool AutoResize;
        public Stack<PooledObject> RuntimeInstances;

        public PoolEntry(PooledObject entry, bool autoResize)
        {
            Template = entry;
            AutoResize = autoResize;
            RuntimeInstances = new Stack<PooledObject>();
        }
    }
}
