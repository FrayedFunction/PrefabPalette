using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    public class PaletteGUI
    {
        Vector2 paletteScrollPosition;
        float dynamicPrefabIconSize = 50f;

        ToolSettings Settings => ToolContext.Instance.Settings;

        public float IconSize => dynamicPrefabIconSize;

        public void Draw(float availableWidth)
        {
            DrawCollectionSelector();

            if (Settings.CurrentPrefabCollection == null)
                return;

            // Cache all values that could change to ensure consistent GUI structure
            var settings = ToolContext.Instance.Settings;
            float scale = settings.palette_thumbnailScale;
            var collection = settings.CurrentPrefabCollection;
            
            var prefabList = collection.prefabList;

            // Calculate icon size once at the start
            dynamicPrefabIconSize = 50 * scale;

            GUILayout.Space(5);
            GUILayout.Label($"Palette - {collection.Name}", EditorStyles.boldLabel);
            GUILayout.Space(5);
            GUILayout.BeginVertical("box");

            paletteScrollPosition = GUILayout.BeginScrollView(paletteScrollPosition);
            DrawGrid(availableWidth, prefabList);
            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            DrawScaleSlider(scale);
        }

        private void DrawScaleSlider(float currentScale)
        {
            var settings = ToolContext.Instance.Settings;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Scale", GUILayout.Width(40));

            float newScale = GUILayout.HorizontalSlider(
                currentScale,
                0.5f,
                5.0f
            );

            // Only update if changed
            if (!Mathf.Approximately(newScale, currentScale))
            {
                settings.palette_thumbnailScale = newScale;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawGrid(float windowWidth, System.Collections.Generic.List<GameObject> prefabList)
        {
            if (prefabList == null || prefabList.Count == 0)
            {
                GUILayout.Label("No prefabs in collection", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            float padding = Mathf.Lerp(2f, dynamicPrefabIconSize * 0.15f, Settings.palette_thumbnailScale / 5.0f);
            float cellSize = dynamicPrefabIconSize + padding;

            int columns = Mathf.Max(1, Mathf.FloorToInt((windowWidth - padding) / cellSize));

            int rowCount = Mathf.CeilToInt((float)prefabList.Count / columns);

            float gridWidth = columns * cellSize;
            float gridPadding = Mathf.Max((windowWidth - gridWidth) * 0.5f, 0);

            int index = 0;

            for (int row = 0; row < rowCount; row++)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Height(cellSize));

                if (gridPadding > 0)
                    GUILayout.Space(gridPadding);

                for (int col = 0; col < columns; col++)
                {
                    if (index < prefabList.Count)
                    {
                        DrawPrefab(prefabList[index], padding);
                        index++;
                    }
                    else
                    {
                        GUILayoutUtility.GetRect(
                            dynamicPrefabIconSize,
                            dynamicPrefabIconSize,
                            GUILayout.Width(dynamicPrefabIconSize),
                            GUILayout.Height(dynamicPrefabIconSize)
                        );
                    }
                }

                if (gridPadding > 0)
                    GUILayout.Space(gridPadding);

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawPrefab(GameObject prefab, float padding)
        {
            if (!prefab)
            {
                GUILayoutUtility.GetRect(
                    dynamicPrefabIconSize,
                    dynamicPrefabIconSize,
                    GUILayout.Width(dynamicPrefabIconSize),
                    GUILayout.Height(dynamicPrefabIconSize)
                );
                return;
            }

            Texture2D preview = AssetPreview.GetAssetPreview(prefab);
            float labelHeight = dynamicPrefabIconSize * 0.25f;

            Rect totalRect = GUILayoutUtility.GetRect(
                dynamicPrefabIconSize,
                dynamicPrefabIconSize,
                GUILayout.Width(dynamicPrefabIconSize),
                GUILayout.Height(dynamicPrefabIconSize)
            );

            Rect buttonRect = new(
                totalRect.x + padding * 0.5f,
                totalRect.y + padding * 0.5f,
                totalRect.width - padding,
                totalRect.height - padding
            );

            bool isHovering = totalRect.Contains(Event.current.mousePosition);
            bool isSelected = ToolContext.Instance.SelectedPrefab == prefab;

            if (isSelected)
                EditorGUI.DrawRect(totalRect, new Color(0.1f, 0.5f, 1f, 1f));

            GUI.DrawTexture(
                buttonRect,
                preview ? preview : EditorGUIUtility.IconContent("Prefab Icon").image,
                ScaleMode.ScaleToFit
            );

            if (GUI.Button(totalRect, GUIContent.none, GUIStyle.none))
                ToolContext.Instance.SelectedPrefab =
                    isSelected ? null : prefab;

            if (isHovering || isSelected)
            {
                Rect labelRect = new(totalRect.x, totalRect.yMax - labelHeight, totalRect.width, labelHeight);
                EditorGUI.DrawRect(labelRect, new Color(0, 0, 0, 0.6f));
                GUI.Label(labelRect, prefab.name,
                    new GUIStyle(EditorStyles.whiteLabel) { alignment = TextAnchor.MiddleCenter });
            }
        }

        private void DrawCollectionSelector()
        {
            GUILayout.Space(5);
            Settings.CurrentCollectionName = (CollectionName)EditorGUILayout.EnumPopup("Prefab Collection", Settings.CurrentCollectionName);
            GUILayout.Space(5);
        }
    }
}