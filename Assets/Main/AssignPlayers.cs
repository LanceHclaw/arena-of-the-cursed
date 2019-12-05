using UnityEngine;

public class AssignPlayers : MonoBehaviour
{
    public GameObject dummy;
    public GameObject player1;
    public GameObject player2;
    private bool allAssigned = false;

    public void AddPlayer(GameObject player)
    {
        if (player1 == null) player1 = player;
        else
        {
            player2 = player;

            player2.GetComponent<Targeting>().Enemy = player1;
            player1.GetComponent<Targeting>().Enemy = player2;

            allAssigned = true;
        }
    }

    private void Update()
    {
        if (allAssigned)
        {
            if (player1.GetComponent<Status>().localHealth <= 0) Debug.Log("Player2 wins!");
            else if (player2.GetComponent<Status>().localHealth <= 0) Debug.Log("Player1 wins!");
        }
    }
}
