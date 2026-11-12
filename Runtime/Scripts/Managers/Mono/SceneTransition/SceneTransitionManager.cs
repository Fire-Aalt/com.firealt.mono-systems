using Cysharp.Threading.Tasks;
using System.Threading;
using ArtificeToolkit.Attributes;
using UnityEngine;

namespace FireAlt.MonoSystems
{
    /// <summary>
    /// Backend-agnostic host for scene transitions. Owns the loading camera, timing and progress reporting,
    /// and delegates all visuals to an optional <see cref="SceneTransitionView"/>.
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] protected SceneTransitionView view;
        [SerializeField] protected Camera loadingCamera;

        [Title("Transition Params")]
        [SerializeField] protected float initializeSceneGroupDuration = 1f;

        public SceneTransitionView View => view;

        public virtual LoadingProgress InitializeProgressBar()
        {
            LoadingProgress progress = new();

            if (view != null)
            {
                view.ResetProgress();
                progress.Progressed += view.ReportProgress;
            }

            return progress;
        }

        public virtual void EnableLoadingCamera(bool enable)
        {
            if (loadingCamera == null)
            {
                return;
            }

            loadingCamera.depth = enable ? 100 : -100;
            loadingCamera.enabled = enable;
        }

        public virtual async UniTask TransitionIn(CancellationToken token)
        {
            if (view != null)
            {
                await view.FadeIn(token);
            }
        }

        public virtual async UniTask TransitionOut(CancellationToken token)
        {
            if (view != null)
            {
                view.CompleteProgress();
            }

            await UniTask.WaitForSeconds(initializeSceneGroupDuration, ignoreTimeScale: true, cancellationToken: token);

            if (view != null)
            {
                await view.FadeOut(token);
            }
        }
    }
}
