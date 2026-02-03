using UnityEngine;
using UnityEditor;

namespace PrefabPalette
{
    public static class RotationAngleSnapHelper
    {
        // Tracks total rotation since drag started
        private static float rotationReference;
        private static float rotationApplied;

        /// <summary>
        /// Reset internal state.
        /// </summary>
        public static void Reset()
        {
            rotationReference = 0f;
            rotationApplied = 0f;
        }

        /// <summary>
        /// Rotate a Transform around the given axis based on mouse delta, respecting Unity's rotation snap.
        /// </summary>
        /// <param name="target">Transform to rotate</param>
        /// <param name="mouseDeltaX">Mouse movement in pixels along X</param>
        /// <param name="rotationSpeed">Degrees per pixel</param>
        /// <param name="axis">Axis to rotate around</param>
        public static void RotateWithSnap(Transform target, float mouseDeltaX, float rotationSpeed, Vector3 axis)
        {
            // accumulate raw delta
            rotationReference += mouseDeltaX * rotationSpeed;

            float snapIncrement = EditorSnapSettings.rotate; // degrees per step
            float targetRotation = snapIncrement > 0f
                ? Handles.SnapValue(rotationReference, snapIncrement)
                : rotationReference;

            float deltaToApply = targetRotation - rotationApplied;

            target.Rotate(axis, deltaToApply, Space.World);

            rotationApplied += deltaToApply;
        }
    }
}
