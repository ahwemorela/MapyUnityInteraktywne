using UnityEngine;

namespace Mapy2D.Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class InteractiveItem : MonoBehaviour
    {
        [SerializeField] private MapStateManager manager;
        [SerializeField] private VisualSettings visualSettings;
        [SerializeField] private string itemId;

        private SpriteRenderer spriteRenderer;
        private Vector3 baseScale;
        private bool isHovered;

        public string ItemId => itemId;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseScale = transform.localScale;
        }

        public void SetHovered(bool hovered)
        {
            if (isHovered == hovered)
            {
                return;
            }

            isHovered = hovered;
            RefreshVisual();
        }

        public void OnClicked()
        {
            if (manager == null)
            {
                return;
            }

            manager.Select(this);
        }

        public void RefreshVisual()
        {
            if (spriteRenderer == null || visualSettings == null)
            {
                return;
            }

            VisualState state = VisualState.Normal;

            if (manager != null && manager.IsSelected(this))
            {
                state = VisualState.Selected;
            }
            else if (isHovered)
            {
                state = VisualState.Hovered;
            }
            else if (manager != null && manager.IsVisited(this))
            {
                state = VisualState.Visited;
            }

            var settings = visualSettings.GetSettings(state);
            spriteRenderer.color = settings.color;

            if (visualSettings.useScale)
            {
                transform.localScale = Vector3.Scale(baseScale, settings.scale);
            }
            else
            {
                transform.localScale = baseScale;
            }
        }
    }
}
