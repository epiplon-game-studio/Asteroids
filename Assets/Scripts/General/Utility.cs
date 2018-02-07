using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.General
{
    public static class Utility
    {
        public static bool CompareLayer(this GameObject obj, LayerMask layerMask)
        {
            return ((1 << obj.layer) & layerMask) != 0;
        }
    }
}
