using UnityEngine;

namespace Mapy2D.Map
{
    [CreateAssetMenu(menuName = "Map/Visual Settings", fileName = "VisualSettings")]
    public class VisualSettings : ScriptableObject
    {
        [System.Serializable]
        public struct VisualStateSettings
        {
            public Color color;
            public Vector3 scale;
        }

        public bool useScale;

        public VisualStateSettings normal = new VisualStateSettings
        {
            color = Color.white,
            scale = Vector3.one,
        };

        public VisualStateSettings hovered = new VisualStateSettings
        {
            color = Color.white,
            scale = Vector3.one,
        };

        public VisualStateSettings visited = new VisualStateSettings
        {
            color = Color.white,
            scale = Vector3.one,
        };

        public VisualStateSettings selected = new VisualStateSettings
        {
            color = Color.white,
            scale = Vector3.one,
        };

        public VisualStateSettings GetSettings(VisualState state)
        {
            switch (state)
            {
                case VisualState.Selected:
                    return selected;
                case VisualState.Hovered:
                    return hovered;
                case VisualState.Visited:
                    return visited;
                case VisualState.Normal:
                default:
                    return normal;
            }
        }
    }

    public enum VisualState
    {
        Normal,
        Hovered,
        Visited,
        Selected,
    }
}
