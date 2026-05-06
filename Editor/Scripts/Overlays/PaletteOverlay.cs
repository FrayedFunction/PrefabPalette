using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrefabPalette
{
    /// <summary>
    /// Palette Overlay for Scene View.
    /// </summary>
    [Overlay(typeof(SceneView), "Prefab Palette")]
    public class PaletteOverlay : Overlay
    {
        PaletteGUI gui;
        IMGUIContainer imguiContainer;

        public override VisualElement CreatePanelContent()
        {
            imguiContainer = new IMGUIContainer(OnGUI);

            imguiContainer.style.width = ToolContext.Instance.Settings.palette_overlayScale.x;
            imguiContainer.style.height = ToolContext.Instance.Settings.palette_overlayScale.y;

            return imguiContainer;
        }

        public override void OnCreated()
        {   
            collapsedIcon = Resources.Load<Texture2D>($"Imgs/PaletteIcon");

            PrefabCollectionList.Instance.Sync();

            gui = new PaletteGUI();

            // Make the overlay resizable
            collapsedChanged += OnCollapsedChanged;
            displayedChanged += OnDisplayChanged;
        }

        public override void OnWillBeDestroyed()
        {
            var context = ToolContext.Instance;
            context.IsPaletteOverlayOpen = false;
            context.OnDisable();

            collapsedChanged -= OnCollapsedChanged;
            displayedChanged -= OnDisplayChanged;
        }

        private void OnCollapsedChanged(bool collapsed)
        {
            if (!collapsed)
                imguiContainer?.MarkDirtyRepaint();
        }

        private void OnDisplayChanged(bool isDisplayed)
        {
            if (isDisplayed)
            {
                ToolContext.Instance.OnEnable();
            }
            else
            {
                ToolContext.Instance.OnDisable();
            }

            ToolContext.Instance.IsPaletteOverlayOpen = isDisplayed;
        }

        void OnGUI()
        {
            float availableWidth = imguiContainer.layout.width;
            if (availableWidth <= 0)
                availableWidth = 300; // Fallback width

            gui.Draw(availableWidth);
        }
    }
}