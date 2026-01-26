using UnityEngine;
using UnityEditor;

namespace PrefabPalette
{
    public class GlobalSettingsWindow : EditorWindow
    {
        ToolSettings Settings => ToolContext.Instance.Settings;
        Vector2 scrollPos;

        [MenuItem("Window/Prefab Palette/Settings")]
        public static void OpenWindow()
        {
            var window = GetWindow<GlobalSettingsWindow>("Prefab Palette: Settings");
        }

        private void OnEnable()
        {
            minSize = new Vector2(300, 500);
            maxSize = new Vector2(350, 550);
        }

        private void OnGUI() 
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            Helpers.TitleText("Prefab Palette: Settings");
            Helpers.DrawLine(Color.gray);

            // Palette Settings
            GUILayout.Label("Palette:", EditorStyles.whiteLargeLabel);
            EditorGUI.indentLevel++;
            Settings.palette_gridColumns = Mathf.Max(1, EditorGUILayout.IntField("Columns", Settings.palette_gridColumns));
            Settings.palette_minThumbnailScale = Mathf.Clamp(EditorGUILayout.FloatField("Min Thumbnail Scale", Settings.palette_minThumbnailScale), 50f, Settings.palette_maxThumbnailScale);
            Settings.palette_maxThumbnailScale = Mathf.Clamp(EditorGUILayout.FloatField("Max Thumbnail Scale", Settings.palette_maxThumbnailScale), Settings.palette_minThumbnailScale, 500f);
            EditorGUI.indentLevel--;
            
            GUILayout.Space(4);

            // Placer Setttings
            GUILayout.Label("Placer:", EditorStyles.whiteLargeLabel);
            EditorGUI.indentLevel++;
            Settings.placer_includeMask = LayerMaskField("Include Layers", Settings.placer_includeMask);
            Settings.placer_color = EditorGUILayout.ColorField("Color", Settings.placer_color);
            Settings.placer_radius = Mathf.Max(0.01f, EditorGUILayout.FloatField("Radius", Settings.placer_radius));
            EditorGUI.indentLevel-- ;

            GUILayout.Space(4);

            // Overlay Settings
            GUILayout.Label("Overlay:", EditorStyles.whiteLargeLabel);
            EditorGUI.indentLevel++;
            Settings.overlay_autoSize = EditorGUILayout.Toggle("Auto Size?", Settings.overlay_autoSize);
            Settings.overlay_size = Settings.overlay_autoSize ? Vector2.zero : EditorGUILayout.Vector2Field("Size", Settings.overlay_size);
            EditorGUI.indentLevel--;

            GUILayout.Space(4);
            
            // Window Scale
            GUILayout.Label("Window Scaling:", EditorStyles.whiteLargeLabel);
            EditorGUI.indentLevel++;
            Settings.globalMinWindowScale = EditorGUILayout.Vector2Field("Global min", Settings.globalMinWindowScale);
            // Only min needs to be clamped to a floor as max floor is already clamped by min.
            Settings.globalMinWindowScale = Vector2.Max(Settings.globalMinWindowScale, Vector2.one);
            Settings.globalMaxWindowScale = EditorGUILayout.Vector2Field("Global max", Settings.globalMaxWindowScale);
            Settings.globalMaxWindowScale = Vector2.Max(Settings.globalMaxWindowScale, Settings.globalMinWindowScale);
            GUILayout.Space(2);
            Helpers.IndentedLabel("Palette", EditorGUI.indentLevel, EditorStyles.whiteLabel);
            WindowScaleSettingsGUI.Draw(Settings.paletteWindowScale);
            GUILayout.Space(1);
            Helpers.IndentedLabel("Collections Manager", EditorGUI.indentLevel, EditorStyles.whiteLabel);
            WindowScaleSettingsGUI.Draw(Settings.collectionsManagerWindowScale);
            GUILayout.Space(1);
            Helpers.IndentedLabel("Settings", EditorGUI.indentLevel, EditorStyles.whiteLabel);
            WindowScaleSettingsGUI.Draw(Settings.settingsWindowScale);
            EditorGUI.indentLevel--;

            GUILayout.Space(10);
            GUILayout.EndScrollView();
        }

        private LayerMask LayerMaskField(string label, LayerMask selected)
        {
            // Get all layer names
            string[] layerNames = new string[32];
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                layerNames[i] = string.IsNullOrEmpty(layerName) ? $"Layer {i}" : layerName;
            }

            selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);
            return selected;
        }
    }
}
