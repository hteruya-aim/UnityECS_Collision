using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace DefaultNamespace
{
    public partial struct SphereFactorySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SphereFactory>();   
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var sphereFactory = SystemAPI.GetSingleton<SphereFactory>();
            var prefab = sphereFactory.Prefab;
            var count = sphereFactory.Count;
            var minPoint = sphereFactory.MinPoint;
            var maxPoint = sphereFactory.MaxPoint;
            var random = Random.CreateFromIndex(1);
            
            for (int i = 0; i < count; i++)
            {
                var entity = state.EntityManager.Instantiate(prefab);
                var position =  random.NextFloat3(minPoint, maxPoint);
                var localTransform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                localTransform.ValueRW.Position = position;
            }

            state.Enabled = false;
        }
    }
}