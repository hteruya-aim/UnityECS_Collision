using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class SphereTagAuthoring : MonoBehaviour
    {
        private class SphereTagAuthoringBaker : Baker<SphereTagAuthoring>
        {
            public override void Bake(SphereTagAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<SphereTag>(entity);
            }
        }
    }
}