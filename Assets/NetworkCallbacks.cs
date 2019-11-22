using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    [Header("SpawnArea")]
    public float fromX = -22.3472f;
    public float fromZ = -4.793188f;
    public float toX = 27.7528f;
    public float toZ = 9.906813f;
    public override void SceneLoadLocalDone(string scene)
    {
        Vector3 spawnPos = new Vector3(Random.Range(fromX, toX), 0.3f, Random.Range(fromZ, toZ));

        BoltNetwork.Instantiate(BoltPrefabs.Player_1, spawnPos, Quaternion.identity);
    }
}
