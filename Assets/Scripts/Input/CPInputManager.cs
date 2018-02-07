using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Input
{
    public class CPInputManager : MonoBehaviour {

        [SerializeField]
        private InputSettings inputSettings; // input conifguration

        // singleton reference
        public static CPInputManager Instance = null;

        public delegate bool ButonAction(string buttonName);
        public delegate float AxisAction(string axisName);

        // points to the configured input methods

        public ButonAction GetButton;
        public ButonAction GetButtonDown;
        public ButonAction GetButtonUp;

        public AxisAction GetAxis;
        public AxisAction GetAxisRaw;

        private void Awake()
        {
            GetButton = inputSettings.GetButton;
            GetButtonDown = inputSettings.GetButtonDown;
            GetButtonUp = inputSettings.GetButtonUp;

            GetAxis = inputSettings.GetAxis;
            GetAxisRaw = inputSettings.GetAxisRaw;

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
}
