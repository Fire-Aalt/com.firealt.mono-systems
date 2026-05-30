using Unity.Entities;

namespace FireAlt.MonoSystems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SceneLoaderPauseSystem : ISystem
    {
    }
}