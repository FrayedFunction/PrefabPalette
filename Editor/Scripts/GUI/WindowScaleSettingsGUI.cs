using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    public class WindowScaleSettingsGUI : MonoBehaviour
    {
        public static void Draw(WindowScaleSettings settings, string boolLabel = "Use global window scale")
        {
            settings.useGlobal = EditorGUILayout.ToggleLeft(boolLabel, settings.useGlobal);

            if (!settings.useGlobal)
            {
                settings.minSize = EditorGUILayout.Vector2Field("Min Size", settings.minSize);
                settings.maxSize = EditorGUILayout.Vector2Field("Max Size", settings.maxSize);

                // Only minSize needs to be clamped to a floor as maxSize is always >= minSize.
                settings.minSize = Vector2.Max(settings.minSize, Vector2.one);

                // Ensure max never goes below min.
                settings.maxSize = Vector2.Max(settings.maxSize, settings.minSize);
            }
        }
    }
}
