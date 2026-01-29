### Versioning

Prefab Palette follows [Semantic Versioning](https://semver.org/) — `MAJOR.MINOR.PATCH`.

Please update the version appropriately if your contribution changes behavior or functionality:

- **MAJOR** – Incompatible API or system changes.
- **MINOR** – New features or functionality (backwards-compatible).
- **PATCH** – Bug fixes, internal refactors, or documentation updates.

> ✅ Example: If you add a new placement mode without breaking anything else, bump from `1.2.0` to `1.3.0`.

---

## Distribution

Prefab Palette is distributed as a `.unitypackage` from the `Releases` section of the repo.

### 1. Prepare for Export

1. Open `Window > Prefab Palette > Collections Manager`, choose `Manage Collections` and remove all items from the list.
2. Open `PrefabPalette/Editor/Generated` folder. 
3. Delete everything except `CollectionName.cs`
4. Ensure no prefabs or other third-party assets are contained in the package. ⚠️

### 2. Export `.unitypackage`
1. In the **Project** window, select the `PrefabPalette` folder.
2. Right-click and choose **Export Package...**
3. In the export dialog, **deselect** the following:
   - Include dependencies
   - Include all scripts
   - Source icon/img files (e.g .kra , .psd etc)
4. **Export** package and name it:  
   `PrefabPalette_vX.Y.Z.unitypackage`
      
> Attach package to a new github release with the same version tag.

⚠️ Ensure no prefabs or other assets are contained in the release! ⚠️
