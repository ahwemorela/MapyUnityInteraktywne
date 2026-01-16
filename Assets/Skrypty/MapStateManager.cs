using System.Collections.Generic;
using UnityEngine;

namespace Mapy2D.Map
{
    public class MapStateManager : MonoBehaviour
    {
        [Header("Interactive elements")]
        [SerializeField] private List<InteractiveItem> interactiveItems = new List<InteractiveItem>();
        [SerializeField] private CenterItem centerItem;

        [Header("Unlock rule")]
        [Tooltip("0 lub mniej = odblokuj po odwiedzeniu wszystkich")]
        [SerializeField] private int unlockAfterVisited;

        private readonly HashSet<InteractiveItem> visitedItems = new HashSet<InteractiveItem>();
        private InteractiveItem currentSelection;
        private bool centerUnlocked;

        public IReadOnlyList<InteractiveItem> InteractiveItems => interactiveItems;
        public bool CenterUnlocked => centerUnlocked;

        private void Start()
        {
            RefreshAllVisuals();
            RecalculateUnlock();
        }

        // ---------- REGISTRATION ----------

        public void AddInteractive(InteractiveItem item)
        {
            if (item == null || interactiveItems.Contains(item))
            {
                return;
            }

            interactiveItems.Add(item);
            RefreshAllVisuals();
            RecalculateUnlock();
        }

        public void RemoveInteractive(InteractiveItem item)
        {
            if (item == null)
            {
                return;
            }

            interactiveItems.Remove(item);
            visitedItems.Remove(item);

            if (currentSelection == item)
            {
                currentSelection = null;
            }

            RefreshAllVisuals();
            RecalculateUnlock();
        }

        // ---------- SELECTION LOGIC ----------

        public void Select(InteractiveItem item)
        {
            if (item == null)
            {
                return;
            }

            // Bezpieczeństwo: tylko elementy z listy
            if (!interactiveItems.Contains(item))
            {
                return;
            }

            // TOGGLE: klik w już zaznaczony element = odznaczenie
            if (currentSelection == item)
            {
                currentSelection = null;
                RefreshAllVisuals();
                RecalculateUnlock();
                return;
            }

            // Normalne zaznaczenie
            currentSelection = item;
            visitedItems.Add(item);

            RefreshAllVisuals();
            RecalculateUnlock();
        }

        // ---------- STATE QUERIES ----------

        public bool IsVisited(InteractiveItem item)
        {
            return item != null && visitedItems.Contains(item);
        }

        public bool IsSelected(InteractiveItem item)
        {
            return item != null && currentSelection == item;
        }

        public int GetVisitedCount()
        {
            return visitedItems.Count;
        }

        // ---------- CENTER UNLOCK ----------

        public void RecalculateUnlock()
        {
            int visitedCount = visitedItems.Count;
            int totalCount = CountInteractiveItems();

            bool shouldUnlock =
                unlockAfterVisited > 0
                    ? visitedCount >= unlockAfterVisited
                    : totalCount > 0 && visitedCount >= totalCount;

            SetCenterUnlocked(shouldUnlock);
        }

        public void ResetAll()
        {
            visitedItems.Clear();
            currentSelection = null;
            centerUnlocked = false;

            RefreshAllVisuals();
            UpdateCenterVisuals();
        }

        private void SetCenterUnlocked(bool unlocked)
        {
            if (centerUnlocked == unlocked)
            {
                return;
            }

            centerUnlocked = unlocked;
            UpdateCenterVisuals();
        }

        private void UpdateCenterVisuals()
        {
            if (centerItem == null)
            {
                return;
            }

            centerItem.SetUnlocked(centerUnlocked);
        }

        // ---------- VISUAL REFRESH ----------

        private void RefreshAllVisuals()
        {
            foreach (var item in interactiveItems)
            {
                if (item == null)
                {
                    continue;
                }

                item.RefreshVisual();
            }
        }

        private int CountInteractiveItems()
        {
            int count = 0;
            foreach (var item in interactiveItems)
            {
                if (item != null)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
