using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Input
{
    [CreateAssetMenu(fileName = "Keyboard Input", menuName = "Asteroids/Keyboard Input")]
    public class KeyboardInputSettings : InputSettings
    {
        public override float GetAxis(string axisName)
        {
            return UnityEngine.Input.GetAxis(axisName);
        }

        public override float GetAxisRaw(string axisName)
        {
            return UnityEngine.Input.GetAxisRaw(axisName);
        }

        public override bool GetButton(string buttonName)
        {
            return UnityEngine.Input.GetButton(buttonName);
        }

        public override bool GetButtonDown(string buttonName)
        {
            return UnityEngine.Input.GetButtonDown(buttonName);
        }

        public override bool GetButtonUp(string buttonName)
        {
            return UnityEngine.Input.GetButtonUp(buttonName);
        }
    }

}
