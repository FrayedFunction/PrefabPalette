using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PrefabPalette
{
    public static class ScenePlacer
    {
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
