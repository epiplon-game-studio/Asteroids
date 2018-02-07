using Asteroids.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Test
{
    public class TestPoolParticle : MonoBehaviour
    {

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                var pooled = PoolManager.Get("explosion");
                pooled.gameObject.SetActive(true);
                pooled.transform.position = Vector3.zero;
            }
        }
    }
}
