using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    /// <summary>
    /// Singleton context class that holds shared tool data and settings
    /// used across the Prefab Palette tool.
    /// </summary>
    public class ToolContext
    {
        private static ToolContext instance;

        /// <summary>
        /// Singleton instance of the ToolContext.
        /// Initialized on first access.
        /// </summary>
        public static ToolContext Instance => instance ??= new();

        public ToolSettings Settings { get; private set; }

        /// <summary>
        /// Currently selected prefab in the palette.
        /// </summary>
        public GameObject SelectedPrefab { get; set; }

        /// <summary>
        /// The active game object new prefabs should be instatiated as children of.
        /// </summary>
        public GameObject ParentObj 
        { 
            get 
            { 
                return parentObj; 
            }
            set
            {
                if (value != null && value.scene.IsValid())
                {
                    parentObj = value;
                    return;
                }

                parentObj = null;
            } 
        }

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// Loads or creates the ToolSettings asset on instantiation.
        /// </summary>
        ToolContext()
        {
            Settings = Helpers.LoadOrCreateAsset<ToolSettings>(PathDr.GetGeneratedFolderPath, "ToolSettings.asset", out _);
        }

        private GameObject parentObj;
        
        /// <summary>
        /// Is the Palette Overlay enabled in the scene view?
        /// </summary>
        public bool IsPaletteOverlayOpen { get; set; }

        /// <summary>
        /// returns true if overlay or window is open
        /// </summary>
        public bool IsPaletteOpen 
        { 
            get 
            {
                return EditorWindow.HasOpenInstances<PaletteWindow>() || IsPaletteOverlayOpen;
            } 
        }

        public void OnEnable()
        {
            VisualPlacer.OnEnable();
            SceneInteraction.OnEnable();
            PlacementModeManager.OnEnable();
        }

        public void OnDisable()
        {
            if (IsPaletteOpen)
                return;

            VisualPlacer.OnDisable();
            SceneInteraction.OnDisable();
            PlacementModeManager.OnDisable();
        }
    }
}
