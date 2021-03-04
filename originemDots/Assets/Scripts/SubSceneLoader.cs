using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

public class SubSceneLoader : ComponentSystem
{
    private SceneSystem sceneSystem;
    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }

    protected override void OnUpdate()
    {
        //UnLoadSubScene(SubSceneReferences.Instance.MenuScene);
        //UnLoadSubScene(SubSceneReferences.Instance.InGameScene);
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("1");
            LoadSubScene(SubSceneReferences.Instance.InGameScene);
            //UnLoadSubScene(SubSceneReferences.Instance.MenuScene);
            //CameraMovementSystem.Instance.pos = new Vector3(0, 2.34f, -4.491f);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("2");
            UnLoadSubScene(SubSceneReferences.Instance.InGameScene);
            //LoadSubScene(SubSceneReferences.Instance.MenuScene);
            //CameraMovementSystem.Instance.pos = new Vector3(0, 2.34f, -4.491f);
        }
    }
    

    private void LoadSubScene(SubScene subScene)
    {
        sceneSystem.LoadSceneAsync(subScene.SceneGUID);
    }
    private void UnLoadSubScene(SubScene subScene)
    {
        sceneSystem.UnloadScene(subScene.SceneGUID);
    }
}
