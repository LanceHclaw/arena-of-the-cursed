using UnityEngine;

public class Targeting : Bolt.EntityBehaviour<IPlayerCharacterState>
{
    public AssignPlayers playerListScript;
    public GameObject Enemy;

    public override void Attached()
    {
        playerListScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AssignPlayers>();

        Enemy = playerListScript.dummy;

        playerListScript.AddPlayer(gameObject);
    }
}
