using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    public abstract class PlacementModeSettings : ScriptableObject
    {
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
