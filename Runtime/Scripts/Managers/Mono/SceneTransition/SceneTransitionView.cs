using Cysharp.Threading.Tasks;
using System.Threading;
using ArtificeToolkit.Attributes;
using UnityEngine;

namespace FireAlt.MonoSystems
{
    /// <summary>
    /// Base class for the visual side of a scene transition. Handles fade timing, input blocking state and
    /// progress smoothing.
    /// </summary>
    public abstract class SceneTransitionView : MonoBehaviour
    {
        private const float PROGRESS_SNAP_THRESHOLD = 0.001f;

        [Title("Fade")]
        [SerializeField] protected float fadeDuration = 0.5f;
        [SerializeField] protected bool startVisible = true;

        [Title("Progress")]
        [SerializeField, Min(0f)] protected float fillSpeed = 5f;

        public float Alpha { get; private set; }
        public bool BlocksInput { get; private set; }
        public float Progress { get; private set; }
        public float TargetProgress { get; private set; }

        protected virtual void Awake()
        {
            Alpha = startVisible ? 1f : 0f;
            BlocksInput = startVisible;
            Refresh();
        }

        protected virtual void Update()
        {
            if (Progress == TargetProgress)
            {
                return;
            }

            Progress = Mathf.Lerp(Progress, TargetProgress, 1f - Mathf.Exp(-fillSpeed * Time.unscaledDeltaTime));
            if (Mathf.Abs(TargetProgress - Progress) < PROGRESS_SNAP_THRESHOLD)
            {
                Progress = TargetProgress;
            }

            ApplyProgress(Progress);
        }

        public UniTask FadeIn(CancellationToken token) => Fade(1f, token);

        public UniTask FadeOut(CancellationToken token) => Fade(0f, token);

        public virtual void ResetProgress()
        {
            Progress = 0f;
            TargetProgress = 0f;
            ApplyProgress(Progress);
        }

        public virtual void ReportProgress(float progress)
        {
            TargetProgress = Mathf.Max(TargetProgress, Mathf.Clamp01(progress));
        }

        public virtual void CompleteProgress()
        {
            TargetProgress = 1f;
        }

        protected virtual async UniTask Fade(float targetAlpha, CancellationToken token)
        {
            bool visible = targetAlpha > 0f;
            if (visible)
            {
                SetBlocksInput(true);
            }

            float startAlpha = Alpha;
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration));
                await UniTask.Yield(token);
            }

            SetAlpha(targetAlpha);

            if (!visible)
            {
                SetBlocksInput(false);
            }
        }

        protected void SetAlpha(float alpha)
        {
            Alpha = alpha;
            ApplyAlpha(alpha);
        }

        protected void SetBlocksInput(bool blocksInput)
        {
            BlocksInput = blocksInput;
            ApplyBlocksInput(blocksInput);
        }

        /// <summary> Re-applies the current state, e.g. after the backend's UI has been (re)built. </summary>
        protected void Refresh()
        {
            ApplyAlpha(Alpha);
            ApplyBlocksInput(BlocksInput);
            ApplyProgress(Progress);
        }

        protected abstract void ApplyAlpha(float alpha);

        protected abstract void ApplyBlocksInput(bool blocksInput);

        /// <summary> Called with a normalized [0, 1] value. Implementations without a progress bar may ignore it. </summary>
        protected abstract void ApplyProgress(float progress);
    }
}
