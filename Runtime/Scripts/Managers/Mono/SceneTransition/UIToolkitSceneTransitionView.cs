using ArtificeToolkit.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace FireAlt.MonoSystems
{
    /// <summary>
    /// UI Toolkit transition view. Fades a named element of a <see cref="PanelRenderer"/> and, if present, drives a
    /// named progress element: either a <see cref="ProgressBar"/> control or a plain element whose width is filled.
    /// </summary>
    public class UIToolkitSceneTransitionView : SceneTransitionView
    {
        [Title("UI Toolkit")]
        [SerializeField] protected PanelRenderer panelRenderer;
        [Tooltip("Element to fade. Empty fades the document root.")]
        [SerializeField] protected string fadeElementName = "BlackPanel";
        [Tooltip("Optional ProgressBar, or an element whose width is set as a percentage.")]
        [SerializeField] protected string progressElementName = "ProgressBar";

        protected VisualElement fadeElement;
        protected VisualElement progressElement;

        protected override void Awake()
        {
            base.Awake();
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
            ((IPanelComponent)panelRenderer).PerformValidation(true);
        }

        protected virtual void OnDestroy()
        {
            if (panelRenderer != null)
            {
                panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            }
        }

        protected virtual void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
        {
            fadeElement = string.IsNullOrEmpty(fadeElementName) ? rootElement : rootElement.Q(fadeElementName);
            progressElement = string.IsNullOrEmpty(progressElementName) ? null : rootElement.Q(progressElementName);
            Refresh();
        }

        protected override void ApplyAlpha(float alpha)
        {
            if (fadeElement != null)
            {
                fadeElement.style.opacity = alpha;
            }
        }

        protected override void ApplyBlocksInput(bool blocksInput)
        {
            if (fadeElement != null)
            {
                fadeElement.pickingMode = blocksInput ? PickingMode.Position : PickingMode.Ignore;
            }
        }

        protected override void ApplyProgress(float progress)
        {
            switch (progressElement)
            {
                case null:
                    return;
                case AbstractProgressBar progressBar:
                    progressBar.value = Mathf.Lerp(progressBar.lowValue, progressBar.highValue, progress);
                    break;
                default:
                    progressElement.style.width = Length.Percent(progress * 100f);
                    break;
            }
        }
    }
}
