using UnityEngine;
using UnityEngine.Events;

namespace Mapy2D.Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class CenterItem : MonoBehaviour
    {
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private UnityEvent onCenterClicked;

        private SpriteRenderer spriteRenderer;
        private Collider2D itemCollider;
        private bool isUnlocked;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            itemCollider = GetComponent<Collider2D>();
            ApplyVisuals();
        }

        public void SetUnlocked(bool unlocked)
        {
            isUnlocked = unlocked;
            ApplyVisuals();
        }

        public void OnClicked()
        {
            if (!isUnlocked)
            {
                return;
            }

            onCenterClicked?.Invoke();
            Debug.Log("Center clicked: FinalTriggered");
        }

        private void ApplyVisuals()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isUnlocked ? unlockedColor : lockedColor;
            }

            if (itemCollider != null)
            {
                itemCollider.enabled = isUnlocked;
            }
        }
    }
}
