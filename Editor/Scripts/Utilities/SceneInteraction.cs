using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace PrefabPalette
{
    public static class SceneInteraction
    {
        static ToolSettings Settings => ToolContext.Instance.Settings;

        /// <summary>
        /// Is snapping on?
        /// </summary>
        public static bool IsSnapActive
        {
            get
            {
                return EditorSnapSettings.gridSnapActive || EditorSnapSettings.incrementalSnapActive;
            }
        }

        /// <summary>
        /// Current cursor possition.
        /// </summary>
        public static Vector3 Position { get; private set; }

        /// <summary>
        /// The world-space normal of the surface currently under the mouse cursor.
        /// </summary>
        public static Vector3 SurfaceNormal { get; private set; }

        static Vector3 snapReference;
        static bool hasSnapReference;

        static Vector2 lastMousePos;

        public static void OnEnable()
        {
            SceneView.duringSceneGui += UpdateRaycast;
            hasSnapReference = false;
            lastMousePos = Vector2.zero;
        }

        public static void OnDisable()
        {
            SceneView.duringSceneGui -= UpdateRaycast;
        }

        static void UpdateRaycast(SceneView sceneView)
        {
            Event e = Event.current;
            if (e == null)
                return;

            // Only process mouse movement events
            if (e.type != EventType.MouseMove &&
                e.type != EventType.MouseDrag)
                return;

            // Skip if mouse hasn't moved significantly
            if (Vector2.Distance(e.mousePosition, lastMousePos) < Settings.placer_mouseMoveThreshold)
                return;

            lastMousePos = e.mousePosition;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    Settings.placer_maxRaycastDistance,
                    Settings.placer_includeMask,
                    QueryTriggerInteraction.Ignore)) // Ignore trigger colliders.
                return;

            SurfaceNormal = hit.normal;
            Vector3 rawPosition = hit.point;

            if (!IsSnapActive)
            {
                Position = rawPosition;
                hasSnapReference = false;
                return;
            }

            if (!hasSnapReference)
            {
                snapReference = rawPosition;
                hasSnapReference = true;
            }

            Vector3 delta = rawPosition - snapReference;
            Vector3 snappedDelta = Handles.SnapValue(delta, EditorSnapSettings.move);
            Position = snapReference + snappedDelta;
        }
    }
}
