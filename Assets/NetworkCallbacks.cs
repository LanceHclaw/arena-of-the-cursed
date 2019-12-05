using Cinemachine;
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

    private BoltEntity owner;
    private BoltEntity client;

public override void SceneLoadLocalDone(string scene)
    {
        Vector3 spawnPos = new Vector3(Random.Range(fromX, toX), 0.3f, Random.Range(fromZ, toZ));

        var inst = BoltNetwork.Instantiate(BoltPrefabs.Player_1, spawnPos, Quaternion.identity);

        if (inst.IsOwner)
        {
            owner = inst;

            var cam = inst.transform.Find("Camera");
            var cmLook = inst.transform.Find("CM FreeLook");
            var body = inst.transform.Find("Body");

            cam.gameObject.SetActive(true);
            cmLook.gameObject.SetActive(true);

            cmLook.gameObject.GetComponent<CinemachineFreeLook>().Follow = body;
            cmLook.gameObject.GetComponent<CinemachineFreeLook>().LookAt = body;
        }

        else
        {
            client = inst;
        }

        if (owner && client)
        {
            owner.GetComponent<Targeting>().Enemy = client;
            client.GetComponent<Targeting>().Enemy = owner;
        }
    }
}
