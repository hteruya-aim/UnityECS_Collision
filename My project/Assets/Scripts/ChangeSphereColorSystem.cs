using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Mathematics;

namespace DefaultNamespace
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSimulationGroup))] 
    public partial struct ChangeSphereColorSystem : ISystem
    {
        private ComponentLookup<SphereTag> sphereTagLookup;
        private ComponentLookup<PlaneTag> planeTagLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<SphereTag>();
            state.RequireForUpdate<PlaneTag>();
            state.RequireForUpdate<URPMaterialPropertyBaseColor>();
            
            sphereTagLookup = SystemAPI.GetComponentLookup<SphereTag>();
            planeTagLookup = SystemAPI.GetComponentLookup<PlaneTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            sphereTagLookup.Update(ref state);
            planeTagLookup.Update(ref state);
            
            var initializeJob = new InitializeJob();
            state.Dependency = initializeJob.ScheduleParallel(state.Dependency);
            
            var entityCommandBufferSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            
            var job = new CollisionJob
            {
                SphereTagLookup = sphereTagLookup,
                PlaneTagLookup = planeTagLookup,
                EntityCommandBuffer = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            };
            
            state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(SphereTag))]
        private partial struct InitializeJob : IJobEntity
        {
            public void Execute(ref URPMaterialPropertyBaseColor baseColor)
            {
                baseColor.Value = new float4 {x = 1, y = 1, z = 1, w = 1};
            }
        }

        [BurstCompile]
        private struct CollisionJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<SphereTag> SphereTagLookup;
            [ReadOnly] public ComponentLookup<PlaneTag> PlaneTagLookup;
            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(CollisionEvent collisionEvent)
            {
                var entityA = collisionEvent.EntityA;
                var entityB = collisionEvent.EntityB;

                if (SphereTagLookup.HasComponent(entityA) && PlaneTagLookup.HasComponent(entityB))
                {
                    EntityCommandBuffer.SetComponent(entityA, new URPMaterialPropertyBaseColor
                    {
                        Value = new float4 { x = 1, y = 0, z = 0, w = 1 }
                    });
                }
                else if (SphereTagLookup.HasComponent(entityB) && PlaneTagLookup.HasComponent(entityA))
                {
                    EntityCommandBuffer.SetComponent(entityB, new URPMaterialPropertyBaseColor
                    {
                        Value = new float4 { x = 1, y = 0, z = 0, w = 1 }
                    });
                }
            }
        }
    }
}