### Versioning

Prefab Palette follows [Semantic Versioning](https://semver.org/) — `MAJOR.MINOR.PATCH`.

Please update the version appropriately if your contribution changes behavior or functionality:

- **MAJOR** – Incompatible API or system changes.
- **MINOR** – New features or functionality (backwards-compatible).
- **PATCH** – Bug fixes, internal refactors, or documentation updates.

> ✅ Example: If you add a new placement mode without breaking anything else, bump from `1.2.0` to `1.3.0`.

---

## Distribution

Prefab Palette is distributed as a zip containing a `.unitypackage` and a copy of core docs at the time of release.

### 1. Prepare for Export

1. Open `Window > Prefab Palette > Collections Manager`, choose `Manage Collections` and remove all items from the list.
2. Open `PrefabPalette/Editor/Generated` folder. 
3. Delete everything except `CollectionName.cs`
4. Ensure no prefabs or other third-party assets are contained in the package.
5. Create an empty folder named `PrefabPalette_vX.Y.Z` to contain the release.

### 2. Export `.unitypackage`
1. In the **Project** window, select the `PrefabPalette` folder.
2. Right-click the selection and choose **Export Package...**
3. In the export dialog:
   - Deselect **Include dependencies**
   - Deselect **Include all scripts**
   - Deselect `PrefabPalette/Docs` folder.
4. **Export** package to the empty folder created in step one, then name it:  
   `PrefabPalette_vX.Y.Z.unitypackage`

### 3. Include Core Docs
1. Create a new folder called `docs` in the same one as the unity package.
2. Copy the following (⚠️ Exclude `.meta` files!):
   * imgs
   * README.md
   * Changelog.md
   * License.md

### 4. Zip the contents
The zipped folder should now resemble the following structure:

      |- PrefabPalette_vX.Y.Z.zip
      |-- PrefabPalette_vX.Y.Z
      |--- docs
      |---- imgs
      |---- Changelog.md
      
> Attach zip to a new github release with the same version tag.

⚠️ Note: Ensure no prefabs or other assets are contained in the release!
