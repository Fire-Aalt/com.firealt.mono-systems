using Unity.Entities;

namespace FireAlt.Mono
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SceneLoaderPauseSystem : ISystem
    {
    }
}