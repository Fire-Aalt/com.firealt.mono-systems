#if FIREALT_UGUI
using ArtificeToolkit.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace FireAlt.MonoSystems
{
    /// <summary>
    /// UGUI transition view. Fades a <see cref="CanvasGroup"/> and, if assigned, fills a progress <see cref="Image"/>.
    /// </summary>
    public class UGUISceneTransitionView : SceneTransitionView
    {
        [Title("UGUI")]
        [SerializeField] protected CanvasGroup canvasGroup;
        [Tooltip("Optional filled Image used as a progress bar.")]
        [SerializeField] protected Image progressFill;

        protected override void ApplyAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        protected override void ApplyBlocksInput(bool blocksInput)
        {
            canvasGroup.blocksRaycasts = blocksInput;
            canvasGroup.interactable = blocksInput;
        }

        protected override void ApplyProgress(float progress)
        {
            if (progressFill != null)
            {
                progressFill.fillAmount = progress;
            }
        }
    }
}
#endif
