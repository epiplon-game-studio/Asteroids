using UnityEngine;

namespace Asteroids.General
{
    public class ScreenTeleporting : MonoBehaviour
    {
        static new Camera camera;
        static Vector2 maxWorldPoint;
        static Vector2 minWorldPoint;

        
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
                transform.position = new Vector2(transform.position.x, minWorldPoint.y);

            if (transform.position.y < minWorldPoint.y)
                transform.position = new Vector2(transform.position.x, maxWorldPoint.y);

            if (transform.position.x > maxWorldPoint.x)
                transform.position = new Vector2(minWorldPoint.x, transform.position.y);

            if (transform.position.x < minWorldPoint.x)
                transform.position = new Vector2(maxWorldPoint.x, transform.position.y);
        }
    }
}
