using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class SphereFactoryAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count;
        [SerializeField] private Vector3 minPoint;
        [SerializeField] private Vector3 maxPoint;
        
        private class SphereFactoryBaker : Baker<SphereFactoryAuthoring>
        {
            public override void Bake(SphereFactoryAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);
                var config = new SphereFactory
                {
                    Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                    Count = authoring.count,
                    MinPoint = authoring.minPoint,
                    MaxPoint = authoring.maxPoint
                };
                AddComponent(entity, config);
            }
        }
    }
    
    public struct SphereFactory : IComponentData
    {
        public Entity Prefab;
        public int Count;
        public Vector3 MinPoint;
        public Vector3 MaxPoint;
    }
}