using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    /// <summary>
    /// Allows placing and rotating a single prefab in the scene using mouse input.
    /// </summary>
    public class SinglePrefabMode : IPlacementMode
    {
        SingleModeSettings modeSettings;
        GameObject currentPlacedObject;
        Vector3 lastSurfaceNormal;

        public SinglePrefabMode(PlacementModeSettings settings)
        {
            this.modeSettings = (SingleModeSettings)settings;
        }

        /// <summary>
        /// Called every frame while the mode is active. Handles prefab placement and rotation based on mouse input.
        /// </summary>
        /// <param name="context">The current tool context.</param>
        public void OnActive(ToolContext context)
        {
            Event e = Event.current;

            if (context.SelectedPrefab == null) 
                return;

            // Place object on left click
            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                VisualPlacer.Stop();

                RotationAngleSnapHelper.Reset();

                lastSurfaceNormal = SceneInteraction.SurfaceNormal;

                Transform parent = PrefabParentManager.GetAppropriateParent();
                currentPlacedObject = (GameObject)PrefabUtility.InstantiatePrefab(context.SelectedPrefab, parent);
                currentPlacedObject.transform.SetPositionAndRotation(SceneInteraction.Position + modeSettings.freeMode_placementOffset, context.Settings.placer_alignWithSurface ? Quaternion.FromToRotation(Vector3.up, lastSurfaceNormal) : Quaternion.identity);
                Undo.RegisterCreatedObjectUndo(currentPlacedObject, "Placed Prop");

                e.Use();
            }

            // Rotate while holding the mouse button
            if (e.type == EventType.MouseDrag && e.button == 0 && !e.alt && currentPlacedObject != null)
            {
                Vector3 axis = GetRotationAxis(context);
                RotationAngleSnapHelper.RotateWithSnap(currentPlacedObject.transform, e.delta.x, modeSettings.freeMode_rotationSpeed, axis);

                e.Use();
            }

            // Stop rotating on mouse release
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                VisualPlacer.ShowTarget();
                currentPlacedObject = null;
                e.Use();
            }
        }
        
        public void OnEnter(ToolContext context)
        {
        }

        public void OnExit(ToolContext context)
        {
        }

        /// <summary>
        /// Draws the settings GUI for this placement mode in the overlay.
        /// Allows configuration of rotation speed, placement offset, and alignment.
        /// </summary>
        /// <param name="context">The current tool context.</param>
        public void SettingsOverlayGUI(ToolContext context)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation Axis", GUILayout.Width(100));
            modeSettings.selectedRotationAxis = GUILayout.Toolbar(
                modeSettings.selectedRotationAxis,
                new[] { "X", "Y", "Z" }
            );
            EditorGUILayout.EndHorizontal();

            modeSettings.freeMode_rotationSpeed = EditorGUILayout.Slider("Rotation Speed", modeSettings.freeMode_rotationSpeed, 0.1f, 5);
            modeSettings.freeMode_placementOffset = EditorGUILayout.Vector3Field("Placement Offset", modeSettings.freeMode_placementOffset);
        }

        public string[] ControlsHelpBox => new string[]
        {
                "LMB", "Place Single Prefab",
                "Hold LMB", "Rotate",
                "Release LMB", "Place"
        };

        private Vector3 GetRotationAxis(ToolContext context)
        {
            Vector3 axis = modeSettings.selectedRotationAxis switch 
            {
                // X
                0 => new(1, 0, 0),
                // Y
                1 => new(0, 1, 0),
                // Z
                2 => new(0, 0, 1),

                // Fallback
                _ => Vector3.up
            };

            if (!context.Settings.placer_alignWithSurface)
                return axis;

            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, lastSurfaceNormal);

            return surfaceRotation * axis;
        }
    }
}
