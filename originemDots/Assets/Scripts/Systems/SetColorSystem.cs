using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class SetColorSystem : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        float r1;
        float r2;
        float r3;
        r1 = Random.Range(0f, 1f);
        r2 = Random.Range(0f, 1f);
        r3 = Random.Range(0f, 1f);
        dstManager.AddComponent<URPMaterialPropertyEmissionColor>(entity);
        URPMaterialPropertyEmissionColor color = dstManager.GetComponentData<URPMaterialPropertyEmissionColor>(entity);
        color.Value.x = r1;
        color.Value.y = r2;
        color.Value.z = r3;
        color.Value.w = 1;
        dstManager.SetComponentData<URPMaterialPropertyEmissionColor>(entity, color);

        dstManager.AddComponent<URPMaterialPropertyBaseColor>(entity);
        URPMaterialPropertyBaseColor color2 = dstManager.GetComponentData<URPMaterialPropertyBaseColor>(entity);
        color.Value.x = r1;
        color.Value.y = r2;
        color.Value.z = r3;
        color.Value.w = 1;
        dstManager.SetComponentData<URPMaterialPropertyBaseColor>(entity, color2);
    }
}
