using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class GetStickLocationSystem : JobComponentSystem
{
    [ReadOnly] public static GetStickLocationSystem Instance;

    public float3 pos;

    protected override void OnCreate()
    {
        Instance = this;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.WithAll<StickTag>().WithoutBurst().ForEach((ref Translation translation) =>
        {
            pos = translation.Value;
            //Debug.Log(pos);
        }).Run();

        return default;
    }
}
