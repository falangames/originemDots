using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class StickMovementSystem : JobComponentSystem
{
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation rotation, ref StickMovementData stickMovementData, in LocalToWorld localTo) =>
        {
            translation.Value.z += stickMovementData.Speed * deltaTime;

            if (Input.GetKeyDown(stickMovementData.leftKey))
            {
                quaternion z = quaternion.RotateZ(45 * Mathf.Deg2Rad);
                rotation.Value = math.mul(rotation.Value, z);
            }
            if (Input.GetKeyDown(stickMovementData.rightKey))
            {
                quaternion z = quaternion.RotateZ(-45 * Mathf.Deg2Rad);
                rotation.Value = math.mul(rotation.Value, z);
            }
        }).Run();

        return default;
    }
}
