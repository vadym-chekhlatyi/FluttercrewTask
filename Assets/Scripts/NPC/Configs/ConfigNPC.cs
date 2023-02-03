using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/ConfigNPC")]
public class ConfigNPC : ScriptableObject
{
    public float walkSpeed = 3f;
    public float idleTime = 8f;

    public int minWalkRange = 1;
    public int maxWalkRange = 3;

    public float maxDistanceToSpawn = 10;
}
