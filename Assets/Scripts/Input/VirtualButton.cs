using UnityEngine;

namespace Asteroids.Input
{
    public class VirtualButton
    {
        public string Name;
        public bool IsPressting { get { return isPressing; } }
        public bool IsDown { get { return down > 0; } }
        public bool IsUp { get { return up > 0; } }

        bool isPressing;

        float down = 0;
        float up = 0;
        float maxCooldown;

        public VirtualButton(string name, float cooldown)
        {
            this.Name = name;
            this.maxCooldown = cooldown;
        }

        public void Press()
        {
            isPressing = true;

            down = maxCooldown;
        }

        public void Release()
        {
            isPressing = false;

            up = maxCooldown;
        }

        public void Update()
        {
            down = Mathf.Clamp(down - Time.deltaTime, 0, maxCooldown);
            up = Mathf.Clamp(up - Time.deltaTime, 0, maxCooldown);
        }
    }
}
