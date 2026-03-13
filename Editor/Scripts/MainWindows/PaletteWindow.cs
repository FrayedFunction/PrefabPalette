using PrefabPalette;
using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    public class PaletteWindow : EditorWindow
    {
        PaletteGUI paletteGUI;

        ToolSettings Settings => ToolContext.Instance.Settings;

        [MenuItem("Window/Prefab Palette/Palette")]
        public static void Open()
        {
            GetWindow<PaletteWindow>("Prefab Palette");
            PrefabCollectionList.Instance.Sync();          
        }

        void OnEnable()
        {
            ToolContext.Instance.OnEnable();
            ToolContext.Instance.IsPaletteWindowOpen = true;

            paletteGUI = new PaletteGUI();

            Settings.paletteWindowScale.Resolve(
                Settings.globalMinWindowScale,
                Settings.globalMaxWindowScale,
                out Vector2 min,
                out Vector2 max
            );

            minSize = min;
            maxSize = max;
        }

        private void OnDisable()
        {
            var context = ToolContext.Instance;
            context.OnDisable();
            context.IsPaletteWindowOpen = false;
        }

        private void OnGUI()
        {
            paletteGUI.Draw(EditorGUIUtility.currentViewWidth - 10);
        }
    }
}
