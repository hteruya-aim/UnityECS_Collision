using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DefaultNamespace
{
    public partial struct PlaneRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlaneTag>();
            state.RequireForUpdate<RotationConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var rotationConfigs = SystemAPI.GetSingleton<RotationConfig>();
            var deltaTime = SystemAPI.Time.DeltaTime;
            var rotate = quaternion.AxisAngle(rotationConfigs.Axis, rotationConfigs.Speed * deltaTime);
            
            foreach (var planeLocalTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<PlaneTag>())
            {
                var position = planeLocalTransform.ValueRW.Position;
                var rotation = planeLocalTransform.ValueRW.Rotation;
            
                planeLocalTransform.ValueRW.Position = math.mul(rotate, position);
                planeLocalTransform.ValueRW.Rotation = math.mul(rotate, rotation);
            }
        }
    }
}