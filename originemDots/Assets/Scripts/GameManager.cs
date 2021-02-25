using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;
using Unity.Physics;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    [ReadOnly] public static GameManager Instance;


    public Transform spawner;
    public float spawnRate = 10f;
    public float nextSpawn = 0f;
    int randomNumberForSpawnAnyCube;


    public GameObject prefabCube1;

    private EntityManager entityManager;
    BlobAssetStore blobAsset;
    GameObjectConversionSettings settings;
    Entity entityCube1;



    public GameObject explosionPrefab;
    public Entity explosioonEntity;

    private void Start()
    {
        Instance = this;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        blobAsset = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAsset);

        entityCube1 = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabCube1, settings);
        explosioonEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(explosionPrefab, settings);
    }


    private void Update()
    {

    }

    private void FixedUpdate()
    {
        randomNumberForSpawnAnyCube = Random.Range(0, 1);
        nextSpawn += 1f;
        if (nextSpawn > spawnRate)
        {
            nextSpawn = 0;
            if (randomNumberForSpawnAnyCube == 0)
            {
                spawnCube1();
            }
        }
    }

    void spawnCube1()
    {
        Entity cube = entityManager.Instantiate(entityCube1);
        entityManager.SetComponentData(cube, new Translation { Value = spawner.position });
    }
}
