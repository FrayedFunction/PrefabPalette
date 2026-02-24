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
            if (e.type != EventType.MouseMove && e.type != EventType.MouseDrag)
                return;

            // Skip if mouse hasn't moved significantly
            if (Vector2.Distance(e.mousePosition, lastMousePos) < Settings.placer_mouseMoveThreshold)
                return;

            lastMousePos = e.mousePosition;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (!TryResolvePosition(ray, out Vector3 position, out Vector3 normal))
                return;

            SurfaceNormal = normal;

            if (!IsSnapActive)
            {
                Position = position;
                hasSnapReference = false;
                return;
            }

            if (!hasSnapReference)
            {
                snapReference = position;
                hasSnapReference = true;
            }

            Vector3 delta = position - snapReference;
            Position = snapReference + Handles.SnapValue(delta, EditorSnapSettings.move);
        }

        /// <summary>
        /// Tries to resolve ray hit position and normal with a priority chain:
        /// <list type="number">
        /// <item>3D raycast</item>
        /// <item>2D raycast</item>
        /// <item>2D fixed depth placement.</item>
        /// <item>XZ ground plane projection.</item>
        /// </list>
        /// </summary>
        /// <param name="ray">Ray fired from mouse position</param>
        /// <param name="position">Resolved world hit position</param>
        /// <param name="normal">Resolved world hit normal</param>
        /// <returns>
        /// true if hit was resolved.
        /// </returns>
        static bool TryResolvePosition(Ray ray, out Vector3 position, out Vector3 normal)
        {
            // Priority 1: 3D raycast.
            if (Physics.Raycast(
                    ray,
                    out RaycastHit hit3D,
                    Settings.placer_maxRaycastDistance,
                    Settings.placer_includeMask,
                    QueryTriggerInteraction.Ignore))
            {
                position = hit3D.point;
                normal = hit3D.normal;
                return true;
            }

            // Priority 2: 2D raycast.
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(
                ray,
                Settings.placer_maxRaycastDistance,
                Settings.placer_includeMask);

            if (hit2D.collider != null)
            {
                position = hit2D.point;
                position.z = hit2D.transform.position.z;
                normal = Vector3.back;
                return true;
            }

            // Priority 3: 2D fixed depth placement
            if (SceneView.lastActiveSceneView.in2DMode)
            {
                position = ray.origin;
                position.z = Settings.placer_2dDepth;
                normal = Vector3.back;
                return true;
            }

            // Priority 4: XZ ground plane projection.
            Plane fallbackPlane = new(Vector3.up, Vector3.zero);
            if (fallbackPlane.Raycast(ray, out float enter))
            {
                position = ray.GetPoint(enter);
                normal = fallbackPlane.normal;
                return true;
            }

            position = Vector3.zero;
            normal = Vector3.zero;
            return false;
        }
    }
}
