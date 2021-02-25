using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct StickMovementData : IComponentData
{
    public float Speed;
    public KeyCode leftKey;
    public KeyCode rightKey;
}
