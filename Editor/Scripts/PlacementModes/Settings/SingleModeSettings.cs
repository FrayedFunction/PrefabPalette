using UnityEngine;

namespace PrefabPalette
{
    public class SingleModeSettings : PlacementModeSettings
    {
        public Vector3 freeMode_placementOffset = Vector3.zero;
        public float freeMode_rotationSpeed = 2f;
        [Range(0, 2)]
        public int selectedRotationAxis = 0;
    }
}
