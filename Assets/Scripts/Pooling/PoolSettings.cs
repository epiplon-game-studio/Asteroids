using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Pooling
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Asteroids/Pool")]
    public class PoolSettings : ScriptableObject
    {
        public PoolReg[] RegistryList;

        public PoolSettings Clone()
        {
            return (PoolSettings)MemberwiseClone();
        }
    }

    [System.Serializable]
    public struct PoolReg
    {
        public string Key;
        public int StartCapacity;
        public bool AutoResize;
        public PooledObject Template;
        public PoolCategory Category;
    }

    public enum PoolCategory { Enemy, Projectile, Particle }
}
