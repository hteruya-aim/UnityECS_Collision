using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DefaultNamespace
{
    [UpdateBefore(typeof(ChangeSphereColorSystem))]
    public partial struct SphereColorInitializeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SphereTag>();   
            state.RequireForUpdate<URPMaterialPropertyBaseColor>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}