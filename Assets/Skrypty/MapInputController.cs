using UnityEngine;
using UnityEngine.InputSystem;

namespace Mapy2D.Map
{
    public class MapInputController : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private LayerMask interactableLayerMask = ~0;

        private InteractiveItem currentHovered;

        private void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPoint = GetWorldPoint(mousePosition);

            UpdateHover(worldPoint);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleClick(worldPoint);
            }
        }

        private Vector3 GetWorldPoint(Vector2 mousePosition)
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            Vector3 worldPoint = targetCamera.ScreenToWorldPoint(mousePosition);
            worldPoint.z = 0f;
            return worldPoint;
        }

        private void UpdateHover(Vector3 worldPoint)
        {
            InteractiveItem hoveredItem = GetInteractiveItemAt(worldPoint);

            if (currentHovered == hoveredItem)
            {
                return;
            }

            if (currentHovered != null)
            {
                currentHovered.SetHovered(false);
            }

            currentHovered = hoveredItem;

            if (currentHovered != null)
            {
                currentHovered.SetHovered(true);
            }
        }

        private void HandleClick(Vector3 worldPoint)
        {
            var hit = Physics2D.Raycast(worldPoint, Vector2.up, 0.01f, interactableLayerMask);

            if (hit.collider == null)
            {
                return;
            }

            InteractiveItem item = hit.collider.GetComponentInParent<InteractiveItem>();
            if (item != null)
            {
                item.OnClicked();
                return;
            }

            CenterItem center = hit.collider.GetComponentInParent<CenterItem>();
            if (center != null)
            {
                center.OnClicked();
            }
        }

        private InteractiveItem GetInteractiveItemAt(Vector3 worldPoint)
        {
            var hit = Physics2D.Raycast(worldPoint, Vector2.up, 0.01f, interactableLayerMask);
            if (hit.collider == null)
            {
                return null;
            }

            return hit.collider.GetComponentInParent<InteractiveItem>();
        }
    }
}
