using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlaneConfigAuthoring : MonoBehaviour
    {
        private class PlaneConfigBaker : Baker<PlaneConfigAuthoring>
        {
            public override void Bake(PlaneConfigAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlaneTag());
            }
        }
    }
}