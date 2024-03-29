﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;
using Unity.Physics;
using Unity.Collections;
using UnityEngine.SceneManagement;
using Unity.Scenes;

public class GameManager : MonoBehaviour
{
    [ReadOnly] public static GameManager Instance;


    public Transform spawner;
    public float spawnRate = 10f;
    public float nextSpawn = 0f;
    int randomNumberForSpawnAnyCube;


    public GameObject prefabCube1;
    public GameObject prefabCube2;
    public GameObject prefabCube3;
    public GameObject prefabCube4;

    private EntityManager entityManager;
    BlobAssetStore blobAsset;
    GameObjectConversionSettings settings;
    Entity entityCube1;
    Entity entityCube2;
    Entity entityCube3;
    Entity entityCube4;



    public GameObject explosionPrefab;
    public Entity explosioonEntity;


    public GameObject InGamePanel;
    public GameObject PausePanel;
    public GameObject GameOverPanel;

    private void Start()
    {
        Instance = this;
        InGamePanel.SetActive(true);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        blobAsset = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAsset);

        entityCube1 = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabCube1, settings);
        entityCube2 = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabCube2, settings);
        entityCube3 = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabCube3, settings);
        entityCube4 = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabCube4, settings);
        explosioonEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(explosionPrefab, settings);
    }


    private void Update()
    {

    }

    private void FixedUpdate()
    {
        randomNumberForSpawnAnyCube = Random.Range(0, 4);
        nextSpawn += 1f;
        if (nextSpawn > spawnRate)
        {
            nextSpawn = 0;
            if (randomNumberForSpawnAnyCube == 0)
            {
                spawnCube1();
            }
            else if (randomNumberForSpawnAnyCube == 1)
            {
                spawnCube2();
            }
            else if (randomNumberForSpawnAnyCube == 2)
            {
                spawnCube3();
            }
            else if (randomNumberForSpawnAnyCube == 3)
            {
                spawnCube4();
            }
        }
    }

    void spawnCube1()
    {
        Entity cube = entityManager.Instantiate(entityCube1);
        entityManager.SetComponentData(cube, new Translation { Value = spawner.position });
    }
    void spawnCube2()
    {
        Entity cube = entityManager.Instantiate(entityCube2);
        entityManager.SetComponentData(cube, new Translation { Value = spawner.position });
    }
    void spawnCube3()
    {
        Entity cube = entityManager.Instantiate(entityCube3);
        entityManager.SetComponentData(cube, new Translation { Value = spawner.position });
    }
    void spawnCube4()
    {
        Entity cube = entityManager.Instantiate(entityCube4);
        entityManager.SetComponentData(cube, new Translation { Value = spawner.position });
    }

    private SceneSystem sceneSystem;
    public void OnClick_ContinueButton()
    {
        //SceneManager.UnloadSceneAsync(2);
        SceneManager.LoadSceneAsync(1);

        
        //InGamePanel.SetActive(true);
        //PausePanel.SetActive(false);
    }
    public void OnClick_PauseButton()
    {
        //InGamePanel.SetActive(false);
        //PausePanel.SetActive(true);
        //Time.timeScale = 0;
        SceneManager.UnloadSceneAsync(1);
        //SceneManager.LoadSceneAsync(2);
    }
    public void OnClick_RestartButton()
    {
        /*SceneManager.LoadScene("MainScene");
        InGamePanel.SetActive(true);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);*/

        Time.timeScale = 1;
    }
    public void OnClick_AdButton()
    {
        
    }
    public void OnClick_MenuSceneButton()
    {
        Time.timeScale = 1;
    }

    public void UnloadSceneImmediate(Entity scene)
    {
        
    }
}
