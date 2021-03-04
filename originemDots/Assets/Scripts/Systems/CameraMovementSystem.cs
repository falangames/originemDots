using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementSystem : MonoBehaviour
{
    public static CameraMovementSystem Instance;

    public float Speed = 5;
    public Vector3 pos;

    public bool canMove = true;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (canMove)
        {
            pos = transform.position;
            pos.z += Speed * Time.deltaTime;
            transform.position = pos;
        }
        else
        {
            pos = new Vector3(0, 2.34f, -4.49f);
            transform.position = pos;
        }
    }
}
