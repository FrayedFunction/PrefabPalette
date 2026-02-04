using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PrefabPalette
{
    /// <summary>
    /// Maintains valid paths to the tools root and collections folder, Saves current path to Editor Prefs.
    /// </summary>
    [InitializeOnLoad]
    public static class PathDr
    {
        // Keys for editor prefs
        private const string ToolPathKey = "PrefabPalette/PathDr_ToolPath";
        private const string CollectionsPathKey = "PrefabPalette/PathDr_CollectionsPath";

        private static string toolPath;
        private static string generatedFolderPath;
        private static string collectionsPath;
        private static string modeSettingsPath;

        static PathDr()
        {
            EditorApplication.delayCall += Init;
        }

        static void Init()
        {
            toolPath = EditorPrefs.GetString(ToolPathKey, string.Empty);
            collectionsPath = EditorPrefs.GetString(CollectionsPathKey, string.Empty);

            // If the path for the tools root folder isn't set or the directory no longer
            // exists, look for it in the asset database.
            if (string.IsNullOrEmpty(toolPath) || !Directory.Exists(toolPath))
            {
                var root = FindFolder("PrefabPalette");

                if (string.IsNullOrEmpty(root))
                {
                    Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Can't find PrefabPalette folder!");
                    return;
                }

                toolPath = Path.Combine(root, "Editor");

                // Verify the Editor folder exists
                if (!Directory.Exists(toolPath))
                {
                    Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Can't find Editor folder at '{toolPath}'!");
                    return;
                }

                // Save the valid path to editor prefs
                EditorPrefs.SetString(ToolPathKey, toolPath);
            }

            generatedFolderPath = Path.Combine(toolPath, "Generated");
            ValidateFolderPath(generatedFolderPath);

            collectionsPath = Path.Combine(generatedFolderPath, "Collections");
            ValidateFolderPath(collectionsPath);

            modeSettingsPath = Path.Combine(generatedFolderPath, "Mode Settings");
            ValidateFolderPath(modeSettingsPath);
        }

        private static bool ValidateFolderPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                Debug.LogError($"PrefabPalette/{nameof(ValidateFolderPath)}: Received null or empty path!");
                return false;
            }

            if (!Directory.Exists(fullPath))
            {
                // Get parent folder and new folder name
                string parent = Path.GetDirectoryName(fullPath);
                string folderName = Path.GetFileName(fullPath);

                if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName))
                {
                    Debug.LogError($"PrefabPalette/{nameof(ValidateFolderPath)}: Invalid path '{fullPath}'");
                    return false;
                }

                // Check if parent exists before trying to create subfolder
                if (!Directory.Exists(parent))
                {
                    Debug.LogError($"PrefabPalette/{nameof(ValidateFolderPath)}: Parent directory doesn't exist: '{parent}'");
                    return false;
                }

                string newGUID = AssetDatabase.CreateFolder(parent, folderName);

                if (string.IsNullOrEmpty(newGUID))
                {
                    Debug.LogError($"PrefabPalette/{nameof(ValidateFolderPath)}: Failed to create folder '{folderName}' in '{parent}'");
                    return false;
                }

                string createdPath = AssetDatabase.GUIDToAssetPath(newGUID);

                Debug.Log($"PrefabPalette/{nameof(ValidateFolderPath)}: Folder '{folderName}' created successfully at '{createdPath}'. Refreshing AssetDatabase...");
                AssetDatabase.Refresh();
            }

            return true;
        }


        /// <returns>
        /// Path to /PrefabPalette/Editor
        /// </returns>
        /// <returns>
        /// Path to /PrefabPalette/Editor
        /// </returns>
        public static string GetToolPath
        {
            get
            {
                if (string.IsNullOrEmpty(toolPath))
                {
                    // Try to find and initialize the tool path
                    var root = FindFolder("PrefabPalette");

                    if (string.IsNullOrEmpty(root))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Can't find PrefabPalette folder!");
                        return string.Empty;
                    }

                    // Verify the root folder actually exists
                    if (!Directory.Exists(root))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: PrefabPalette folder path is invalid: '{root}'");
                        return string.Empty;
                    }

                    toolPath = Path.Combine(root, "Editor");

                    // Verify the Editor folder exists
                    if (!Directory.Exists(toolPath))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Can't find Editor folder at '{toolPath}'!");
                        toolPath = string.Empty; // Reset it so we don't cache a bad path
                        return string.Empty;
                    }

                    // Save the valid path to editor prefs
                    EditorPrefs.SetString(ToolPathKey, toolPath);
                }

                return toolPath;
            }
        }

        public static string GetGeneratedFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(generatedFolderPath))
                {
                    if (string.IsNullOrEmpty(GetToolPath))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Cannot get Generated folder - toolPath is not initialized!");
                        return string.Empty;
                    }

                    generatedFolderPath = Path.Combine(GetToolPath, "Generated");
                    ValidateFolderPath(generatedFolderPath);
                }

                return generatedFolderPath ?? string.Empty;
            }
        }

        /// <summary>
        /// Returns the path to the folder where prefab collections are generated.
        /// </summary>
        /// <remarks>
        /// Note: Always assumed to be in the tools root folder,
        /// a new one will be created here if it's moved or deleted
        /// </remarks>
        public static string GetCollectionsFolder
        {
            get
            {
                if (string.IsNullOrEmpty(collectionsPath))
                {
                    string genFolder = GetGeneratedFolderPath;

                    if (string.IsNullOrEmpty(genFolder))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Cannot get Collections folder - Generated folder is not available!");
                        return string.Empty;
                    }

                    collectionsPath = Path.Combine(genFolder, "Collections");
                    ValidateFolderPath(collectionsPath);
                }

                return collectionsPath ?? string.Empty;
            }
        }

        public static string GetModeSettingsFolder
        {
            get
            {
                if (string.IsNullOrEmpty(modeSettingsPath))
                {
                    string genFolder = GetGeneratedFolderPath;

                    if (string.IsNullOrEmpty(genFolder))
                    {
                        Debug.LogError($"PrefabPalette/{nameof(PathDr)}: Cannot get Mode Settings folder - Generated folder is not available!");
                        return string.Empty;
                    }

                    modeSettingsPath = Path.Combine(genFolder, "Mode Settings");
                    ValidateFolderPath(modeSettingsPath);
                }

                return modeSettingsPath ?? string.Empty;
            }
        }

        /// <returns>
        /// Path of <paramref name="folderName"/> from asset database, or null if not found
        /// </returns>
        private static string FindFolder(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                Debug.LogError($"PrefabPalette/{nameof(FindFolder)}: Received null or empty folder name!");
                return null;
            }

            try
            {
                string[] guids = AssetDatabase.FindAssets($"t:Folder {folderName}");

                if (guids == null || guids.Length == 0)
                {
                    return null;
                }

                return guids.Select(AssetDatabase.GUIDToAssetPath)
                            .FirstOrDefault(path => !string.IsNullOrEmpty(path) && path.EndsWith(folderName));
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"PrefabPalette/{nameof(FindFolder)}: Exception while searching for folder '{folderName}': {ex.Message}");
                return null;
            }
        }
    }
}