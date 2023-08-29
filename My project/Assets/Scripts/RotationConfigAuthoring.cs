using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class RotationConfigAuthoring : MonoBehaviour
    {
        [SerializeField] private Vector3 axis;
        [SerializeField] private float speed;
        
        private class Baker : Baker<RotationConfigAuthoring>
        {
            public override void Bake(RotationConfigAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);
                var config = new RotationConfig
                {
                    Axis = authoring.axis.normalized,
                    Speed = authoring.speed
                };
                AddComponent(entity, config);
            }
        }
    }
    
    public struct RotationConfig : IComponentData
    {
        public Vector3 Axis;
        public float Speed;
    }
}