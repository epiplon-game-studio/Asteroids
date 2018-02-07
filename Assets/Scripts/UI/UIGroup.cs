using UnityEngine;

namespace Asteroids.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIGroup : MonoBehaviour
    {
        public bool hideOnStart;

        CanvasGroup canvasGroup;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (hideOnStart) Hide();
            else Show();
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
