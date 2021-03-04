using Unity.Entities;
using Unity.Scenes;

public class ToggleSubSceneSystem : ComponentSystem
{
    /*protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, SubScene scene) => {
            if (EntityManager.HasComponent<RequestSceneLoaded>(entity))
            {
                EntityManager.RemoveComponent<RequestSceneLoaded>(entity);
            }
            else
            {
                EntityManager.AddComponent<RequestSceneLoaded>(entity);
            }
        });

        this.Enabled = false;
    }*/
    protected override void OnUpdate()
    {
        
    }
}