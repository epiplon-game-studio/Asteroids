using UnityEngine;

namespace Asteroids.General
{
    public class ScreenBounds : MonoBehaviour
    {
        static new Camera camera;
        static Vector2 maxWorldPoint;
        static Vector2 minWorldPoint;

        public static Vector2 MaxPoint { get { return maxWorldPoint; } }
        public static Vector2 MinPoint { get { return minWorldPoint; } }

        [Tooltip("What to do when this object gets out of the screen bounds")]
        public OutofBoundsAction OutsideAction = OutofBoundsAction.Teleport;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            camera = Camera.main; // cache camera reference
            maxWorldPoint = camera.ViewportToWorldPoint(new Vector2(1, 1));
            minWorldPoint = camera.ViewportToWorldPoint(new Vector2(0, 0));
        }

        void Update()
        {
            if (transform.position.y > maxWorldPoint.y)
            {
                if (OutsideAction == OutofBoundsAction.Teleport)
                    transform.position = new Vector2(transform.position.x, minWorldPoint.y);
                else if (OutsideAction == OutofBoundsAction.Disable)
                    gameObject.SetActive(false);
            }

            if (transform.position.y < minWorldPoint.y)
            {
                if (OutsideAction == OutofBoundsAction.Teleport)
                    transform.position = new Vector2(transform.position.x, maxWorldPoint.y);
                else if (OutsideAction == OutofBoundsAction.Disable)
                    gameObject.SetActive(false);
            }

            if (transform.position.x > maxWorldPoint.x)
            {
                if (OutsideAction == OutofBoundsAction.Teleport)
                    transform.position = new Vector2(minWorldPoint.x, transform.position.y);
                else if (OutsideAction == OutofBoundsAction.Disable)
                    gameObject.SetActive(false);
            }

            if (transform.position.x < minWorldPoint.x)
            {
                if (OutsideAction == OutofBoundsAction.Teleport)
                    transform.position = new Vector2(maxWorldPoint.x, transform.position.y);
                else if (OutsideAction == OutofBoundsAction.Disable)
                    gameObject.SetActive(false);
            }
        }
    }

    public enum OutofBoundsAction { Teleport, Disable }
}
