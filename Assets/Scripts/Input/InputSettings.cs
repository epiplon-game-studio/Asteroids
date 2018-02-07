using UnityEngine;

namespace Asteroids.Input
{
    public abstract class InputSettings : ScriptableObject {

        public abstract bool GetButton(string buttonName);
        public abstract bool GetButtonDown(string buttonName);
        public abstract bool GetButtonUp(string buttonName);

        public abstract float GetAxis(string axisName);
        public abstract float GetAxisRaw(string axisName);
    }

}
