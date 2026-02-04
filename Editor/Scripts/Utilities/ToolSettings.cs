using UnityEngine;
using UnityEditor;

namespace PrefabPalette
{
    /// <summary>
    /// Holds persistent tool settings.
    /// </summary>
    public class ToolSettings : ScriptableObject
    {
        // Defualt collection name is .None
        public CollectionName CurrentCollectionName { get; set; } = CollectionName.None;
        public PrefabCollection CurrentPrefabCollection => PrefabCollection.GetOrCreateCollection(CurrentCollectionName);

        // Palette
        public float palette_minThumbnailScale = 50f;
        public float palette_maxThumbnailScale = 300f;
        public int palette_gridColumns = 4;

        // Placer
        public Color placer_color = Color.white;
        public float placer_radius = 0.2f;
        public LayerMask placer_includeMask = ~0; // masks to be included in scene interaction raycast. Default is everything.
        public bool placer_alignWithSurface = false;
        public float placer_mouseMoveThreshold = 0.5f;
        public float placer_maxRaycastDistance = 1000f;

        // Overlay
        public Vector2 overlay_size = new(420, 0);
        public bool overlay_autoSize;
        public bool overlay_showControlsHelpBox = true;

        // Window scale settings.
        public Vector2 globalMinWindowScale = new(100f, 100f);
        public Vector2 globalMaxWindowScale = new(1000f, 1000f);

        public WindowScaleSettings paletteWindowScale;
        public WindowScaleSettings settingsWindowScale;
        public WindowScaleSettings collectionsManagerWindowScale;

        // Marked dirty on disable so Unity knows to save it
        private void OnDisable()
        {
            EditorUtility.SetDirty(this);
            
            EditorApplication.delayCall += () =>
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            };
        }
    }
}
