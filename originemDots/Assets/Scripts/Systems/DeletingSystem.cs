using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(TriggerSystem))]
[BurstCompile]
public class DeletingSystem : JobComponentSystem
{
    public float timeLeft = 2f;
    public float timeLeftforCubes = 10f;
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        /*Entities
            .WithoutBurst()
            .WithAll<DeleteTag>()
            .ForEach((Entity entity) =>
            {
                commandBuffer.DestroyEntity(entity);
            }).Run();*/
        Entities
           .WithoutBurst()
           .WithAll<SphereTag>()
           .ForEach((Entity entity) =>
           {
               this.timeLeft -= Time.DeltaTime;

               //destroy gameobject when time is up:
               if (this.timeLeft < 0f)
               {
                   timeLeft = 2f;
                   commandBuffer.DestroyEntity(entity);
               }
           }).Run();

        /*Entities
           .WithoutBurst()
           .WithAll<CubeAnimateTag>()
           .ForEach((Entity entity) =>
           {
               this.timeLeftforCubes -= Time.DeltaTime;
               if (this.timeLeftforCubes < 0f)
               {
                   timeLeftforCubes = 10f;
                   commandBuffer.DestroyEntity(entity);
               }
           }).Run();*/
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}
