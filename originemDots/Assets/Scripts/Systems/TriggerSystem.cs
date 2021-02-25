using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Physics;
using Random = UnityEngine.Random;


[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(BuildPhysicsWorld))]
public class TriggerSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    private EntityManager entityManager;
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();


        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    //[BurstCompile]
    struct TriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<StickTag> stickTag;
        [ReadOnly] public ComponentDataFromEntity<CubeTag> cubeTag;
        [ReadOnly] public ComponentDataFromEntity<NoneStickTag> noneStickTag;

        [NativeDisableParallelForRestriction] public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (cubeTag.HasComponent(entityA) && cubeTag.HasComponent(entityB))
            {
                return;
            }

            if (stickTag.HasComponent(entityA) && stickTag.HasComponent(entityB))
            {
                return;
            }

            if (noneStickTag.HasComponent(entityA) && noneStickTag.HasComponent(entityB))
            {
                return;
            }

            if (stickTag.HasComponent(entityA) && noneStickTag.HasComponent(entityB))
            {
                return;
            }

            if (noneStickTag.HasComponent(entityA) && stickTag.HasComponent(entityB))
            {
                return;
            }

            if (stickTag.HasComponent(entityA) && cubeTag.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityB);
                float3 a = GetStickLocationSystem.Instance.pos;

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Entity entityr = entityCommandBuffer.Instantiate(GameManager.Instance.explosioonEntity);
                        entityCommandBuffer.SetComponent(entityr, new Translation { Value = new float3(i * -0.02f, j * -0.02f, a.z) });
                    }
                }

                //GameManager.Instance.spawnSphere(entityCommandBuffer);
            }

            else if (noneStickTag.HasComponent(entityA) && cubeTag.HasComponent(entityB))
            {
                //entityCommandBuffer.AddComponent(entityB, new CubeAnimateTag());
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TriggerSystemJob();
        job.stickTag = GetComponentDataFromEntity<StickTag>(true);
        job.cubeTag = GetComponentDataFromEntity<CubeTag>(true);
        job.noneStickTag = GetComponentDataFromEntity<NoneStickTag>(true);
        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();
        commandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }


}

/*[AlwaysSynchronizeSystem]
public class TriggerSystem : JobComponentSystem
{
    float thresholdDistance = 50f;
    float3 pos;
    float3 stickPos;


    EntityManager manager;

    protected override void OnCreate()
    {
        pos = StickCubeLocation.Instance.pos;

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        pos = StickCubeLocation.Instance.pos;

        Entities.WithAll<StickTag>().WithoutBurst().ForEach((Entity stick, ref Translation stickPosition) =>
        {
            stickPos = stickPosition.Value;
        }).Run();

        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        Entities.WithAll<CubeTag>().WithoutBurst().ForEach((Entity cube, ref Translation cubePos) =>
        {
            if (math.distance(stickPos, cubePos.Value) <= thresholdDistance)
            {
                commandBuffer.DestroyEntity(cube);
                Debug.Log(cubePos.Value);
            }
        }).Run();
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();


        return default;
    }
}*/
/*[AlwaysSynchronizeSystem]
public class TriggerSystem : ComponentSystem
{
    float thresholdDistance = 0.03f;
    float3 stickPosition;
    protected override void OnUpdate()
    {
        Entities.WithAll<CubeTag>().ForEach((Entity cube, ref Translation cubePosition) =>
        {
            Entities.WithAll<StickTag>().ForEach((ref LocalToWorld stickPos) =>
            {
                stickPosition = stickPos.Position;
                //Debug.Log(stickPosition);
            });
            //Debug.Log(cubePosition.Value);
            //float3 enemyPosition = enemyPos.Value;
            if (math.distance(cubePosition.Value.z, stickPosition.z) <= thresholdDistance)
            {
                PostUpdateCommands.DestroyEntity(cube);
                Debug.Log("asdasd");
            }
        });
    }
}*/

/*public class TriggerSystem : JobComponentSystem
{
    private BeginInitializationEntityCommandBufferSystem bufferSystem;
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem endFrameBarier;

    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        endFrameBarier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob triggerJob = new TriggerJob
        {
            stickEntity = GetComponentDataFromEntity<StickTag>(),
            cubeEntity = GetComponentDataFromEntity<CubeTag>(),
            entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(),
            commandBuffer = bufferSystem.CreateCommandBuffer()
        };
        inputDeps = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        endFrameBarier.AddJobHandleForProducer(inputDeps);
        return inputDeps;
    }


    private struct TriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<StickTag> stickEntity;
        [ReadOnly] public ComponentDataFromEntity<CubeTag> cubeEntity;
        [ReadOnly] public ComponentDataFromEntity<DeleteTag> entitiesToDelete;

        public EntityCommandBuffer commandBuffer;

        [System.Obsolete]
        public void Execute(TriggerEvent triggerEvent)
        {
            StickToCube(triggerEvent.Entities.EntityA, triggerEvent.Entities.EntityB);
            StickToCube(triggerEvent.Entities.EntityB, triggerEvent.Entities.EntityA);

            //NoneStickToCube(triggerEvent.Entities.EntityA, triggerEvent.Entities.EntityB);
            //NoneStickToCube(triggerEvent.Entities.EntityB, triggerEvent.Entities.EntityA);
        }

        private void StickToCube(Entity entity1, Entity entity2)
        {
            if (stickEntity.HasComponent(entity1))
            {
                if (cubeEntity.HasComponent(entity2))
                {
                    return;
                }
                commandBuffer.AddComponent(entity2, new DeleteTag());
            }
        }

        private void NoneStickToCube(Entity entity1, Entity entity2)
        {
            if (noneStickEntity.HasComponent(entity1))
            {
                if (cubeEntity.HasComponent(entity2))
                {
                    UnityEngine.Debug.Log("nonestick");
                    return;
                }
                commandBuffer.AddComponent(entity2, new AnimationTag());
            }
        }
    }
}*/

