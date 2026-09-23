#if UNITY_EDITOR
using FireAlt.MonoSystems;
using Unity.Scripting.LifecycleManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace HierarchyRestore
{
    public partial class Startup
    {
        [OnCodeInitializing]
        private static void Initialize()
        {
            if (!GameEssentialsSettingsProvider.RestoreHierarchyEnabled)
            {
                return;
            }

            EditorSceneManager.sceneOpened += OnEditorSceneManagerSceneOpened;
            EditorSceneManager.sceneClosing += OnEditorSceneClosing;
            EditorApplication.quitting += SaveActiveHierarchy;
            EditorApplication.delayCall += LoadActiveHierarchy;
        }

        [OnCodeUnloading]
        private static void Unload()
        {
            EditorSceneManager.sceneOpened -= OnEditorSceneManagerSceneOpened;
            EditorSceneManager.sceneClosing -= OnEditorSceneClosing;
            EditorApplication.quitting -= SaveActiveHierarchy;
            EditorApplication.delayCall -= LoadActiveHierarchy;
        }

        private static void SaveActiveHierarchy() => HierarchyRestorer.SaveActiveHierarchy(SceneManager.GetActiveScene());

        private static void LoadActiveHierarchy() => HierarchyRestorer.LoadActiveHierarchy(SceneManager.GetActiveScene());

        private static void OnEditorSceneManagerSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (!GameEssentialsSettingsProvider.RestoreHierarchyEnabled || !scene.isLoaded)
            {
                return;
            }

            HierarchyRestorer.LoadActiveHierarchy(scene);
        }

        private static void OnEditorSceneClosing(Scene scene, bool removingScene)
        {
            if (!GameEssentialsSettingsProvider.RestoreHierarchyEnabled)
            {
                return;
            }

            HierarchyRestorer.SaveActiveHierarchy(scene);
        }
    }
}
#endif
