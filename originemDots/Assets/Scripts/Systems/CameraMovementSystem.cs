using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementSystem : MonoBehaviour
{
    public float Speed = 5;
    Vector3 pos;

    private void Update()
    {
        pos = transform.position;
        pos.z += Speed * Time.deltaTime;
        transform.position = pos;
    }
}
