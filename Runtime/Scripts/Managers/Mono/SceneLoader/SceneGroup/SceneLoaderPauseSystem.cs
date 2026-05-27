using Unity.Entities;

namespace FireAlt.Essentials
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SceneLoaderPauseSystem : ISystem
    {
    }
}