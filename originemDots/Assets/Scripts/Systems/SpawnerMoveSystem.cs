using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMoveSystem : MonoBehaviour
{
    public static SpawnerMoveSystem Instance;

    public float Speed = 5;
    Vector3 pos;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        pos = transform.position;
        pos.z += Speed * Time.deltaTime;
        transform.position = pos;
    }
}
