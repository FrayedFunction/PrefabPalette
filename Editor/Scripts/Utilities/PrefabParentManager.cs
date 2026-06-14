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
        /// <summary>
        /// Returns the appropriate parent transform for a prefab, depending on whether
        /// it is being placed in a prefab stage or the active scene.
        /// </summary>
        /// <returns>
        /// The parent transform, or null if the prefab should be placed in the active scene.
        /// </returns>
        public static Transform GetAppropriateParent()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.stageHandle.IsValid())
            {
                return prefabStage.prefabContentsRoot.transform;
            }

            var parent = ToolContext.Instance.ParentObj;
            if (parent == null)
                return null;

            return ToolContext.Instance.ParentObj.transform;
        }
    }
}
