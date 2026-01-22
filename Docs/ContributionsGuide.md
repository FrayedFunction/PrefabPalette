## Prefab Palette: Contributing

---
## See Also
* [ReadMe](./README.md)
* [Developers](./Developers.md) 
---

Contributions are encouraged, whether fixing bugs, adding new modes, or improving documentation, your help is appreciated. 

## How to contribute

1. **Read the Docs:** Familiarise yourself with this guide and the [Developers](./Developers.md) doc. 

2. **Create or Claim an issue (If applicable):** Before starting significant work (e.g. new modes, features or bug fixes), it’s recommended to check the issues page to see if someone is already working on it. You can claim an open issue, or create a new one. This helps avoid duplicate work and allows for early feedback.

3. **Fork the repository:** Fork the Prefab Palette repository to your github account.

4. **Create a new branch:** For each new feature or bugfix, create a new branch from main. Use descriptive names for your branches (e.g., feature/add-new-mode, fix/bug-or-issue).

5. **Write clear commit messages:** Use concise and informative commit messages that explain the purpose of your changes.

6. **Submit a Pull Request (PR):** Once you’ve tested your changes, submit a pull request to the main branch of the original Prefab Palette repository.

## Contribution Guidelines

To ensure smooth collaboration and maintainability, please follow the guidelines outlined in this section.

### Code Style and Project Structure

Follow existing coventions and style within the project. Consistency improves readability and simplifies maintenance.

### Editor Folder Structure

When adding new files, follow the established folder structure within the Editor directory.

### New Placement Modes

If you’re creating a new placement mode, follow the steps outlined in the **Modes** section of the [Developers](./Developers.md) doc, this ensures your mode integrates seamlessly with the existing architecture.

Ensure the following:

1. Both the Mode and Settings script are in the correct folders.  
2. The modes settings asset is correctly generated/loaded in the appropriate subfolder within the `Generated` folder, using `PathDr` for path management.  
3. The new modes toolbar icon is setup correctly.
4. Any instantiated prefabs are integrated into Unity’s native Undo/Redo system.  

### Path Management

Use the `PathDr` script to access valid paths for the Generated, Collections, and Mode Settings folders.

### Documentation

If your contribution introduces new features or changes existing behavior, update the relevant sections of the docs to reflect changes.

### Pull Requests

Pull request should inculde the following:
* **Brief description**: Summary of what the PR does and why.
* **Type of change**: Bug fix, new feature, refactor, documentation update, etc.
* **Documentation**: List any updated docs (Do not update the changelog or version)
* **Related issues / PRs:** Reference any relevant issues or PRs (e.g, 'Closes #12')

### Versioning

Version numbers are managed by the maintainer at release time.  
Please **do not** update the package version, changelog, or add version tags as part of a pull request.
