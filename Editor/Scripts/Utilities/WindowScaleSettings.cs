using UnityEngine;

namespace PrefabPalette
{
    [System.Serializable]
    public class WindowScaleSettings
    {
        public bool useGlobal = true;
        public Vector2 minSize = new(100f, 100f);
        public Vector2 maxSize = new(700f, 700f);

        public void Resolve(Vector2 globalMin, Vector2 globalMax, out Vector2 resolvedMin, out Vector2 resolvedMax)
        {
            if (useGlobal)
            {
                resolvedMin = globalMin;
                resolvedMax = globalMax;
            }
            else
            {
                resolvedMin = minSize;
                resolvedMax = maxSize;
            }
        }
    }
}
