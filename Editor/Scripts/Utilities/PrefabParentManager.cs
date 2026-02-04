using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PrefabPalette
{
    /// <summary>
    /// Provides helpers for parenting prefabs.
    /// </summary>
    public static class PrefabParentManager
    {
        // TODO: Add support for custom parenting options.
        /// <summary>
        /// Returns the appropriate parent transform for a prefab, depending on whether
        /// it is being placed in a prefab stage or the active scene.
        /// </summary>
        /// <remarks>
        /// <paramref name="prefab"/> Currently unused; included for future custom parenting options.
        /// </remarks>
        /// <returns>
        /// The parent transform, or null if the prefab should be placed in the active scene.
        /// </returns>
        public static Transform GetAppropriateParent(GameObject prefab)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.stageHandle.IsValid())
            {
                return prefabStage.prefabContentsRoot.transform;
            }

            // Null parent is simply a transform in the active scene
            return null;
        }
    }
}
